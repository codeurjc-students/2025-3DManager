using _3DMANAGER_APP.BLL.Models;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Catalog;
using _3DMANAGER_APP.BLL.Models.Filament;
using _3DMANAGER_APP.BLL.Models.Group;
using _3DMANAGER_APP.BLL.Models.Print;
using _3DMANAGER_APP.BLL.Models.Printer;
using _3DMANAGER_APP.BLL.Models.User;
using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Models;
using _3DMANAGER_APP.DAL.Models.Filament;
using _3DMANAGER_APP.DAL.Models.Group;
using _3DMANAGER_APP.DAL.Models.Print;
using _3DMANAGER_APP.DAL.Models.Printer;
using _3DMANAGER_APP.DAL.Models.User;
using AutoMapper;

namespace _3DMANAGER_APP.BLL.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Base
            CreateMap<ErrorDbObject, BaseError>().ReverseMap();
            #endregion

            #region User

            CreateMap<UserCreateRequest, UserCreateRequestDbObject>().ReverseMap();
            CreateMap<UserObject, UserDbObject>().ReverseMap();
            CreateMap<UserListResponseDbObject, UserListResponse>()
                .ForMember(dest => dest.UserHours,
                           opt => opt.MapFrom(src => $"{(int)TimeSpan.FromSeconds((double)src.UserHours).TotalHours}h " +
                           $"{TimeSpan.FromSeconds((double)src.UserHours).Minutes}min"));



            #endregion

            #region Group
            CreateMap<GroupRequest, GroupRequestDbObject>().ReverseMap();
            CreateMap<GroupInvitation, GroupInvitationDbObject>().ReverseMap();

            #endregion

            #region Filament

            CreateMap<FilamentListResponse, FilamentListResponseDbObject>().ReverseMap();
            CreateMap<FilamentRequest, FilamentRequestDbObject>().ReverseMap();
            #endregion

            #region Print

            CreateMap<PrintListResponseDbObject, PrintListResponse>().ForMember(dest => dest.PrintTime,
                           opt => opt.MapFrom(src => $"{(int)TimeSpan.FromSeconds((double)src.PrintTime).TotalHours}h " +
                           $"{TimeSpan.FromSeconds((double)src.PrintTime).Minutes}min"));
            CreateMap<PrintRequest, PrintRequestDbObject>().ReverseMap();
            #endregion

            #region Catalog

            CreateMap<CatalogResponse, CatalogResponseDbObject>().ReverseMap();
            #endregion

            #region Printer

            CreateMap<PrinterListObject, PrinterListDbObject>().ReverseMap();
            CreateMap<PrinterRequest, PrinterRequestDbObject>().ReverseMap();
            CreateMap<PrinterObject, PrinterDbObject>().ReverseMap();
            #endregion
        }
    }
}
