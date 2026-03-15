using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Filament;
using _3DMANAGER_APP.BLL.Models.File;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.Filament;
using _3DMANAGER_APP.DAL.Models.File;
using _3DMANAGER_APP.DAL.Models.Print;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace _3DMANAGER_APP.BLL.Managers
{
    public class FilamentManager : IFilamentManager
    {
        private readonly IFilamentDbManager _filamentDbManager;
        private readonly IMapper _mapper;
        private readonly ILogger<FilamentManager> _logger;
        private readonly IAwsS3Service _awsS3Service;
        public FilamentManager(IFilamentDbManager filamentDbManager, IMapper mapper, ILogger<FilamentManager> logger, IAwsS3Service awsS3Service)
        {
            _filamentDbManager = filamentDbManager;
            _mapper = mapper;
            _logger = logger;
            _awsS3Service = awsS3Service;
        }

        public List<FilamentListResponse> GetFilamentList(int group, out BaseError? error)
        {
            error = null;
            List<FilamentListResponseDbObject> list = _filamentDbManager.GetFilamentList(group);
            if (list == null || list.Count == 0)
                error = new BaseError() { code = (int)HttpStatusCode.InternalServerError, message = "Error al obtener listado de filamentos" };

            return _mapper.Map<List<FilamentListResponse>>(list);
        }

        public async Task<CommonResponse<int>> PostFilament(FilamentRequest filament)
        {
            CommonResponse<int> response = new CommonResponse<int>();

            FilamentRequestDbObject filamentDbObject = _mapper.Map<FilamentRequestDbObject>(filament);
            var responseDb = _filamentDbManager.PostFilament(filamentDbObject, out int? errorDb);

            if (errorDb != null)
            {
                string msg = "";
                switch (errorDb)
                {
                    case 1:
                        msg = $"Error al crear filamento con nombre {filament.FilamentName}.";
                        _logger.LogError(msg);
                        response.Error = new Response.ErrorProperties() { Code = StatusCodes.Status409Conflict, Message = msg };
                        break;
                    case 500:
                        msg = $"Error al crear filamento en el servidor.";
                        _logger.LogError(msg);
                        response.Error = new Response.ErrorProperties() { Code = StatusCodes.Status500InternalServerError, Message = msg };
                        break;
                    default:
                        break;
                }
                return response;
            }
            response.Data = responseDb;
            if (filament.ImageFile != null)
            {
                bool responseImage = await UpdateS3FilamentImage(responseDb, filament.ImageFile, filament.GroupId);
                if (!responseImage)
                {
                    string msg = $"El filamento {filament.FilamentName} se ha creado correctamente, pero la imagen ha fallado al ser guardada.";
                    _logger.LogError(msg);
                    response.Error = new Response.ErrorProperties() { Code = StatusCodes.Status409Conflict, Message = msg };
                }
            }
            return response;
        }
        public async Task<bool> UpdateS3FilamentImage(int filamentId, IFormFile imageFile, int groupId)
        {
            FileResponse? image = null;

            if (imageFile != null)
            {
                image = await _awsS3Service.UploadImageAsync(imageFile.OpenReadStream(), imageFile.FileName,
                    imageFile.ContentType, "filaments", groupId);
                if (image != null)
                    return _filamentDbManager.UpdateFilamentImageData(filamentId, _mapper.Map<FileResponseDbObject>(image));
                else return false;
            }
            else
            {
                return false;
            }

        }

        public bool UpdateFilament(FilamentUpdateRequest request)
        {
            FilamentUpdateRequestDbObject requestDb = _mapper.Map<FilamentUpdateRequestDbObject>(request);
            return _filamentDbManager.UpdateFilament(requestDb);
        }

        public FilamentDetailObject GetFilamentDetail(int groupId, int filamentId, out BaseError? error)
        {
            error = null;
            var responseDb = _filamentDbManager.GetFilamentDetail(groupId, filamentId);
            if (responseDb.FilamentId == 0)
            {
                string msg = $"Error al obtener el detalle de filamento {filamentId}.";
                _logger.LogError(msg);
                error = new BaseError()
                {
                    code = StatusCodes.Status500InternalServerError,
                    message = msg
                };
                return new FilamentDetailObject();
            }
            var response = _mapper.Map<FilamentDetailObject>(responseDb);

            if (response != null)
            {
                if (response.FilamentImageFile != null && response.FilamentImageFile.FileUrl != null && response.FilamentImageFile.FileKey != null)
                    response.FilamentImageFile.FileUrl = _awsS3Service.GetPresignedUrl(response.FilamentImageFile.FileKey, 1);
                else
                    response.FilamentImageFile!.FileUrl = _awsS3Service.GetPresignedUrl("default/3dmanager-default-filament.png", 1);

            }
            return response!;
        }

        public async Task<CommonResponse<bool>> DeleteFilament(int filamentId, int groupId)
        {
            CommonResponse<bool> response = new CommonResponse<bool>();

            DeletedDbObject responseDb = _filamentDbManager.DeleteFilament(filamentId, groupId, out int? errorDb);

            if (errorDb != null)
            {
                string msg = $"Error al eliminar el filamento con id: {filamentId}.";
                _logger.LogError(msg);
                response.Error = new Response.ErrorProperties() { Code = StatusCodes.Status500InternalServerError, Message = msg };
            }
            response.Data = responseDb.SuccesfullDelete;
            if (responseDb.FileResponse != null)
            {
                //Aun no implementado
            }
            return response;
        }
    }
}
