using _3DMANAGER.DAL.Base;
using _3DMANAGER.DAL.Models;

namespace _3DMANAGER.DAL.Interfaces
{
    public interface IPrinterDbManager
    {
        List<PrinterDbObject> GetPrinterList(out ErrorDbObject error);
    }
}
