using _3DMANAGER_APP.BLL.Models;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Printer;

namespace _3DMANAGER_APP.BLL.Interfaces
{
    public interface IPrinterManager
    {
        List<PrinterObject> GetPrinterList(out BaseError error);
        public bool PostPrinter(PrinterRequest printer, out BaseError? error);
    }
}
