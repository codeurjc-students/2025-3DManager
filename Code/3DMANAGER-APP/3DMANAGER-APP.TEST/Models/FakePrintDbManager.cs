using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.Print;

namespace _3DMANAGER_APP.TEST.Models
{

    /// <summary>
    /// Fake DAL CI: a mock response from BBDD
    /// </summary>
    public class FakePrintDbManager : IPrintDbManager
    {
        public List<PrintListResponseDbObject> GetPrintList(int group)
        {
            return new List<PrintListResponseDbObject>
            {
                new PrintListResponseDbObject {
                    PrintId = 1,
                    PrintName = $"Prueba 01",
                    PrintUserCreator = "user1",
                    PrintDate = new DateTime(2025, 12, 14, 10, 0, 0),
                    PrintTime = 3720,
                    PrintFilamentConsumed = 12.5m
                },
                new PrintListResponseDbObject
                {
                    PrintId = 2,
                    PrintName = $"Prueba 3D 02 - Grupo {group}",
                    PrintUserCreator = "user2",
                    PrintDate = new DateTime(2025, 12, 14, 11, 30, 0),
                    PrintTime = 780,
                    PrintFilamentConsumed = 7.2m
                },
            };
        }

        public bool PostPrint(PrintRequestDbObject request, out int? error)
        {
            error = null;
            return false;
        }
    }
}
