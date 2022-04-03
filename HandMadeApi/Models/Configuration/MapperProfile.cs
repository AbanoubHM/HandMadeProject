using AutoMapper;
using HandMadeApi.Models.DTO.Cart;
using HandMadeApi.Models.DTO.Category;
using HandMadeApi.Models.DTO.Products;
using HandMadeApi.Models.StoreDatabase;

namespace HandMadeApi.Models.Configuration {
    public class MapperProfile:Profile {
        public MapperProfile() {
            CreateMap<Product, DTOCategoryProducts>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ProductImage, opt => opt.MapFrom(src => src.Image))
                .ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.Description));
            CreateMap<CartDetails, CartDetailsDto>();
            CreateMap<CartHeader, CartHeaderDto>();

        }
    }
}
