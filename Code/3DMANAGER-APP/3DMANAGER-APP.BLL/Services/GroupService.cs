using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Group;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.Group;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Net;

namespace _3DMANAGER_APP.BLL.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupDbManager;
        private readonly IMapper _mapper;
        private readonly ILogger<GroupService> _logger;
        private readonly IAzureBlobStorageService _absService;
        private readonly INotificationService _notificationManager;
        public GroupService(IGroupRepository groupDbManager, IMapper mapper, ILogger<GroupService> logger, IAzureBlobStorageService absService, INotificationService notificationManager)
        {
            _groupDbManager = groupDbManager;
            _mapper = mapper;
            _logger = logger;
            _absService = absService;
            _notificationManager = notificationManager;
        }

        public List<GroupInvitation> GetGroupInvitations(int userId, out bool error)
        {
            error = false;
            var list = _groupDbManager.GetGroupInvitations(userId, out int? errorDb);
            if (errorDb != 0)
            {
                error = true;
            }

            return _mapper.Map<List<GroupInvitation>>(list);
        }

        public bool PostNewGroup(GroupRequest request)
        {
            GroupRequestDbObject groupDbObject = _mapper.Map<GroupRequestDbObject>(request);
            return _groupDbManager.PostNewGroup(groupDbObject);
        }

        public bool PostAcceptInvitation(int groupId, bool isAccepted, int userId, out BaseError? error)
        {
            error = null;
            bool response = _groupDbManager.PostAcceptInvitation(groupId, isAccepted, userId, out int? errorDb);
            if (errorDb != null || errorDb > 0)
            {
                error = new BaseError() { code = (int)HttpStatusCode.InternalServerError, message = $"Error al tratar de aceptar la invitacion del grupo {groupId}" };
                return false;
            }
            return response;
        }

        public GroupBasicDataResponse GetGroupBasicData(int groupId, out BaseError? error)
        {
            error = null;
            GroupBasicDataResponseDbObject response = _groupDbManager.GetGroupBasicData(groupId);
            if (response.GroupId == 0)
            {
                error = new BaseError() { code = (int)HttpStatusCode.InternalServerError, message = $"Error al tratar de recoger la información básica del grupo {groupId}" };
                return new GroupBasicDataResponse();
            }
            return _mapper.Map<GroupBasicDataResponse>(response);
        }

        public bool UpdateGroupData(GroupRequest request, int groupId)
        {
            GroupRequestDbObject groupDbObject = _mapper.Map<GroupRequestDbObject>(request);
            return _groupDbManager.UpdateGroupData(groupDbObject, groupId);

        }

        public bool UpdateLeaveGroup(int userId)
        {
            return _groupDbManager.UpdateLeaveGroup(userId);

        }

        public bool UpdateMembership(int userKickedId, int userId)
        {

            bool response = _groupDbManager.UpdateMembership(userKickedId);
            if (!response)
                return false;

            string msg = $"Has sido expulsado del grupo al que pertenecías";
            bool responseNotification = _notificationManager.CreateNotification(userKickedId, userId, Models.Notifications.NotificationType.GroupExpulsion, msg, out BaseError? error);
            if (error != null)
                return false;
            return responseNotification;
        }

        public async Task<bool> DeleteGroup(int userId, int groupId)
        {
            var dbResponse = _groupDbManager.DeleteGroup(userId, groupId);

            if (!dbResponse)
            {
                string msg = $"No se pudo borrar el grupo {groupId} en la base de datos.";
                _logger.LogError(msg);
                return false;
            }

            try
            {
                await _absService.DeleteGroupAsync(groupId);
            }
            catch (Exception ex)
            {
                string msg = $"Error al borrar las imágenes del grupo {groupId} en Azure Blob Storage.";
                _logger.LogError(ex, msg);
            }

            return true;
        }

        public bool TrasnferOwnership(int userId, int groupId, int newOwnerUserId)
        {
            return _groupDbManager.TrasnferOwnership(userId, groupId, newOwnerUserId);
        }

        public GroupDashboardData GetGroupDashboardData(int groupId, out BaseError? error)
        {
            error = null;
            GroupDashboardDataDbObject response = _groupDbManager.GetGroupDashboardData(groupId);
            if (response.GroupId == 0)
            {
                error = new BaseError() { code = (int)HttpStatusCode.InternalServerError, message = "Error al tratar de recoger la información del dashboard del grupo" };
                return new GroupDashboardData();
            }
            return _mapper.Map<GroupDashboardData>(response);
        }
    }
}
