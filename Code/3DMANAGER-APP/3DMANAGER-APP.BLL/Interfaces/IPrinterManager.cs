using _3DMANAGER_APP.BLL.Models;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Printer;
namespace _3DMANAGER_APP.BLL.Interfaces
{
    public interface IPrinterManager
    {
        List<PrinterObject> GetPrinterList(out BaseError error);
        public Task<CommonResponse<int>> PostPrinter(PrinterRequest printer);
        List<PrinterListObject> GetPrinterDashboardList(int groupId, out BaseError error);
        bool UpdatePrinterState(int groupId, int printerId, int stateId);
    }
}
