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
using static _3DMANAGER_APP.BLL.Models.Base.Response;

namespace _3DMANAGER_APP.BLL.Managers
{
    public class UserManager : IUserManager
    {
        private readonly IUserDbManager _userDbManager;
        private readonly IMapper _mapper;
        private readonly ILogger<UserManager> _logger;
        private readonly IAzureBlobStorageService _absService;
        public UserManager(IUserDbManager userDbManager, IMapper mapper, ILogger<UserManager> logger, IAzureBlobStorageService absService)
        {
            _userDbManager = userDbManager;
            _mapper = mapper;
            _logger = logger;
            _absService = absService;
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
                bool responseImage = await UpdateABSUserImage(responseDb, user.ImageFile);
                if (!responseImage)
                {
                    string msg = "El usuario se ha creado correctamente, pero la imagen ha fallado al ser guardada.";
                    _logger.LogError(msg);
                    response.Error = new Response.ErrorProperties() { Code = StatusCodes.Status409Conflict, Message = msg };
                }
            }
            return response;
        }

        public async Task<bool> UpdateABSUserImage(int userId, IFormFile imageFile)
        {
            FileResponse? image = null;

            if (imageFile != null)
            {
                image = await _absService.UploadImageAsync(imageFile.OpenReadStream(), imageFile.FileName,
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
        public UserObject Login(string userName, string userPassword, out BaseError? error)
        {
            error = null;

            var userDb = _userDbManager.Login(userName);
            if (userDb.UserId == 0)
            {
                string msg = $"El usuario {userName} no existe";
                _logger.LogError(msg);
                error = new BaseError
                {
                    code = StatusCodes.Status404NotFound,
                    message = msg
                };
                return new UserObject();
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
                return new UserObject();
            }
            UserObject userResponse = _mapper.Map<UserObject>(userDb);
            userResponse.UserPassword = string.Empty;
            return userResponse;
        }

        public List<UserListResponse> GetUserList(int group, out BaseError? error)
        {
            error = null;
            List<UserListResponseDbObject> list = _userDbManager.GetUserList(group, out bool errorDb);
            if (errorDb)
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
            List<UserListResponseDbObject> list = _userDbManager.GetUserInvitationList(filter, out bool errorDb);
            if (errorDb)
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
            if (responseDb.UserId == 0)
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
                    response.UserImageData.FileUrl = _absService.GetPresignedUrl(response.UserImageData.FileKey, 1);
                else
                    response.UserImageData!.FileUrl = _absService.GetPresignedUrl("default/3dmanager-default-user.png", 1);

            }
            return response!;
        }

        public async Task<CommonResponse<bool>> DeleteUserImage(int userId, int groupId)
        {
            CommonResponse<bool> response = new CommonResponse<bool>();

            FileResponseDbObject imageData = _userDbManager.GetUserImageData(userId, groupId, out bool error);

            if (error)
            {
                response.Error = new ErrorProperties(StatusCodes.Status404NotFound, "Ha ocurrido un error al buscar en el usuario la imagen asociada.");
                return response;
            }
            else if (imageData.FileKey == null && !error)
            {
                response.Data = true;
                return response;
            }

            await _absService.DeleteImageAsync(imageData!.FileKey!);
            bool dbResponse = _userDbManager.DeleteUserImageData(userId, groupId);

            if (!dbResponse)
            {
                response.Error = new ErrorProperties(StatusCodes.Status500InternalServerError, "Error al eliminar la imagen en la base de datos.");
                return response;
            }

            response.Data = true;
            return response;
        }

        public async Task<CommonResponse<bool>> UpdateUserImage(int userId, int groupId, IFormFile imageFile)
        {
            CommonResponse<bool> response = new CommonResponse<bool>();
            response.Data = false;
            if (imageFile == null)
            {
                response.Error = new ErrorProperties(StatusCodes.Status400BadRequest, "Error, no se ha recibido una imagen para actualizar");
                return response;
            }

            var aBSResponse = await _absService.UploadImageAsync(imageFile.OpenReadStream(), imageFile.FileName, imageFile.ContentType, "users", null);
            if (aBSResponse == null)
            {
                response.Error = new ErrorProperties(StatusCodes.Status409Conflict, "Error al subir la imagen a Azure Blob Storage.");
                return response;
            }
            var deletedImage = await DeleteUserImage(userId, groupId);
            if (!deletedImage.Data)
            {
                var fileData = _userDbManager.GetUserImageData(userId, groupId, out bool errorDbImage);
                string? keyValue = errorDbImage ? "FileKey Desconocido" : fileData.FileKey;
                string msg = $"Se ha intentado eliminar una foto del usuario {userId} del grupo {groupId} con el fileKey {keyValue}";
                _logger.LogError(msg);
            }
            bool dbResponse = _userDbManager.UpdateUserImageData(userId, _mapper.Map<FileResponseDbObject>(aBSResponse));
            if (!dbResponse)
            {
                response.Error = new ErrorProperties(StatusCodes.Status500InternalServerError, "Error al actualizar la imagen en la base de datos.");
                return response;
            }

            response.Data = true;
            return response;
        }

    }
}
