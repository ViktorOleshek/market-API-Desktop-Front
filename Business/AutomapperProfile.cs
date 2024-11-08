using System.Linq;
using Abstraction.Models;
using Data.Entities;
using AutoMapper;

namespace Business
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            this.CreateMap<Receipt, ReceiptModel>()
                .ForMember(rm => rm.Id, c => c.MapFrom(r => r.Id))
                .ForMember(rm => rm.ReceiptDetailsIds, r => r.MapFrom(x => x.ReceiptDetails.Select(rd => rd.Id)))
                .ReverseMap();

            this.CreateMap<Product, ProductModel>()
                .ForMember(cm => cm.Id, c => c.MapFrom(x => x.Id))
                .ForMember(pm => pm.ProductCategoryId, p => p.MapFrom(x => x.ProductCategoryId))
                .ForMember(pm => pm.CategoryName, p => p.MapFrom(x => x.Category.CategoryName))
                .ForMember(pm => pm.ReceiptDetailIds, p => p.MapFrom(x => x.ReceiptDetails.Select(rd => rd.Id)))
                .ReverseMap();

            this.CreateMap<ReceiptDetail, ReceiptDetailModel>()
                .ForMember(cm => cm.Id, c => c.MapFrom(x => x.Id))
                .ReverseMap();

            this.CreateMap<Customer, CustomerModel>()
                .ForMember(cm => cm.Id, c => c.MapFrom(x => x.Id))
                .ForMember(cm => cm.ReceiptsIds, c => c.MapFrom(x => x.Receipts.Select(r => r.Id)))
                .ForMember(cm => cm.Name, c => c.MapFrom(x => x.Person.Name))
                .ForMember(cm => cm.Surname, c => c.MapFrom(x => x.Person.Surname))
                .ForMember(cm => cm.BirthDate, c => c.MapFrom(x => x.Person.BirthDate))
                .ReverseMap();

            this.CreateMap<ProductCategory, ProductCategoryModel>()
                .ForMember(cm => cm.Id, c => c.MapFrom(x => x.Id))
                .ForMember(pcm => pcm.ProductIds, pc => pc.MapFrom(x => x.Products.Select(p => p.Id)))
                .ReverseMap();
        }
    }
}