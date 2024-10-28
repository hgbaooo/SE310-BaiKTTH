using AutoMapper;
using SE310.P12_BaiKTTH_BE.Dto;
using SE310.P12_BaiKTTH_BE.Models;

namespace SE310.P12_BaiKTTH_BE.Helper;

public class MappingProfiles: Profile
{
    public MappingProfiles()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<ProductDto, Product>();
        CreateMap<Product, GetProductDto>();
        CreateMap<GetProductDto, Product>();
        CreateMap<Category, CategoryDto>();
        CreateMap<CategoryDto, Category>();
        CreateMap<Category, GetCategoryDto>();
        CreateMap<GetCategoryDto, Category>();
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
    }
}