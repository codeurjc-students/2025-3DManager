using _3DMANAGER_APP.DAL.Models.File;
using _3DMANAGER_APP.DAL.Models.Print;

namespace _3DMANAGER_APP.DAL.Interfaces
{
    public interface IPrintDbManager
    {
        public List<PrintListResponseDbObject> GetPrintList(int group, int pageNumber, int pageSize, out int totalItems, out bool error);
        public int PostPrint(PrintRequestDbObject request, out int? error);
        public bool UpdatePrintImageData(int printId, FileResponseDbObject image);
        public List<PrintListResponseDbObject> GetPrintListByType(int group, int pageNumber, int pageSize, int type, int id, out int totalItems, out bool error);
        bool UpdatePrint(PrintDetailRequestDbObject request);
        PrintDetailDbObject GetPrintDetail(int groupId, int printId);
        List<PrintCommentDbObject> GetPrintComments(int groupId, int printId, out bool error);
        int PostPrintComment(PrintCommentRequestDbObject request);
        DeletedDbObject DeletePrint(int printId, int groupId, out int? error);
        public FileResponseDbObject GetPrintImageData(int printId, int groupId, out bool error);
        public bool DeletePrintImageData(int printId, int groupId);
    }
}
