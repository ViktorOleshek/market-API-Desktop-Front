using System.Linq;
using Abstraction.Entities;
using Abstraction.Models;
using AutoMapper;

namespace Business
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            _ = this.CreateMap<Receipt, ReceiptModel>()
                .ForMember(rm => rm.ReceiptDetailsIds, r => r.MapFrom(x => x.ReceiptDetails.Select(rd => rd.Id)))
                .ReverseMap();

            _ = this.CreateMap<Product, ProductModel>()
                .ForMember(pm => pm.ProductCategoryId, p => p.MapFrom(x => x.ProductCategoryId))
                .ForMember(pm => pm.CategoryName, p => p.MapFrom(x => x.Category.CategoryName))
                .ForMember(pm => pm.ReceiptDetailIds, p => p.MapFrom(x => x.ReceiptDetails.Select(rd => rd.Id)))
                .ReverseMap();

            _ = this.CreateMap<ReceiptDetail, ReceiptDetailModel>()
                .ReverseMap();

            _ = this.CreateMap<Customer, CustomerModel>()
                .ForMember(cm => cm.ReceiptsIds, c => c.MapFrom(x => x.Receipts.Select(r => r.Id)))
                .ForMember(cm => cm.Name, c => c.MapFrom(x => x.Person.Name))
                .ForMember(cm => cm.Surname, c => c.MapFrom(x => x.Person.Surname))
                .ForMember(cm => cm.BirthDate, c => c.MapFrom(x => x.Person.BirthDate))
                .ReverseMap();

            _ = this.CreateMap<ProductCategory, ProductCategoryModel>()
                .ForMember(pcm => pcm.ProductIds, pc => pc.MapFrom(x => x.Products.Select(p => p.Id)))
                .ReverseMap();

            _ = this.CreateMap<User, UserModel>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.PersonId))
                .ReverseMap();

        }
    }
}