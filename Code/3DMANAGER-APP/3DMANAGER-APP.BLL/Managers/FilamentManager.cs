using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Filament;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.Filament;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Net;

namespace _3DMANAGER_APP.BLL.Managers
{
    public class FilamentManager : IFilamentManager
    {
        private readonly IFilamentDbManager _filamentDbManager;
        private readonly IMapper _mapper;
        private readonly ILogger<FilamentManager> _logger;
        public FilamentManager(IFilamentDbManager filamentDbManager, IMapper mapper, ILogger<FilamentManager> logger)
        {
            _filamentDbManager = filamentDbManager;
            _mapper = mapper;
            _logger = logger;
        }

        public List<FilamentListResponse> GetFilamentList(int group, out BaseError? error)
        {
            error = null;
            List<FilamentListResponseDbObject> list = _filamentDbManager.GetFilamentList(group);
            if (list == null)
                error = new BaseError() { code = (int)HttpStatusCode.InternalServerError, message = "Error al obtener listado de filamentos" };

            return _mapper.Map<List<FilamentListResponse>>(list);
        }
    }
}
