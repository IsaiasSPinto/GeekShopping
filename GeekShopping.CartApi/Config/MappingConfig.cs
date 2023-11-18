using AutoMapper;
using GeekShopping.CartApi.Data.Dtos;
using GeekShopping.CartApi.Model;

namespace GeekShopping.CartApi.Config;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        return new(cfg =>
        {
            cfg.CreateMap<ProductDto, Product>().ReverseMap();
            cfg.CreateMap<CartHeaderDto, CartHeader>().ReverseMap();
            cfg.CreateMap<CartDatailDto, CartDatail>().ReverseMap();
            cfg.CreateMap<CartDto, Cart>().ReverseMap();
        });
    }
}
