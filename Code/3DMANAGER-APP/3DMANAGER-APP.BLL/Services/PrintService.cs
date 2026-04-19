using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.File;
using _3DMANAGER_APP.BLL.Models.Print;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.File;
using _3DMANAGER_APP.DAL.Models.Print;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using static _3DMANAGER_APP.BLL.Models.Base.Response;

namespace _3DMANAGER_APP.BLL.Services
{
    public class PrintService : IPrintService
    {
        private readonly IPrintRepository _printRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PrintService> _logger;
        private readonly IAzureBlobStorageService _absService;
        private readonly INotificationService _notificationManager;
        public PrintService(IPrintRepository printRepository, IMapper mapper, ILogger<PrintService> logger, IAzureBlobStorageService absService, INotificationService notificationManager)
        {
            _printRepository = printRepository;
            _mapper = mapper;
            _logger = logger;
            _absService = absService;
            _notificationManager = notificationManager;
        }

        public PrintListResponse GetPrintList(int group, PagedRequest pagination, out BaseError? error)
        {
            error = null;
            var result = _printRepository.GetPrintList(group, pagination.PageNumber, pagination.PageSize, out int totalItems, out bool errorDb);
            if (errorDb)
            {
                error = new BaseError()
                {
                    code = (int)HttpStatusCode.InternalServerError,
                    message = "Error al obtener listado de impresiones"
                };
            }
            var printsList = _mapper.Map<List<PrintResponse>>(result);

            return new PrintListResponse
            {
                prints = printsList,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling((double)totalItems / pagination.PageSize)
            };
        }

        public async Task<CommonResponse<int>> PostPrint(PrintRequest print)
        {
            CommonResponse<int> response = new CommonResponse<int>();

            PrintRequestDbObject printDbObject = _mapper.Map<PrintRequestDbObject>(print);
            int responseDb = _printRepository.PostPrint(printDbObject, out int? errorDb);

            if (errorDb != null)
            {
                string msg = "";
                switch (errorDb)
                {
                    case 1:
                        msg = $"Error al crear impresion con nombre {print.PrintName}";
                        _logger.LogError(msg);
                        response.Error = new Response.ErrorProperties() { Code = StatusCodes.Status409Conflict, Message = msg };
                        break;
                    case 500:
                        msg = $"Error al crear impresion en el servidor.";
                        _logger.LogError(msg);
                        response.Error = new Response.ErrorProperties() { Code = StatusCodes.Status500InternalServerError, Message = msg };
                        break;
                    default:
                        break;
                }

            }
            response.Data = responseDb;
            if (print.ImageFile != null)
            {
                bool responseImage = await UpdateABSPrintImage(responseDb, print.ImageFile, print.GroupId);
                if (!responseImage)
                {
                    string msg = "La impresion 3d se ha creado correctamente, pero la imagen ha fallado al ser guardada.";
                    _logger.LogError(msg);
                    response.Error = new Response.ErrorProperties() { Code = StatusCodes.Status409Conflict, Message = msg };
                }
            }
            return response;
        }

        public async Task<bool> UpdateABSPrintImage(int printerId, IFormFile imageFile, int groupId)
        {
            FileResponse? image = null;

            if (imageFile != null)
            {
                image = await _absService.UploadImageAsync(imageFile.OpenReadStream(), imageFile.FileName,
                    imageFile.ContentType, "prints", groupId);
                if (image != null)
                    return _printRepository.UpdatePrintImageData(printerId, _mapper.Map<FileResponseDbObject>(image));
                else return false;
            }
            else
            {
                return false;
            }

        }

        public PrintListResponse GetPrintListByType(int group, PagedRequest pagination, int type, int id, out BaseError? error)
        {
            error = null;
            List<PrintListResponseDbObject> result;
            result = _printRepository.GetPrintListByType(group, pagination.PageNumber, pagination.PageSize, type, id, out int totalItems, out bool errorDb);
            if (errorDb)
            {
                error = new BaseError()
                {
                    code = (int)HttpStatusCode.InternalServerError,
                    message = "Error al obtener listado de impresiones para el detalle"
                };
            }
            var printsList = _mapper.Map<List<PrintResponse>>(result);

            return new PrintListResponse
            {
                prints = printsList,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling((double)totalItems / pagination.PageSize)
            };
        }

        public bool UpdatePrint(PrintDetailRequest request)
        {
            PrintDetailRequestDbObject requestDb = _mapper.Map<PrintDetailRequestDbObject>(request);
            return _printRepository.UpdatePrint(requestDb);
        }

        public PrintDetailObject GetPrintDetail(int groupId, int printId, out BaseError? error)
        {
            error = null;
            PrintDetailObject response;
            var responseDb = _printRepository.GetPrintDetail(groupId, printId);
            if (responseDb.PrintId == 0)
            {
                string msg = $"Error al obtener el detalle de impresión {printId}";
                _logger.LogError(msg);
                error = new BaseError()
                {
                    code = StatusCodes.Status500InternalServerError,
                    message = msg
                };
            }
            response = _mapper.Map<PrintDetailObject>(responseDb);

            if (response != null)
            {
                if (response.PrintImageData != null && response.PrintImageData.FileUrl != null && response.PrintImageData.FileKey != null)
                    response.PrintImageData.FileUrl = _absService.GetPresignedUrl(response.PrintImageData.FileKey, 1);
                else
                    response.PrintImageData!.FileUrl = _absService.GetPresignedUrl("default/3dbenchy.stl", 1);

            }
            return response!;
        }

