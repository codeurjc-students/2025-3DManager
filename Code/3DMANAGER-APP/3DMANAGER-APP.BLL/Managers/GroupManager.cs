using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Group;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.Group;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Net;

namespace _3DMANAGER_APP.BLL.Managers
{
    public class GroupManager : IGroupManager
    {
        private readonly IGroupDbManager _groupDbManager;
        private readonly IMapper _mapper;
        private readonly ILogger<GroupManager> _logger;
        private readonly IAwsS3Service _awsS3Service;
        public GroupManager(IGroupDbManager groupDbManager, IMapper mapper, ILogger<GroupManager> logger, IAwsS3Service awsS3Service)
        {
            _groupDbManager = groupDbManager;
            _mapper = mapper;
            _logger = logger;
            _awsS3Service = awsS3Service;
        }

        public List<GroupInvitation> GetGroupInvitations(int userId)
        {
            return _mapper.Map<List<GroupInvitation>>(_groupDbManager.GetGroupInvitations(userId));
        }

        public bool PostNewGroup(GroupRequest request, out BaseError? error)
        {
            error = null;

            GroupRequestDbObject groupDbObject = _mapper.Map<GroupRequestDbObject>(request);
            return _groupDbManager.PostNewGroup(groupDbObject);
        }

        public bool PostAcceptInvitation(int groupId, bool isAccepted, int userId, out BaseError? error)
        {
            error = null;
            bool response = _groupDbManager.PostAcceptInvitation(groupId, isAccepted, userId, out int? errorDb);
            if (errorDb != null || errorDb > 0)
            {
                error = new BaseError() { code = (int)HttpStatusCode.InternalServerError, message = "Error al tratar de aceptar la invitacion del grupo" };
                return false;
            }
            return response;
        }

        public GroupBasicDataResponse GetGroupBasicData(int groupId, out BaseError? error)
        {
            error = null;
            GroupBasicDataResponseDbObject response = _groupDbManager.GetGroupBasicData(groupId);
            if (response == null)
            {
                error = new BaseError() { code = (int)HttpStatusCode.InternalServerError, message = "Error al tratar de recoger la información básica del grupo" };
                return null;
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

        public bool UpdateMembership(int userKickedId)
        {
            return _groupDbManager.UpdateMembership(userKickedId);

        }

        public async Task<bool> DeleteGroup(int userId, int groupId)
        {
            var dbResponse = _groupDbManager.DeleteGroup(userId, groupId);

            if (!dbResponse)
            {
                _logger.LogWarning($"No se pudo borrar el grupo {groupId} en la base de datos.");
                return false;
            }

            try
            {
                await _awsS3Service.DeleteGroupAsync(groupId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al borrar las imágenes del grupo {groupId} en S3.");
            }

            return true;
        }

        public bool TrasnferOwnership(int userId, int groupId, int newOwnerUserId)
        {
            return _groupDbManager.TrasnferOwnership(userId, groupId, newOwnerUserId);
        }
    }
}
