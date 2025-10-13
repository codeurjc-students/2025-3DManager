using _3DMANAGER_APP.BLL.Models;
using _3DMANAGER_APP.BLL.Models.Base;

namespace _3DMANAGER_APP.BLL.Interfaces
{
    public interface IPrinterManager
    {
        List<PrinterObject> GetPrinterList(out BaseError error);
    }
}
