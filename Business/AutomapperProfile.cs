using System.Linq;
using Abstraction.IEntities;
using Abstraction.Models;
using AutoMapper;

namespace Business
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            _ = this.CreateMap<IReceipt, ReceiptModel>()
                .ForMember(rm => rm.ReceiptDetailsIds, r => r.MapFrom(x => x.ReceiptDetails.Select(rd => rd.Id)))
                .ReverseMap();

            _ = this.CreateMap<IProduct, ProductModel>()
                .ForMember(pm => pm.ProductCategoryId, p => p.MapFrom(x => x.ProductCategoryId))
                .ForMember(pm => pm.CategoryName, p => p.MapFrom(x => x.Category.CategoryName))
                .ForMember(pm => pm.ReceiptDetailIds, p => p.MapFrom(x => x.ReceiptDetails.Select(rd => rd.Id)))
                .ReverseMap();

            _ = this.CreateMap<IReceiptDetail, ReceiptDetailModel>()
                .ReverseMap();

            _ = this.CreateMap<ICustomer, CustomerModel>()
                .ForMember(cm => cm.ReceiptsIds, c => c.MapFrom(x => x.Receipts.Select(r => r.Id)))
                .ForMember(cm => cm.Name, c => c.MapFrom(x => x.Person.Name))
                .ForMember(cm => cm.Surname, c => c.MapFrom(x => x.Person.Surname))
                .ForMember(cm => cm.BirthDate, c => c.MapFrom(x => x.Person.BirthDate))
                .ReverseMap();

            _ = this.CreateMap<IProductCategory, ProductCategoryModel>()
                .ForMember(pcm => pcm.ProductIds, pc => pc.MapFrom(x => x.Products.Select(p => p.Id)))
                .ReverseMap();

            _ = this.CreateMap<IUser, UserModel>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.PersonId))
                .ReverseMap();

        }
    }
}