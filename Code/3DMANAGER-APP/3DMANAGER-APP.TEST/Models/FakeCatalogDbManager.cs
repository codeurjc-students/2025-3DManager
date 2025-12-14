using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.Filament;

namespace _3DMANAGER_APP.TEST.Models
{

    /// <summary>
    /// Fake DAL CI: a mock response from BBDD
    /// </summary>
    public class FakeCatalogDbManager : ICatalogDbManager
    {
        public List<CatalogResponseDbObject> GetFilamentCatalog(int groupId)
        {
            return new List<CatalogResponseDbObject>
            {
                new CatalogResponseDbObject { Id = 1, Description = "Filamento 1" },
                new CatalogResponseDbObject { Id = 2, Description = "Filamento 2" }
            };
        }

        public List<CatalogResponseDbObject> GetFilamentType()
        {
            return new List<CatalogResponseDbObject>
            {
                new CatalogResponseDbObject { Id = 1, Description = "PLA" },
                new CatalogResponseDbObject { Id = 2, Description = "ABS" },
                new CatalogResponseDbObject { Id = 3, Description = "PETG" }
            };
        }

        public List<CatalogResponseDbObject> GetPrinterCatalog(int groupId)
        {
            return new List<CatalogResponseDbObject>
            {
                new CatalogResponseDbObject { Id = 1, Description = $"Ender 3 " },
                new CatalogResponseDbObject { Id = 2, Description = $"Creality printer" }
            };
        }

        public List<CatalogResponseDbObject> GetPrintState()
        {
            return new List<CatalogResponseDbObject>
            {
                new CatalogResponseDbObject { Id = 1, Description = $"Completo" },
                new CatalogResponseDbObject { Id = 2, Description = $"No Completo" }
            };
        }
    }
}
