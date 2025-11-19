using _3DMANAGER_APP.BLL.Models;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Filament;
using _3DMANAGER_APP.BLL.Models.Group;
using _3DMANAGER_APP.BLL.Models.Print;
using _3DMANAGER_APP.BLL.Models.User;
using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Models;
using _3DMANAGER_APP.DAL.Models.Filament;
using _3DMANAGER_APP.DAL.Models.Print;
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

            CreateMap<UserCreateRequest, UserCreateRequestDbObject>().ReverseMap();
            CreateMap<UserObject, UserDbObject>().ReverseMap();
            CreateMap<UserListResponse, UserListResponseDbObject>().ReverseMap();
            #endregion

            #region Group
            CreateMap<GroupRequest, GroupRequestDbObject>().ReverseMap();

            #endregion

            #region Filament

            CreateMap<FilamentListResponse, FilamentListResponseDbObject>().ReverseMap();
            #endregion

            #region Print

            CreateMap<PrintListResponse, PrintListResponseDbObject>().ReverseMap();
            #endregion
        }
    }
}
