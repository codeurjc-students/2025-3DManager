using _3DMANAGER_APP.BLL.Models;
using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Catalog;
using _3DMANAGER_APP.BLL.Models.Filament;
using _3DMANAGER_APP.BLL.Models.File;
using _3DMANAGER_APP.BLL.Models.Group;
using _3DMANAGER_APP.BLL.Models.Print;
using _3DMANAGER_APP.BLL.Models.Printer;
using _3DMANAGER_APP.BLL.Models.User;
using _3DMANAGER_APP.DAL.Base;
using _3DMANAGER_APP.DAL.Models;
using _3DMANAGER_APP.DAL.Models.Filament;
using _3DMANAGER_APP.DAL.Models.File;
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

            CreateMap<UserDetailDbObject, UserDetailObject>()
                .ForMember(dest => dest.UserTotalHours,
                    opt => opt.MapFrom(src => ConvertHours(src.UserTotalHours)))
                .ForMember(dest => dest.UserPrintHours,
                    opt => opt.MapFrom(src => ConvertHours(src.UserPrintHours)));

            CreateMap<UserUpdateRequest, UserUpdateRequestDbObject>().ReverseMap();

            #endregion

            #region Group
            CreateMap<GroupRequest, GroupRequestDbObject>().ReverseMap();
            CreateMap<GroupInvitation, GroupInvitationDbObject>().ReverseMap();
            CreateMap<GroupBasicDataResponse, GroupBasicDataResponseDbObject>().ReverseMap();
            CreateMap<GroupDashboardDataDbObject, GroupDashboardData>().ForMember(dest => dest.GroupTotalHours,
                opt => opt.MapFrom(src => ConvertHours(src.GroupTotalHours)));
            CreateMap<GroupPrinterHoursDbObject, PrinterHoursObject>().ReverseMap();
            #endregion

            #region Filament

            CreateMap<FilamentListResponse, FilamentListResponseDbObject>().ReverseMap();
            CreateMap<FilamentRequest, FilamentRequestDbObject>().ReverseMap();
            CreateMap<FilamentDetailObject, FilamentDetailDbObject>().ReverseMap();
            CreateMap<FilamentUpdateRequest, FilamentUpdateRequestDbObject>().ReverseMap();

            #endregion

            #region Print

            CreateMap<PrintListResponseDbObject, PrintResponse>().ForMember(dest => dest.PrintTime,
                           opt => opt.MapFrom(src => $"{(int)TimeSpan.FromSeconds((double)src.PrintTime).TotalHours}h " +
                           $"{TimeSpan.FromSeconds((double)src.PrintTime).Minutes}min"));
            CreateMap<PrintRequest, PrintRequestDbObject>().ReverseMap();

            CreateMap<PrintDetailDbObject, PrintDetailObject>().ForMember(dest => dest.PrintTimeImpression,
                           opt => opt.MapFrom(src => $"{(int)TimeSpan.FromSeconds((double)src.PrintTimeImpression).TotalHours}h " +
                           $"{TimeSpan.FromSeconds((double)src.PrintTimeImpression).Minutes}min"))
            .ForMember(dest => dest.PrintRealTimeImpression,
                           opt => opt.MapFrom(src => $"{(int)TimeSpan.FromSeconds((double)src.PrintRealTimeImpression).TotalHours}h " +
                           $"{TimeSpan.FromSeconds((double)src.PrintRealTimeImpression).Minutes}min"))
            .ForMember(dest => dest.PrintEstimedCost, opt => opt.MapFrom(src => src.FilamentCost * src.PrintMaterialConsumed));

            CreateMap<PrintDetailRequest, PrintDetailRequestDbObject>().ReverseMap();
            CreateMap<PrintCommentRequest, PrintCommentRequestDbObject>().ReverseMap();
            CreateMap<PrintCommentObject, PrintCommentDbObject>().ReverseMap();

            #endregion

            #region Catalog

            CreateMap<CatalogResponse, CatalogResponseDbObject>().ReverseMap();
            CreateMap<CatalogPrinterResponse, CatalogResponseDbObject>().ReverseMap();

            #endregion

            #region Printer

            CreateMap<PrinterListObject, PrinterListDbObject>().ReverseMap();
            CreateMap<PrinterRequest, PrinterRequestDbObject>().ReverseMap();
            CreateMap<PrinterObject, PrinterDbObject>().ReverseMap();
            CreateMap<PrinterDetailDbObject, PrinterDetailObject>()
            .ForMember(dest => dest.PrinterTotalHours,
                opt => opt.MapFrom(src => ConvertHours(src.PrinterTotalHours)))
            .ForMember(dest => dest.PrinterTotalHoursMonth,
                opt => opt.MapFrom(src => ConvertHours(src.PrinterTotalHoursMonth)))
            .ForMember(dest => dest.PrinterSuccessRate,
                opt => opt.MapFrom(src =>
                (src.PrinterPrintsComplete + src.PrinterPrintsNoComplete) == 0 ? 0
                : (float)src.PrinterPrintsComplete / (src.PrinterPrintsComplete + src.PrinterPrintsNoComplete)
            ));

            CreateMap<PrinterDetailRequest, PrinterDetailRequestDbObject>();

            #endregion

            #region File
            CreateMap<FileResponse, FileResponseDbObject>().ReverseMap();
            #endregion
        }

        private static string ConvertHours(double value)
        {
            int horas = (int)value;
            int minutos = (int)((value - horas) * 60);

            return $"{horas} h {minutos} minutos";
        }

    }
}
