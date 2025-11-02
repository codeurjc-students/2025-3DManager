using _3DMANAGER_APP.BLL.Interfaces;
using _3DMANAGER_APP.BLL.Models;
using _3DMANAGER_APP.BLL.Models.Base;
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

        public bool PostNewUser(UserObject user, out BaseError? error)
        {
            error = null;

            var passwordHasher = new PasswordHasher<UserObject>();
            string hashedPassword = passwordHasher.HashPassword(user, user.UserPassword);

            user.UserPassword = hashedPassword;
            UserDbObject userDbObject = _mapper.Map<UserDbObject>(user);
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
    }
}
