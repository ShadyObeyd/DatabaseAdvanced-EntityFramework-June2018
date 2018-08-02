namespace _03.ProductShopDatabase
{
    using ReadingDTOS = Data.ReadingDTOs;
    using ToExportDTOS = Data.ToExportDTOs;
    using Data.Models;
    using AutoMapper;
    using System.Linq;

    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            CreateMap<User, ReadingDTOS.UserDTO>().ReverseMap();

            CreateMap<Product, ReadingDTOS.ProductDTO>().ReverseMap();

            CreateMap<Category, ReadingDTOS.CategoryDTO>().ReverseMap();

            CreateMap<Product, ToExportDTOS.ProductInfoDTO>()
                .ForMember(dto => dto.BuyerFullName, opt => opt.MapFrom(src => $"{src.Buyer.FirstName} {src.Buyer.LastName}"));

            CreateMap<User, ToExportDTOS.UserDTO>()
                .ForMember(dto => dto.Products, opt => opt.MapFrom(src => src.ProductsSold));

            CreateMap<Category, ToExportDTOS.CategoryDTO>()
                .ForMember(dto => dto.NumberOfProducts, opt => opt.MapFrom(src => src.CategoryProducts.Count))
                .ForMember(dto => dto.AveragePrice, opt => opt.MapFrom(src => src.CategoryProducts.Select(cp => cp.Product.Price).DefaultIfEmpty(0).Average()))
                .ForMember(dto => dto.TotalRevenue, opt => opt.MapFrom(src => src.CategoryProducts.Sum(cp => cp.Product.Price)));


            CreateMap<Product, ToExportDTOS.ProductWithAttributesDTO>().ReverseMap();
        }
    }
}
