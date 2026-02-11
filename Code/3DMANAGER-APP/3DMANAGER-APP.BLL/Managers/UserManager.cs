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
        private IAwsS3Service _awsS3Service;
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
                error = new BaseError
                {
                    code = StatusCodes.Status404NotFound,
                    message = "El usuario no existe"
                };
                return null;
            }


            var passwordHasher = new PasswordHasher<UserDbObject>();
            var result = passwordHasher.VerifyHashedPassword(userDb, userDb.UserPassword, userPassword);

            if (result != PasswordVerificationResult.Success)
            {
                error = new BaseError
                {
                    code = StatusCodes.Status401Unauthorized,
                    message = "Contraseña incorrecta"
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
                error = new BaseError() { code = (int)HttpStatusCode.InternalServerError, message = "Error al obtener listado de usuarios" };

            return _mapper.Map<List<UserListResponse>>(list);
        }
        public List<UserListResponse> GetUserInvitationList(string? filter, out BaseError? error)
        {
            error = null;
            List<UserListResponseDbObject> list = _userDbManager.GetUserInvitationList(filter);
            if (list == null)
                error = new BaseError() { code = (int)HttpStatusCode.InternalServerError, message = "Error al obtener listado de usuarios para invitar" };

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
                        msg = $"Error al invitar al  usuario . Ya existe una invitacion del grupo a ese usuario.";
                        _logger.LogError(msg);
                        error = new BaseError { code = StatusCodes.Status409Conflict, message = msg };
                        break;
                    case 500:
                        msg = $"Error al invitar al usuario en el servidor.";
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
    }
}
