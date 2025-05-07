using AutoMapper;
using WebCatApi.Data.Entities;
using WebCatApi.Models.Category;

namespace WebCatApi.Mapper;

public class CategoryMapper : Profile
{
    public CategoryMapper()
    {
        CreateMap<CategoryEntity, CategoryItemViewModel>();

        CreateMap<CategoryCreateViewModel, CategoryEntity>()
            .ForMember(opt => opt.Image, x => x.Ignore());

        CreateMap<CategoryEditViewModel, CategoryEntity>()
            .ForMember(opt => opt.Image, x => x.Ignore());
    }
}
