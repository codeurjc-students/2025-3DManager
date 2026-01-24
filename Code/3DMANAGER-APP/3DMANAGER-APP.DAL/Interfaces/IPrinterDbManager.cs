using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Models;
using _3DMANAGER_APP.DAL.Models.File;
using _3DMANAGER_APP.DAL.Models.Printer;

namespace _3DMANAGER_APP.DAL.Interfaces
{
    public interface IPrinterDbManager
    {
        List<PrinterDbObject> GetPrinterList(out ErrorDbObject error);
        public int PostPrinter(PrinterRequestDbObject request, out int? error);
        public List<PrinterListDbObject> GetPrinterDashboardList(int group);
        public bool UpdatePrinterImageData(int printerId, FileResponseDbObject image);
    }
}
