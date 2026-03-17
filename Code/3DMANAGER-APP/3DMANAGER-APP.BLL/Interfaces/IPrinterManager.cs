using _3DMANAGER_APP.BLL.Models;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Printer;
using Microsoft.AspNetCore.Http;
namespace _3DMANAGER_APP.BLL.Interfaces
{
    public interface IPrinterManager
    {
        List<PrinterObject> GetPrinterList(out BaseError? error);
        public Task<CommonResponse<int>> PostPrinter(PrinterRequest printer);
        List<PrinterListObject> GetPrinterDashboardList(int groupId, out BaseError? error);
        bool UpdatePrinter(PrinterDetailRequest request);
        PrinterDetailObject GetPrinterDetail(int groupId, int printerId, out BaseError? error);
        public Task<CommonResponse<bool>> DeletePrinter(int printerId, int groupId);
        public Task<CommonResponse<bool>> DeletePrinterImage(int printerId, int groupId);
        public Task<CommonResponse<bool>> UpdatePrinterImage(int printerId, int groupId, IFormFile imageFile);
    }
}
