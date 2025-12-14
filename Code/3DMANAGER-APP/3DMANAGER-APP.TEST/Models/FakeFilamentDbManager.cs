using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.Filament;

namespace _3DMANAGER_APP.TEST.Models
{

    /// <summary>
    /// Fake DAL CI: a mock response from BBDD
    /// </summary>
    public class FakeFilamentDbManager : IFilamentDbManager
    {
        public List<FilamentListResponseDbObject> GetFilamentList(int group)
        {
            return new List<FilamentListResponseDbObject>
            {
                new FilamentListResponseDbObject
                {
                    FilamentId = 1,
                    FilamentName = $"Filamento 1",
                    FilamentState = "Disponible",
                    FilamentLength = 1200.5m,
                    FilamentCost = 25.5m
                },
                new FilamentListResponseDbObject
                {
                    FilamentId = 2,
                    FilamentName = $"Filamento 2",
                    FilamentState = "En uso",
                    FilamentLength = 800.0m,
                    FilamentCost = 30.0m
                },
            };
        }

        public bool PostFilament(FilamentRequestDbObject request, out int? error)
        {
            error = null;
            return true;
        }
    }
}
