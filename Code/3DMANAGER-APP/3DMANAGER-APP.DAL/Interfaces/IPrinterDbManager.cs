using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Models;
using _3DMANAGER_APP.DAL.Models.Printer;

namespace _3DMANAGER_APP.DAL.Interfaces
{
    public interface IPrinterDbManager
    {
        List<PrinterDbObject> GetPrinterList(out ErrorDbObject error);
        public bool PostPrinter(PrinterRequestDbObject request, out int? error);
    }
}
