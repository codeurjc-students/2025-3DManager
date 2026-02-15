using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Print;

namespace _3DMANAGER_APP.BLL.Interfaces
{
    public interface IPrintManager
    {
        public PrintListResponse GetPrintList(int group, PagedRequest pagination, out BaseError? error);
        public Task<CommonResponse<int>> PostPrint(PrintRequest print);
        public PrintListResponse GetPrintListByType(int group, PagedRequest pagination, int type, int id, out BaseError? error);
    }
}
