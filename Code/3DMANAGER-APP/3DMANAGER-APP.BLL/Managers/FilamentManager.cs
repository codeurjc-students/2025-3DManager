using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Filament;
using _3DMANAGER_APP.BLL.Models.File;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.Filament;
using _3DMANAGER_APP.DAL.Models.File;
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
            if (list == null)
                error = new BaseError() { code = (int)HttpStatusCode.InternalServerError, message = "Error al obtener listado de filamentos" };

            return _mapper.Map<List<FilamentListResponse>>(list);
        }

        public async Task<CommonResponse<int>> PostFilament(FilamentRequest filament)
        {
            CommonResponse<int> response = new CommonResponse<int>(); ;

            FilamentRequestDbObject filamentDbObject = _mapper.Map<FilamentRequestDbObject>(filament);
            var responseDb = _filamentDbManager.PostFilament(filamentDbObject, out int? errorDb);

            if (errorDb != null)
            {
                string msg = "";
                switch (errorDb)
                {
                    case 1:
                        msg = $"Error al crear filamento con nombre {filament.FilamentName}";
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
                    string msg = "El filamento se ha creado correctamente, pero la imagen ha fallado al ser guardada.";
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
    }
}
