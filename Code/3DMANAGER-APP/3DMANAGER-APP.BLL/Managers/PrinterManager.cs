using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.File;
using _3DMANAGER_APP.BLL.Models.Printer;
using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.File;
using _3DMANAGER_APP.DAL.Models.Printer;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

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

        public List<PrinterObject> GetPrinterList(out BaseError error)
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
            List<PrinterListDbObject> list = _printerDbManager.GetPrinterDashboardList(groupId);
            if (list == null)
                error = new BaseError() { code = (int)HttpStatusCode.InternalServerError, message = "Error al obtener listado de impresoras" };

            List<PrinterListObject> response = _mapper.Map<List<PrinterListObject>>(list);
            if (response != null)
            {
                foreach (PrinterListObject p in response)
                {
                    if (p.PrinterImageData != null && p.PrinterImageData.FileUrl != null && p.PrinterImageData.FileKey != null)
                        p.PrinterImageData.FileUrl = _awsS3Service.GetPresignedUrl(p.PrinterImageData.FileKey, 1);
                    else
                        p.PrinterImageData.FileUrl = _awsS3Service.GetPresignedUrl("default/3dmanager-default-printer.jpg", 1);
                }
            }

            return response;
        }
    }
}
