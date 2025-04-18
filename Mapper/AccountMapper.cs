using AutoMapper;
using WebCatApi.Data.Entities.Identity;
using WebCatApi.Models.Account;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebCatApi.Mapper;

public class AccountMapper : Profile
{
    public AccountMapper()
    {
        CreateMap<RegisterViewModel, UserEntity>()
            .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.Email));
    }
}