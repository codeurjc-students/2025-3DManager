using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Catalog;
using _3DMANAGER_APP.BLL.Models.Filament;
using _3DMANAGER_APP.BLL.Models.Group;
using _3DMANAGER_APP.BLL.Models.Print;
using _3DMANAGER_APP.BLL.Models.Printer;
using _3DMANAGER_APP.BLL.Models.User;
using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Models.Filament;
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
                           opt => opt.MapFrom(src => TimeSpan.FromSeconds((double)src.UserHours).TotalHours.ToString("F2")))
                .ReverseMap()
                .ForMember(dest => dest.UserHours,
                           opt => opt.MapFrom(src => (decimal)TimeSpan.FromHours(double.Parse(src.UserHours)).TotalSeconds));


            #endregion

            #region Group
            CreateMap<GroupRequest, GroupRequestDbObject>().ReverseMap();

            #endregion

            #region Filament

            CreateMap<FilamentListResponse, FilamentListResponseDbObject>().ReverseMap();
            CreateMap<FilamentRequest, FilamentRequestDbObject>().ReverseMap();
            #endregion

            #region Print

            CreateMap<PrintListResponse, PrintListResponseDbObject>().ReverseMap();
            CreateMap<PrintRequest, PrintRequestDbObject>().ReverseMap();
            #endregion

            #region Catalog

            CreateMap<CatalogResponse, CatalogResponseDbObject>().ReverseMap();
            #endregion

            #region Printer

            CreateMap<PrinterListObject, PrinterListDbObject>().ReverseMap();
            CreateMap<PrinterRequest, PrinterRequestDbObject>().ReverseMap();
            #endregion
        }
    }
}
