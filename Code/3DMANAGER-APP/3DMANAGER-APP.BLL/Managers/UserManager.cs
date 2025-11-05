using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.User;
using _3DMANAGER_APP.DAL.Interfaces;
using _3DMANAGER_APP.DAL.Models.User;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace _3DMANAGER_APP.BLL.Managers
{
    public class UserManager : IUserManager
    {
        private readonly IUserDbManager _userDbManager;
        private readonly IMapper _mapper;
        private readonly ILogger<UserManager> _logger;
        public UserManager(IUserDbManager userDbManager, IMapper mapper, ILogger<UserManager> logger)
        {
            _userDbManager = userDbManager;
            _mapper = mapper;
            _logger = logger;
        }

        public bool PostNewUser(UserCreateRequest user, out BaseError? error)
        {
            error = null;

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
                        error = new BaseError() { code = StatusCodes.Status409Conflict, message = msg };
                        break;
                    case 4092:
                        msg = $"Error al crear usuario con email {user.UserEmail}. Ya existe una cuenta con ese correo";
                        _logger.LogError(msg);
                        error = new BaseError() { code = StatusCodes.Status409Conflict, message = msg };
                        break;
                    case 500:
                        msg = $"Error al crear usuario en el servidor.";
                        _logger.LogError(msg);
                        error = new BaseError() { code = StatusCodes.Status500InternalServerError, message = msg };
                        break;
                    default:
                        break;
                }

            }
            return responseDb;
        }

        public UserObject? Login(string userName, string userPassword, out BaseError? error)
        {
            error = null;

            // Paso 1: buscar usuario
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
    }
}
