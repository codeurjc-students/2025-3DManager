using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Models;
using _3DMANAGER_APP.DAL.Models.File;
using _3DMANAGER_APP.DAL.Models.Print;
using _3DMANAGER_APP.DAL.Models.Printer;

namespace _3DMANAGER_APP.DAL.Interfaces
{
    public interface IPrinterDbManager
    {
        List<PrinterDbObject> GetPrinterList(out ErrorDbObject error);
        public int PostPrinter(PrinterRequestDbObject request, out int? error);
        public List<PrinterListDbObject> GetPrinterDashboardList(int group, out bool error);
        public bool UpdatePrinterImageData(int printerId, FileResponseDbObject image);
        public bool UpdatePrinter(PrinterDetailRequestDbObject requestDb);
        PrinterDetailDbObject GetPrinterDetail(int groupId, int printerId);
        List<PrinterTimesValuesDbObject> GetTimeVariation(int groupId, int printerId, out bool error);
        DeletedDbObject DeletePrinter(int printerId, int groupId, out int? error);
        public FileResponseDbObject GetPrinterImageData(int printerId, int groupId, out bool error);
        public bool DeletePrinterImageData(int printerId, int groupId);

    }
}
