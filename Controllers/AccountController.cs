using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebCatApi.Abstract;
using WebCatApi.Constants;
using WebCatApi.Data.Entities.Identity;
using WebCatApi.Models.Account;

namespace WebCatApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AccountController(UserManager<UserEntity> userManager,
    IMapper mapper, IImageService imageService,
    IJwtTokenService jwtTokenService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model)
    {
        try
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null) return BadRequest(new { error = "Дані вказано не вірно!" });

            if (!await userManager.CheckPasswordAsync(user, model.Password))
                return BadRequest(new { error = "Не вірно вказано дані!" });
            var token = await jwtTokenService.CreateTokenAsync(user);
            return Ok(new { token });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }


    [HttpPost]
    public async Task<IActionResult> Register([FromForm] RegisterViewModel model)
    {
        var user = mapper.Map<UserEntity>(model);
        user.Image = await imageService.SaveImageAsync(model.Image);

        var result = await userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            result = await userManager.AddToRoleAsync(user, Roles.User);
            var token = await jwtTokenService.CreateTokenAsync(user);
            return Ok(new { token });
        }
        else
        {
            return BadRequest(new { errors = result.Errors });
        }
    }
}