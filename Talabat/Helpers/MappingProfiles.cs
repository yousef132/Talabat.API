using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities.Cart;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Entities.Product_Aggregate;

namespace Talabat.APIs.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.Brand, options => options.MapFrom(s => s.Brand.Name))
                .ForMember(d => d.Category, options => options.MapFrom(s => s.Category.Name))
                .ForMember(d => d.PictureUrl, options => options.MapFrom<ProductPictureUrlResolver>());


            CreateMap<CustomerCartDto, CustomerCart>();
            CreateMap<CartItemDto, CartItem>();
            CreateMap<UserAddress, AddressDto>().ReverseMap();
            CreateMap<OrderAddressDto, Core.Entities.Order_Aggregate.Address>();


            CreateMap<Order, OrderToReturnDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.DeliveryMethodName, opt => opt.MapFrom(src => src.DeliveryMethod.ShortName))
                .ForMember(dest => dest.DeliveryMethodCost, opt => opt.MapFrom(src => src.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.ProductId))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => src.Product.PictureUrl))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<OrderItemPictureUrlResolver>());



        }
    }
}
