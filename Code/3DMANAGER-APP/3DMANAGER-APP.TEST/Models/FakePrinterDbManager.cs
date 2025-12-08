using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.Printer;

namespace _3DMANAGER_APP.TEST.Models
{

    /// <summary>
    /// Fake del acceso a datos para CI: simula la respuesta de la BBDD
    /// </summary>
    public class FakePrinterDbManager : IPrinterDbManager
    {
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
    }
}
