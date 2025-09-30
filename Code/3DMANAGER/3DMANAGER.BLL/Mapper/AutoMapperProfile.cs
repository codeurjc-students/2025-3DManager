using _3DMANAGER.API.Models;
using _3DMANAGER.BLL.Models;
using _3DMANAGER.DAL.Base;
using _3DMANAGER.DAL.Models;
using AutoMapper;

namespace _3DMANAGER.BLL.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<PrinterObject, PrinterDbObject>().ReverseMap();
            CreateMap<ErrorDbObject, BaseError>().ReverseMap();
        }
    }
}
