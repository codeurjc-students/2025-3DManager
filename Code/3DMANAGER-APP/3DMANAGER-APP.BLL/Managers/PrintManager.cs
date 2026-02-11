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

namespace _3DMANAGER_APP.BLL.Managers
{
    public class PrintManager : IPrintManager
    {
        private readonly IPrintDbManager _printDbManager;
        private readonly IMapper _mapper;
        private readonly ILogger<PrintManager> _logger;
        private IAwsS3Service _awsS3Service;
        public PrintManager(IPrintDbManager printDbManager, IMapper mapper, ILogger<PrintManager> logger, IAwsS3Service awsS3Service)
        {
            _printDbManager = printDbManager;
            _mapper = mapper;
            _logger = logger;
            _awsS3Service = awsS3Service;
        }

        public PrintListResponse GetPrintList(int group, PagedRequest pagination, out BaseError? error)
        {
            error = null;
            var result = _printDbManager.GetPrintList(group, pagination.PageNumber, pagination.PageSize, out int totalItems);
            if (result == null)
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
            int responseDb = _printDbManager.PostPrint(printDbObject, out int? errorDb);

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
                bool responseImage = await UpdateS3PrintImage(responseDb, print.ImageFile, print.GroupId);
                if (!responseImage)
                {
                    string msg = "La impresion 3d se ha creado correctamente, pero la imagen ha fallado al ser guardada.";
                    _logger.LogError(msg);
                    response.Error = new Response.ErrorProperties() { Code = StatusCodes.Status409Conflict, Message = msg };
                }
            }
            return response;
        }

        public async Task<bool> UpdateS3PrintImage(int printerId, IFormFile imageFile, int groupId)
        {
            FileResponse? image = null;

            if (imageFile != null)
            {
                image = await _awsS3Service.UploadImageAsync(imageFile.OpenReadStream(), imageFile.FileName,
                    imageFile.ContentType, "prints", groupId);
                if (image != null)
                    return _printDbManager.UpdatePrintImageData(printerId, _mapper.Map<FileResponseDbObject>(image));
                else return false;
            }
            else
            {
                return false;
            }

        }
    }
}
