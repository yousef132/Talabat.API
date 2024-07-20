﻿using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Cart;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.Brand, optoins => optoins.MapFrom(s => s.Brand.Name))
                .ForMember(d => d.Category, options => options.MapFrom(s => s.Category.Name))
                .ForMember(d => d.PictureUrl, options => options.MapFrom<ProductPictureUrlResolver>());


            CreateMap<CustomerCartDto, CustomerCart>();
            CreateMap<CartItemDto, CartItem>();
            CreateMap<Address, AddressDto>().ReverseMap();

        }
    }
}
