using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.File;
using _3DMANAGER_APP.BLL.Models.Printer;
using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.File;
using _3DMANAGER_APP.DAL.Models.Print;
using _3DMANAGER_APP.DAL.Models.Printer;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using static _3DMANAGER_APP.BLL.Models.Base.Response;

namespace _3DMANAGER_APP.BLL.Managers
{
    public class PrinterManager : IPrinterManager
    {
        private readonly IPrinterDbManager _printerDbManager;
        private readonly IMapper _mapper;
        private readonly ILogger<PrinterManager> _logger;
        private readonly IAwsS3Service _awsS3Service;
        public PrinterManager(IPrinterDbManager printerDbManager, IMapper mapper, ILogger<PrinterManager> logger, IAwsS3Service awsS3Service)
        {
            _printerDbManager = printerDbManager;
            _mapper = mapper;
            _logger = logger;
            _awsS3Service = awsS3Service;
        }

        public List<PrinterObject> GetPrinterList(out BaseError? error)
        {
            error = null;
            List<PrinterObject> response = null;
            var responseDb = _printerDbManager.GetPrinterList(out ErrorDbObject errorDb);
            if (errorDb != null)
            {
                _logger.LogError("Error al obtener el listado de impresoras");
                error = new BaseError()
                {
                    code = errorDb.code,
                    message = errorDb.message
                };
            }
            response = _mapper.Map<List<PrinterObject>>(responseDb);
            return response;
        }

        public async Task<CommonResponse<int>> PostPrinter(PrinterRequest printer)
        {

            CommonResponse<int> response = new CommonResponse<int>();
            PrinterRequestDbObject printerDbObject = _mapper.Map<PrinterRequestDbObject>(printer);
            var responseDb = _printerDbManager.PostPrinter(printerDbObject, out int? errorDb);

            if (errorDb != null && errorDb > 0)
            {
                string msg = "";
                switch (errorDb)
                {
                    case 1:
                        msg = $"Error al crear impresora con nombre {printer.PrinterName}. El nombre de la impresora ya existe";
                        _logger.LogError(msg);
                        response.Error = new Response.ErrorProperties() { Code = StatusCodes.Status409Conflict, Message = msg };
                        break;
                    case 500:
                        msg = $"Error al crear impresora en el servidor.";
                        _logger.LogError(msg);
                        response.Error = new Response.ErrorProperties() { Code = StatusCodes.Status500InternalServerError, Message = msg };
                        break;
                    default:
                        break;
                }

                return response;
            }
            response.Data = responseDb;
            if (printer.ImageFile != null)
            {
                bool responseImage = await UpdateS3PrinterImage(responseDb, printer.ImageFile, printer.GroupId);
                if (!responseImage)
                {
                    string msg = "La impresora se ha creado correctamente, pero la imagen ha fallado al ser guardada.";
                    _logger.LogError(msg);
                    response.Error = new Response.ErrorProperties() { Code = StatusCodes.Status409Conflict, Message = msg };
                }
            }
            return response;
        }
        public async Task<bool> UpdateS3PrinterImage(int printerId, IFormFile imageFile, int groupId)
        {
            FileResponse? image = null;

            if (imageFile != null)
            {
                image = await _awsS3Service.UploadImageAsync(imageFile.OpenReadStream(), imageFile.FileName,
                    imageFile.ContentType, "printers", groupId);
                if (image != null)
                    return _printerDbManager.UpdatePrinterImageData(printerId, _mapper.Map<FileResponseDbObject>(image));
                else return false;
            }
            else
            {
                return false;
            }

        }
        public List<PrinterListObject> GetPrinterDashboardList(int groupId, out BaseError? error)
        {
            error = null;
            List<PrinterListDbObject> list = _printerDbManager.GetPrinterDashboardList(groupId, out bool errorDb);
            if (errorDb)
                error = new BaseError() { code = (int)HttpStatusCode.InternalServerError, message = "Error al obtener listado de impresoras" };

            List<PrinterListObject> response = _mapper.Map<List<PrinterListObject>>(list);
            if (response != null)
            {
                foreach (PrinterListObject p in response)
                {
                    if (p.PrinterImageData != null && p.PrinterImageData.FileUrl != null && p.PrinterImageData.FileKey != null)
                        p.PrinterImageData.FileUrl = _awsS3Service.GetPresignedUrl(p.PrinterImageData.FileKey, 1);
                    else
                        p.PrinterImageData!.FileUrl = _awsS3Service.GetPresignedUrl("default/3dmanager-default-printer.jpg", 1);
                }
            }

            return response!;
        }

        public bool UpdatePrinter(PrinterDetailRequest request)
        {
            PrinterDetailRequestDbObject requestDb = _mapper.Map<PrinterDetailRequestDbObject>(request);
            return _printerDbManager.UpdatePrinter(requestDb);
        }

