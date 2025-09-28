using _3DMANAGER.BLL.Interfaces;
using _3DMANAGER.BLL.Models;
using _3DMANAGER.DAL.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DMANAGER.BLL.Managers
{
    public class PrinterManager : IPrinterManager
    {
        private IPrinterDbManager _printerDbManager;
        private IMapper _mapper;
        private ILogger<PrinterManager> _logger;
        public PrinterManager(IPrinterDbManager printerDbManager,IMapper mapper,ILogger<PrinterManager> logger)
        {
            _printerDbManager = printerDbManager;
            _mapper = mapper;
            _logger = logger;
        }

        public List<PrinterObject> GetPrinterList()
        {
            List<PrinterObject> response = null;
            var responseDb = _printerDbManager.GetPrinterList(out int error);
            response = _mapper.Map<List<PrinterObject>>(responseDb);
            return response;
        }
    }
}
