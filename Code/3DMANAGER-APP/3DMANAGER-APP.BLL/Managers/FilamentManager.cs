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
using static _3DMANAGER_APP.BLL.Models.Base.Response;

namespace _3DMANAGER_APP.BLL.Managers
{
    public class FilamentManager : IFilamentManager
    {
        private readonly IFilamentDbManager _filamentDbManager;
        private readonly IMapper _mapper;
        private readonly ILogger<FilamentManager> _logger;
        private readonly IAzureBlobStorageService _absService;
        private readonly INotificationManager _notificationManager;
        public FilamentManager(IFilamentDbManager filamentDbManager, IMapper mapper, ILogger<FilamentManager> logger, IAzureBlobStorageService absService, INotificationManager notificationManager)
        {
            _filamentDbManager = filamentDbManager;
            _mapper = mapper;
            _logger = logger;
            _absService = absService;
            _notificationManager = notificationManager;
        }

        public List<FilamentListResponse> GetFilamentList(int group, out BaseError? error)
        {
            error = null;
            List<FilamentListResponseDbObject> list = _filamentDbManager.GetFilamentList(group, out bool errordb);
            if (errordb)
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
                bool responseImage = await UpdateABSFilamentImage(responseDb, filament.ImageFile, filament.GroupId);
                if (!responseImage)
                {
                    string msg = $"El filamento {filament.FilamentName} se ha creado correctamente, pero la imagen ha fallado al ser guardada.";
                    _logger.LogError(msg);
                    response.Error = new Response.ErrorProperties() { Code = StatusCodes.Status409Conflict, Message = msg };
                }
            }
            return response;
        }
        public async Task<bool> UpdateABSFilamentImage(int filamentId, IFormFile imageFile, int groupId)
        {
            FileResponse? image = null;

            if (imageFile != null)
            {
                image = await _absService.UploadImageAsync(imageFile.OpenReadStream(), imageFile.FileName,
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
                    response.FilamentImageFile.FileUrl = _absService.GetPresignedUrl(response.FilamentImageFile.FileKey, 1);
                else
                    response.FilamentImageFile!.FileUrl = _absService.GetPresignedUrl("default/3dmanager-default-filament.png", 1);

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
            if (responseDb.FileResponse != null && responseDb.FileResponse.FileKey != null)
            {
                var responseImage = await DeleteFilamentImage(filamentId, groupId);
                if (!responseImage.Data)
                {
                    string msg = $"Filamento eliminado correctamente. Pero ha ocurrido un error al eliminar la imagen en Azure Blob Storage del fichero {responseDb.FileResponse.FileKey}.";
                    response.Error = new ErrorProperties(StatusCodes.Status500InternalServerError, msg);
                    return response;
                }
            }
            return response;
        }

        public async Task<CommonResponse<bool>> DeleteFilamentImage(int filamentId, int groupId)
        {
            CommonResponse<bool> response = new CommonResponse<bool>();

            FileResponseDbObject imageData = _filamentDbManager.GetFilamentImageData(filamentId, groupId, out bool error);

            if (error)
            {
                response.Error = new ErrorProperties(StatusCodes.Status404NotFound, "Ha ocurrido un error al buscar en el filamento la imagen asociada.");
                return response;
            }
            else if (imageData.FileKey == null && !error)
            {
                response.Data = true;
                return response;
            }

            await _absService.DeleteImageAsync(imageData!.FileKey!);
            bool dbResponse = _filamentDbManager.DeleteFilamentImageData(filamentId, groupId);

            if (!dbResponse)
            {
                response.Error = new ErrorProperties(StatusCodes.Status500InternalServerError, "Error al eliminar la imagen en la base de datos.");
                return response;
            }

            response.Data = true;
            return response;
        }

        public async Task<CommonResponse<bool>> UpdateFilamentImage(int filamentId, int groupId, IFormFile imageFile)
        {
            CommonResponse<bool> response = new CommonResponse<bool>();
            response.Data = false;
            if (imageFile == null)
            {
                response.Error = new ErrorProperties(StatusCodes.Status400BadRequest, "Error, no se ha recibido una imagen para actualizar");
                return response;
            }

            var aBSResponse = await _absService.UploadImageAsync(imageFile.OpenReadStream(), imageFile.FileName, imageFile.ContentType, "filaments", groupId);
            if (aBSResponse == null)
            {
                response.Error = new ErrorProperties(StatusCodes.Status409Conflict, "Error al subir la imagen a Azure Blob Storage.");
                return response;
            }
            var deletedImage = await DeleteFilamentImage(filamentId, groupId);
            if (!deletedImage.Data)
            {
                var fileData = _filamentDbManager.GetFilamentImageData(filamentId, groupId, out bool errorDbImage);
                string? keyValue = errorDbImage ? "FileKey Desconocido" : fileData.FileKey;
                string msg = $"Se ha intentado eliminar una foto del filamento {filamentId} del grupo {groupId} con el fileKey {keyValue}";
                _logger.LogError(msg);
            }
            bool dbResponse = _filamentDbManager.UpdateFilamentImageData(filamentId, _mapper.Map<FileResponseDbObject>(aBSResponse));
            if (!dbResponse)
            {
                response.Error = new ErrorProperties(StatusCodes.Status500InternalServerError, "Error al actualizar la imagen en la base de datos.");
                return response;
            }

            response.Data = true;
            return response;
        }

        public async Task CheckFilamentLevelsAsync()
        {
            try
            {
                List<FilamentNotificationDbObject> filaments = _filamentDbManager.GetAllFilaments();

                foreach (var filament in filaments)
                {

                    string msg = $"El filamento {filament.FilamentName} se encuentra por debajo del umbral del 25% de material restante, con  {filament.FilamentLength} de material restante.";

                    bool responseNotification = _notificationManager.CreateNotification(filament.OwnerGroupId, 0, Models.Notifications.NotificationType.FilamentWarning, msg, out BaseError? error);
                    if (error != null || !responseNotification)
                    {
                        string msgError = $"Error: Error al generar notificación de filamento {filament.FilamentId} bajo de material :{error}";
                        _logger.LogError(msgError);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en CheckFilamentLevelsAsync al generar las notificaciones de filamento con bajo nivel de material");
            }
        }

    }
}
