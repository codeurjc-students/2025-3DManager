using _3DMANAGER_APP.BLL.Models;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Models;
using _3DMANAGER_APP.DAL.Models.User;
using AutoMapper;

namespace _3DMANAGER_APP.BLL.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Base
            CreateMap<PrinterObject, PrinterDbObject>().ReverseMap();
            CreateMap<ErrorDbObject, BaseError>().ReverseMap();
            #endregion

            #region User
            CreateMap<UserObject, UserDbObject>().ReverseMap();
            #endregion 
        }
    }
}