        public PrinterDetailObject GetPrinterDetail(int groupId, int printerId, out BaseError? error)
        {
            error = null;
            PrinterDetailObject response;
            var responseDb = _printerDbManager.GetPrinterDetail(groupId, printerId);
            if (responseDb.PrinterId == 0)
            {
                string msg = $"Error al obtener el detalle de impresora {printerId}";
                _logger.LogError(msg);
                error = new BaseError()
                {
                    code = StatusCodes.Status500InternalServerError,
                    message = msg
                };
            }
            response = _mapper.Map<PrinterDetailObject>(responseDb);

            if (response != null)
            {
                if (response.PrinterImageData != null && response.PrinterImageData.FileUrl != null && response.PrinterImageData.FileKey != null)
                    response.PrinterImageData.FileUrl = _awsS3Service.GetPresignedUrl(response.PrinterImageData.FileKey, 1);
                else
                    response.PrinterImageData!.FileUrl = _awsS3Service.GetPresignedUrl("default/3dmanager-default-printer.jpg", 1);

                response.PrinterTimeVariation = GetPrinterTimeVariation(groupId, printerId, out error);
            }

            return response!;
        }

        public float GetPrinterTimeVariation(int groupId, int printerId, out BaseError? error)
        {
            error = null;
            var responseDb = _printerDbManager.GetTimeVariation(groupId, printerId, out bool errorDb);
            if (errorDb)
            {
                string msg = $"Error al obtener el listado de tiempos de impresión de la impresora {printerId}";
                _logger.LogError(msg);
                error = new BaseError()
                {
                    code = StatusCodes.Status500InternalServerError,
                    message = msg
                };
            }
            var variations = responseDb?.Count > 0 ?
            responseDb.Where(time => time.PrinterTimeImpresion > 0)
            .Average(time => ((float)(time.PrinterRealTimeImpresion - time.PrinterTimeImpresion) / time.PrinterTimeImpresion) * 100
            ) : 0;

            return variations;
        }

        public async Task<CommonResponse<bool>> DeletePrinter(int printerId, int groupId)
        {
            CommonResponse<bool> response = new CommonResponse<bool>();

            DeletedDbObject responseDb = _printerDbManager.DeletePrinter(printerId, groupId, out int? errorDb);

            if (errorDb != null)
            {
                string msg = $"Error al eliminar impresora con id: {printerId}";
                _logger.LogError(msg);
                response.Error = new Response.ErrorProperties() { Code = StatusCodes.Status500InternalServerError, Message = msg };
            }
            response.Data = responseDb.SuccesfullDelete;
            if (responseDb.FileResponse != null)
            {
                var responseImage = await DeletePrinterImage(printerId, groupId);
                if (!responseImage.Data)
                {
                    string msg = $"Impresora eliminada correctamento. Pero ha ocurrido un error al eliminar la imagen en S3 del fichero {responseDb.FileResponse.FileKey}.";
                    response.Error = new ErrorProperties(StatusCodes.Status500InternalServerError, msg);
                    return response;
                }
            }
            return response;
        }

        public async Task<CommonResponse<bool>> DeletePrinterImage(int printerId, int groupId)
        {
            CommonResponse<bool> response = new CommonResponse<bool>();

            FileResponseDbObject imageData = _printerDbManager.GetPrinterImageData(printerId, groupId, out bool error);

            if (error)
            {
                response.Error = new ErrorProperties(StatusCodes.Status404NotFound, "Ha ocurrido un error al buscar en la impresora la imagen asociada.");
                return response;
            }
            else if (imageData.FileKey == null && !error)
            {
                response.Data = true;
                return response;
            }

            await _awsS3Service.DeleteImageAsync(imageData!.FileKey!);
            bool dbResponse = _printerDbManager.DeletePrinterImageData(printerId, groupId);

            if (!dbResponse)
            {
                response.Error = new ErrorProperties(StatusCodes.Status500InternalServerError, "Error al eliminar la imagen en la base de datos.");
                return response;
            }

            response.Data = true;
            return response;
        }

        public async Task<CommonResponse<bool>> UpdatePrinterImage(int printerId, int groupId, IFormFile imageFile)
        {
            CommonResponse<bool> response = new CommonResponse<bool>();
            response.Data = false;
            if (imageFile == null)
            {
                response.Error = new ErrorProperties(StatusCodes.Status400BadRequest, "Error, no se ha recibido una imagen para actualizar");
                return response;
            }

            var s3Response = await _awsS3Service.UploadImageAsync(imageFile.OpenReadStream(), imageFile.FileName, imageFile.ContentType, "printers", groupId);
            if (s3Response == null)
            {
                response.Error = new ErrorProperties(StatusCodes.Status409Conflict, "Error al subir la imagen a S3.");
                return response;
            }
            var deletedImage = await DeletePrinterImage(printerId, groupId);
            if (!deletedImage.Data)
            {
                var fileData = _printerDbManager.GetPrinterImageData(printerId, groupId, out bool errorDbImage);
                string? keyValue = errorDbImage ? "FileKey Desconocido" : fileData.FileKey;
                string msg = $"Se ha intentado eliminar una foto de la impresora {printerId} del grupo {groupId} con el fileKey {keyValue}";
                _logger.LogError(msg);
            }
            bool dbResponse = _printerDbManager.UpdatePrinterImageData(printerId, _mapper.Map<FileResponseDbObject>(s3Response));
            if (!dbResponse)
            {
                response.Error = new ErrorProperties(StatusCodes.Status500InternalServerError, "Error al actualizar la imagen en la base de datos.");
                return response;
            }

            response.Data = true;
            return response;
        }

    }
}
