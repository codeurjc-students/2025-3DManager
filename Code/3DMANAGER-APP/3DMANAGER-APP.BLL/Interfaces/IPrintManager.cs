using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Print;
using Microsoft.AspNetCore.Http;

namespace _3DMANAGER_APP.BLL.Interfaces
{
    public interface IPrintManager
    {
        public PrintListResponse GetPrintList(int group, PagedRequest pagination, out BaseError? error);
        public Task<CommonResponse<int>> PostPrint(PrintRequest print);
        public PrintListResponse GetPrintListByType(int group, PagedRequest pagination, int type, int id, out BaseError? error);
        bool UpdatePrint(PrintDetailRequest request);
        PrintDetailObject GetPrintDetail(int groupId, int printId, out BaseError? error);
        List<PrintCommentObject> GetPrintComments(int groupId, int printId, out BaseError? error);
        int PostPrintComment(PrintCommentRequest request, int groupId);
        public Task<CommonResponse<bool>> DeletePrint(int printId, int groupId);
        public Task<CommonResponse<bool>> DeletePrintImage(int printId, int groupId);
        public Task<CommonResponse<bool>> UpdatePrintImage(int printId, int groupId, IFormFile imageFile);
        public bool DeletePrintComment(int commentId);
    }
}
