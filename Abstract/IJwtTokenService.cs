using WebCatApi.Data.Entities.Identity;

namespace WebCatApi.Abstract;

public interface IJwtTokenService
{
    Task<string> CreateTokenAsync(UserEntity user);
}