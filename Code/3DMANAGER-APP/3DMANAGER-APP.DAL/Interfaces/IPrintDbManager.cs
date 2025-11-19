using _3DMANAGER_APP.DAL.Models.Print;

namespace _3DMANAGER_APP.DAL.Interfaces
{
    public interface IPrintDbManager
    {
        public List<PrintListResponseDbObject> GetPrintList(int group);
    }
}
