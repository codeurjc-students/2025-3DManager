using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Print;

namespace _3DMANAGER_APP.BLL.Interfaces
{
    public interface IPrintManager
    {
        public PrintListResponse GetPrintList(int group, PagedRequest pagination, out BaseError? error);
        public bool PostPrint(PrintRequest print, out BaseError? error);
    }
}
