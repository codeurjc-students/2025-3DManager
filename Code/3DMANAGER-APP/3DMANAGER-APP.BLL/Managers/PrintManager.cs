using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Print;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.Print;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Net;

namespace _3DMANAGER_APP.BLL.Managers
{
    public class PrintManager : IPrintManager
    {
        private readonly IPrintDbManager _printDbManager;
        private readonly IMapper _mapper;
        private readonly ILogger<PrintManager> _logger;
        public PrintManager(IPrintDbManager printDbManager, IMapper mapper, ILogger<PrintManager> logger)
        {
            _printDbManager = printDbManager;
            _mapper = mapper;
            _logger = logger;
        }

        public List<PrintListResponse> GetPrintList(int group, out BaseError? error)
        {
            error = null;
            List<PrintListResponseDbObject> list = _printDbManager.GetPrintList(group);
            if (list == null)
                error = new BaseError() { code = (int)HttpStatusCode.InternalServerError, message = "Error al obtener listado de impresiones" };

            return _mapper.Map<List<PrintListResponse>>(list);
        }
    }
}
