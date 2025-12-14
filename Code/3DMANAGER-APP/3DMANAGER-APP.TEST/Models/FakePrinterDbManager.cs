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
            return new List<PrinterListDbObject>
            {
                new PrinterListDbObject
                {
                    PrinterId = 1,
                    PrinterName = $"Impresora Dashboard 01 - Grupo {group}",
                    PrinterModel = "Ender 3",
                    PrinterDescription = "Impresora de prueba 3D",
                    PrinterStateId = 1,
                    PrinterStateName = "Disponible"
                },
                new PrinterListDbObject
                {
                    PrinterId = 2,
                    PrinterName = $"Impresora Dashboard 02 - Grupo {group}",
                    PrinterModel = "CR-10",
                    PrinterDescription = "Impresora de prueba 3D",
                    PrinterStateId = 2,
                    PrinterStateName = "En uso"
                }
            };
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
            error = null;
            return true;
        }

    }
}
