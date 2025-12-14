using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models;
using _3DMANAGER_APP.DAL.Models.Printer;

namespace _3DMANAGER_APP.TEST.Models
{

    /// <summary>
    /// Fake DAL CI: a mock response from BBDD
    /// </summary>
    public class FakePrinterDbManager : IPrinterDbManager
    {
        public List<PrinterListDbObject> GetPrinterDashboardList(int group)
        {
            throw new NotImplementedException();
        }

        public List<PrinterDbObject> GetPrinterList(out ErrorDbObject error)
        {
            error = null;
            return new List<PrinterDbObject>
            {
                new PrinterDbObject { PrinterName = "Impresora test mock 01" },
                new PrinterDbObject { PrinterName = "Impresora test mock 02" },
                new PrinterDbObject { PrinterName = "Impresora test mock 03" }
            };
        }

        public bool PostPrinter(PrinterRequestDbObject request, out int? error)
        {
            throw new NotImplementedException();
        }

        List<PrinterDbObject> IPrinterDbManager.GetPrinterList(out ErrorDbObject error)
        {
            throw new NotImplementedException();
        }
    }
}
