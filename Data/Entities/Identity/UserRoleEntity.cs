using Microsoft.AspNetCore.Identity;

namespace WebCatApi.Data.Entities.Identity;

public class UserRoleEntity : IdentityUserRole<long>
{
    public virtual UserEntity User { get; set; } = new();
    public virtual RoleEntity Role { get; set; } = new();
}
