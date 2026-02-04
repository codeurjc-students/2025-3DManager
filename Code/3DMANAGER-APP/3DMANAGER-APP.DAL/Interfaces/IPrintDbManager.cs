using _3DMANAGER_APP.DAL.Models.File;
using _3DMANAGER_APP.DAL.Models.Print;

namespace _3DMANAGER_APP.DAL.Interfaces
{
    public interface IPrintDbManager
    {
        public List<PrintListResponseDbObject> GetPrintList(int group, int pageNumber, int pageSize, out int totalItems);
        public int PostPrint(PrintRequestDbObject request, out int? error);
        public bool UpdatePrintImageData(int printId, FileResponseDbObject image);
    }
}
