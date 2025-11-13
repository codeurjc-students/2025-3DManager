using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Group;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.User;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace _3DMANAGER_APP.BLL.Managers
{
    public class GroupManager : IGroupManager
    {
        private readonly IGroupDbManager _groupDbManager;
        private readonly IMapper _mapper;
        private readonly ILogger<GroupManager> _logger;
        public GroupManager(IGroupDbManager groupDbManager, IMapper mapper, ILogger<GroupManager> logger)
        {
            _groupDbManager = groupDbManager;
            _mapper = mapper;
            _logger = logger;
        }

        public bool PostNewGroup(GroupRequest request, out BaseError? error)
        {
            error = null;

            GroupRequestDbObject groupDbObject = _mapper.Map<GroupRequestDbObject>(request);
            return _groupDbManager.PostNewGroup(groupDbObject);
        }
    }
}
