using _3DMANAGER.API.Models;
using _3DMANAGER.BLL.Models;

namespace _3DMANAGER.BLL.Interfaces
{
    public interface IPrinterManager
    {
        List<PrinterObject> GetPrinterList(out BaseError error);
    }
}
