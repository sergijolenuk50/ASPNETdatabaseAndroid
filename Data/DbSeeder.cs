using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebCatApi.Abstract;
using WebCatApi.Constants;
using WebCatApi.Data.Entities.Identity;
using WebCatApi.Models.Seeder;

namespace WebCatApi.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<WebCatDbContext>();

        context.Database.Migrate(); //Запускає команду автоматично Update-Database

        if (!context.Roles.Any())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleEntity>>();
            await roleManager.CreateAsync(new() { Name = Roles.Admin });
            await roleManager.CreateAsync(new() { Name = Roles.User });
        }

        if (!context.Users.Any())
        {
            var imageService = scope.ServiceProvider.GetRequiredService<IImageService>();
            var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "Users.json");
            if (File.Exists(jsonFile))
            {
                var jsonData = File.ReadAllText(jsonFile, Encoding.UTF8);
                try
                {
                    var users = JsonConvert.DeserializeObject<List<SeederUserModel>>(jsonData)
                        ?? throw new JsonException();
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();
                    foreach (var user in users)
                    {
                        var newUser = new UserEntity
                        {
                            UserName = user.Email,
                            PhoneNumber = user.PhoneNumber,
                            Email = user.Email,
                            Firstname = user.Firstname,
                            Lastname = user.Lastname,
                            Image = await imageService.SaveImageFromUrlAsync(user.Image)
                        };
                        var result = await userManager.CreateAsync(newUser, user.Password);
                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(newUser, user.Role);
                        }
                        else Console.WriteLine($"--Error create user {user.Email}--");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--Error parse json--{ex.Message}");
                }
            }
            else Console.WriteLine($"--Error open file {jsonFile}--");
        }
    }
}