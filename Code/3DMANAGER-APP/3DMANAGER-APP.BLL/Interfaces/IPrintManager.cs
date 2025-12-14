using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Print;

namespace _3DMANAGER_APP.BLL.Interfaces
{
    public interface IPrintManager
    {
        public List<PrintListResponse> GetPrintList(int group, out BaseError? error);
        public bool PostPrint(PrintRequest print, out BaseError? error);
    }
}
