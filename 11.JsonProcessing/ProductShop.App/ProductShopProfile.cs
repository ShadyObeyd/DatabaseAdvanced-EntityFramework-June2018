namespace ProductShop.App
{
    using AutoMapper;
    using Models;
    using Dto;
    using System.Linq;

    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(dto => dto.SellerFullName, opt => opt.MapFrom(src => $"{src.Seller.FirstName} {src.Seller.LastName}"));

            CreateMap<Product, ProductWithBuyerDTO>()
                .ForMember(dto => dto.BuyerFirstName, opt => opt.MapFrom(src => src.Buyer.FirstName))
                .ForMember(dto => dto.BuyerLastName, opt => opt.MapFrom(src => src.Buyer.LastName));

            CreateMap<User, UserDTO>()
                .ForMember(dto => dto.SoldProducts, opt => opt.MapFrom(src => src.ProductsSold));

            CreateMap<Category, CategoryDTO>()
                .ForMember(dto => dto.ProductCount, opt => opt.MapFrom(src => src.CategoryProducts.Count))
                .ForMember(dto => dto.AveragePrice, opt => opt.MapFrom(src => src.CategoryProducts.Select(cp => cp.Product.Price).DefaultIfEmpty(0).Average()))
                .ForMember(dto => dto.TotalRevenue, opt => opt.MapFrom(src => src.CategoryProducts.Sum(cp => cp.Product.Price)));
        }
    }
}