        public List<PrintCommentObject> GetPrintComments(int groupId, int printId, out BaseError? error)
        {
            error = null;

            var dbResult = _printRepository.GetPrintComments(groupId, printId, out bool errorDb);

            if (errorDb)
            {
                string msg = $"Error obteniendo comentarios de la impresión {printId}";
                _logger.LogError(msg);
                error = new BaseError
                {
                    code = 500,
                    message = msg
                };
                return new List<PrintCommentObject>();
            }

            return _mapper.Map<List<PrintCommentObject>>(dbResult);
        }


        public int PostPrintComment(PrintCommentRequest request, int groupId)
        {
            PrintCommentRequestDbObject requestDb = _mapper.Map<PrintCommentRequestDbObject>(request);
            int response = _printRepository.PostPrintComment(requestDb);
            if (response == 0)
            {
                string msgLog = $"Error haciendo comentarios de la impresión {request.PrintId}";
                _logger.LogError(msgLog);
                return 0;
            }

            var responseDb = _printRepository.GetPrintDetail(groupId, request.PrintId);
            if (responseDb.PrintId == 0)
            {
                _logger.LogError("Error al obtener el datos para generar notificacion del comentario de impresión");
                return 0;
            }
            if (responseDb.PrintUserId != request.UserId)
            {
                string msg = $"Alguien ha realizado un comentario sobre tu impresión {responseDb.PrintName}";
                _notificationManager.CreateNotification(responseDb.PrintUserId, request.UserId, Models.Notifications.NotificationType.PrintComment, msg, out BaseError? error);
                if (error != null)
                    return 0;
            }


            return response;
        }

        public async Task<CommonResponse<bool>> DeletePrint(int printId, int groupId)
        {
            CommonResponse<bool> response = new CommonResponse<bool>();

            DeletedDbObject responseDb = _printRepository.DeletePrint(printId, groupId, out int? errorDb);

            if (errorDb != null)
            {
                string msg = $"Error al eliminar impresión con id: {printId}";
                _logger.LogError(msg);
                response.Error = new Response.ErrorProperties() { Code = StatusCodes.Status500InternalServerError, Message = msg };
            }
            response.Data = responseDb.SuccesfullDelete;
            if (responseDb.FileResponse != null && responseDb.FileResponse.FileKey != null)
            {
                var responseImage = await DeletePrintImage(printId, groupId);
                if (!responseImage.Data)
                {
                    string msg = $"Impresión eliminada correctamente. Pero ha ocurrido un error al eliminar el ficher STL en Azure Blob Storage del fichero {responseDb.FileResponse.FileKey}.";
                    response.Error = new ErrorProperties(StatusCodes.Status500InternalServerError, msg);
                    return response;
                }
            }
            return response;
        }

        public async Task<CommonResponse<bool>> DeletePrintImage(int printId, int groupId)
        {
            CommonResponse<bool> response = new CommonResponse<bool>();

            FileResponseDbObject imageData = _printRepository.GetPrintImageData(printId, groupId, out bool error);

            if (error)
            {
                response.Error = new ErrorProperties(StatusCodes.Status404NotFound, "Ha ocurrido un error al buscar en la impresión el fichero STL asociado.");
                return response;
            }
            else if (imageData.FileKey == null && !error)
            {
                response.Data = true;
                return response;
            }

            await _absService.DeleteImageAsync(imageData!.FileKey!);
            bool dbResponse = _printRepository.DeletePrintImageData(printId, groupId);

            if (!dbResponse)
            {
                response.Error = new ErrorProperties(StatusCodes.Status500InternalServerError, "Error al eliminar referencia del fichero STL en la base de datos.");
                return response;
            }

            response.Data = true;
            return response;
        }

        public async Task<CommonResponse<bool>> UpdatePrintImage(int printId, int groupId, IFormFile imageFile)
        {
            CommonResponse<bool> response = new CommonResponse<bool>();
            response.Data = false;
            if (imageFile == null)
            {
                response.Error = new ErrorProperties(StatusCodes.Status400BadRequest, "Error, no se ha recibido una imagen para actualizar");
                return response;
            }

            var aBSResponse = await _absService.UploadImageAsync(imageFile.OpenReadStream(), imageFile.FileName, imageFile.ContentType, "prints", groupId);
            if (aBSResponse == null)
            {
                response.Error = new ErrorProperties(StatusCodes.Status409Conflict, "Error al subir fichero STL a Azure Blob Storage.");
                return response;
            }
            var deletedImage = await DeletePrintImage(printId, groupId);
            if (!deletedImage.Data)
            {
                var fileData = _printRepository.GetPrintImageData(printId, groupId, out bool errorDbImage);
                string? keyValue = errorDbImage ? "FileKey Desconocido" : fileData.FileKey;
                string msg = $"Se ha intentado eliminar un fichero STL de la impresión {printId} del grupo {groupId} con el fileKey {keyValue}";
                _logger.LogError(msg);
            }
            bool dbResponse = _printRepository.UpdatePrintImageData(printId, _mapper.Map<FileResponseDbObject>(aBSResponse));
            if (!dbResponse)
            {
                response.Error = new ErrorProperties(StatusCodes.Status500InternalServerError, "Error al actualizar la referencia del fichero STL en la base de datos.");
                return response;
            }

            response.Data = true;
            return response;
        }

        public bool DeletePrintComment(int commentId)
        {
            return _printRepository.DeletePrintComment(commentId);
        }
    }
}
