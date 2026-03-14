using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.File;
using _3DMANAGER_APP.BLL.Models.User;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.File;
using _3DMANAGER_APP.DAL.Models.User;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Net;

namespace _3DMANAGER_APP.BLL.Managers
{
    public class UserManager : IUserManager
    {
        private readonly IUserDbManager _userDbManager;
        private readonly IMapper _mapper;
        private readonly ILogger<UserManager> _logger;
        private readonly IAwsS3Service _awsS3Service;
        public UserManager(IUserDbManager userDbManager, IMapper mapper, ILogger<UserManager> logger, IAwsS3Service awsS3Service)
        {
            _userDbManager = userDbManager;
            _mapper = mapper;
            _logger = logger;
            _awsS3Service = awsS3Service;
        }

        public async Task<CommonResponse<int>> PostNewUser(UserCreateRequest user)
        {

            CommonResponse<int> response = new CommonResponse<int>();
            var passwordHasher = new PasswordHasher<UserCreateRequest>();
            string hashedPassword = passwordHasher.HashPassword(user, user.UserPassword);

            user.UserPassword = hashedPassword;
            UserCreateRequestDbObject userDbObject = _mapper.Map<UserCreateRequestDbObject>(user);
            var responseDb = _userDbManager.PostNewUser(userDbObject, out int? errorDb);

            if (errorDb != null)
            {
                string msg = "";
                switch (errorDb)
                {
                    case 4091:
                        msg = $"Error al crear usuario con nombre {user.UserName}. Ya existe una cuenta con ese nombre";
                        _logger.LogError(msg);
                        response.Error = new Response.ErrorProperties() { Code = StatusCodes.Status409Conflict, Message = msg };
                        break;
                    case 4092:
                        msg = $"Error al crear usuario con email {user.UserEmail}. Ya existe una cuenta con ese correo";
                        _logger.LogError(msg);
                        response.Error = new Response.ErrorProperties() { Code = StatusCodes.Status409Conflict, Message = msg };
                        break;
                    case 500:
                        msg = $"Error al crear usuario en el servidor.";
                        _logger.LogError(msg);
                        response.Error = new Response.ErrorProperties() { Code = StatusCodes.Status500InternalServerError, Message = msg };
                        break;
                    default:
                        break;
                }
                return response;
            }
            response.Data = responseDb;
            if (user.ImageFile != null)
            {
                bool responseImage = await UpdateS3UserImage(responseDb, user.ImageFile);
                if (!responseImage)
                {
                    string msg = "El usuario se ha creado correctamente, pero la imagen ha fallado al ser guardada.";
                    _logger.LogError(msg);
                    response.Error = new Response.ErrorProperties() { Code = StatusCodes.Status409Conflict, Message = msg };
                }
            }
            return response;
        }

        public async Task<bool> UpdateS3UserImage(int userId, IFormFile imageFile)
        {
            FileResponse? image = null;

            if (imageFile != null)
            {
                image = await _awsS3Service.UploadImageAsync(imageFile.OpenReadStream(), imageFile.FileName,
                    imageFile.ContentType, "users", null);
                if (image != null)
                    return _userDbManager.UpdateUserImageData(userId, _mapper.Map<FileResponseDbObject>(image));
                else return false;
            }
            else
            {
                return false;
            }

        }
        public UserObject? Login(string userName, string userPassword, out BaseError? error)
        {
            error = null;

            var userDb = _userDbManager.Login(userName);
            if (userDb == null)
            {
                string msg = $"El usuario {userName} no existe";
                _logger.LogError(msg);
                error = new BaseError
                {
                    code = StatusCodes.Status404NotFound,
                    message = msg
                };
                return null;
            }

            var passwordHasher = new PasswordHasher<UserDbObject>();
            var result = passwordHasher.VerifyHashedPassword(userDb, userDb.UserPassword, userPassword);

            if (result != PasswordVerificationResult.Success)
            {
                string msg = $"Contraseña incorrecta para el usuario {userName}";
                _logger.LogError(msg);
                error = new BaseError
                {
                    code = StatusCodes.Status401Unauthorized,
                    message = msg
                };
                return null;
            }
            UserObject userResponse = _mapper.Map<UserObject>(userDb);
            userResponse.UserPassword = string.Empty;
            return userResponse;
        }

        public List<UserListResponse> GetUserList(int group, out BaseError? error)
        {
            error = null;
            List<UserListResponseDbObject> list = _userDbManager.GetUserList(group);
            if (list == null)
            {
                string msg = $"Error al obtener listado de usuarios para el grupo {group}";
                _logger.LogError(msg);
                error = new BaseError() { code = (int)HttpStatusCode.InternalServerError, message = msg };

            }

            return _mapper.Map<List<UserListResponse>>(list);
        }
        public List<UserListResponse> GetUserInvitationList(string? filter, out BaseError? error)
        {
            error = null;
            List<UserListResponseDbObject> list = _userDbManager.GetUserInvitationList(filter);
            if (list == null)
            {
                _logger.LogError("Error al obtener listado de usuarios para invitar");
                error = new BaseError() { code = (int)HttpStatusCode.InternalServerError, message = "Error al obtener listado de usuarios para invitar" };
            }
            return _mapper.Map<List<UserListResponse>>(list);
        }

        public bool PostUserInvitation(int groupId, int userId, out BaseError? error)
        {
            bool responseDb = _userDbManager.PostUserInvitation(groupId, userId, out int? errorDb);
            error = null;
            if (errorDb != null || !responseDb)
            {
                string msg = "";
                switch (errorDb)
                {
                    case 409:
                        msg = $"Ya existe una invitacion del grupo {groupId} al usuario {userId}.";
                        _logger.LogInformation(msg);
                        error = new BaseError { code = StatusCodes.Status409Conflict, message = msg };
                        break;
                    case 500:
                        msg = $"Error al invitar al usuario {userId} al grupo {groupId} en el servidor.";
                        _logger.LogError(msg);
                        error = new BaseError { code = StatusCodes.Status500InternalServerError, message = msg };
                        break;
                    default:
                        break;
                }
                return false;
            }
            return true;
        }

        public UserObject GetUserById(int userId)
        {
            return _mapper.Map<UserObject>(_userDbManager.GetUserById(userId));
        }
        public int GetGroupIdByUserId(int userId)
        {
            return _userDbManager.GetGroupIdByUserId(userId);
        }

        public bool UpdateUser(UserUpdateRequest request)
        {
            UserUpdateRequestDbObject requestDb = _mapper.Map<UserUpdateRequestDbObject>(request);
            return _userDbManager.UpdateUser(requestDb);
        }

        public UserDetailObject GetUserDetail(int groupId, int userId, out BaseError? error)
        {
            error = null;
            UserDetailObject response;
            var responseDb = _userDbManager.GetUserDetail(groupId, userId);
            if (responseDb == null)
            {
                string msg = $"Error al obtener el detalle de usuario {userId}";
                _logger.LogError(msg);
                error = new BaseError()
                {
                    code = StatusCodes.Status500InternalServerError,
                    message = msg
                };
            }
            response = _mapper.Map<UserDetailObject>(responseDb);

            if (response != null)
            {
                if (response.UserImageData != null && response.UserImageData.FileUrl != null && response.UserImageData.FileKey != null)
                    response.UserImageData.FileUrl = _awsS3Service.GetPresignedUrl(response.UserImageData.FileKey, 1);
                else
                    response.UserImageData.FileUrl = _awsS3Service.GetPresignedUrl("default/3dmanager-default-user.png", 1);

            }
            return response;
        }
    }
}
