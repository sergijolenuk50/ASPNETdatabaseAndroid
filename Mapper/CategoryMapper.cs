using AutoMapper;
using WebCatApi.Data.Entities;
using WebCatApi.Models.Category;

namespace WebCatApi.Mapper;

public class CategoryMapper : Profile
{
    public CategoryMapper()
    {
        CreateMap<CategoryEntity, CategoryItemViewModel>();
    }
}
