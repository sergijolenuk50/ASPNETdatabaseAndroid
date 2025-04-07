using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebCatApi.Data.Entities.Identity;

namespace WebCatApi.Data;

public class WebCatDbContext : IdentityDbContext<UserEntity, RoleEntity, long>
{
    public WebCatDbContext(DbContextOptions<WebCatDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<UserRoleEntity>(ur =>
        {
            ur.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(r => r.RoleId)
                .IsRequired();

            ur.HasOne(ur => ur.User)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(u => u.UserId)
                .IsRequired();
        });
    }
}
