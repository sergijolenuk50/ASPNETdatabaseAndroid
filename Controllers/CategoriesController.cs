using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebCatApi.Abstract;
using WebCatApi.Data;
using WebCatApi.Data.Entities;
using WebCatApi.Data.Entities.Identity;
using WebCatApi.Models.Category;
using WebCatApi.Services;

namespace WebCatApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoriesController(IMapper mapper,
    WebCatDbContext context,
    //UserManager<UserEntity> userManager) : ControllerBase
     UserManager<UserEntity> userManager,
    IImageService imageService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        try
        {
            string userName = User.Claims.FirstOrDefault().Value;
            var user = await userManager.FindByEmailAsync(userName);
            var list = context.Categories
                .Where(x => x.UserId == user.Id)
                .ProjectTo<CategoryItemViewModel>(mapper.ConfigurationProvider)
                .ToList();
            return Ok(list);
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                invalid = ex.Message
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CategoryCreateViewModel model)
    {
        string userName = User.Claims.FirstOrDefault().Value;
        var user = await userManager.FindByEmailAsync(userName);

        var category = mapper.Map<CategoryEntity>(model);
        category.Image = await imageService.SaveImageAsync(model.Image);
        category.UserId = user.Id;

        context.Categories.Add(category);
        context.SaveChanges();
        return Ok(new { id = category.Id });
    }

    [HttpPut]
    public async Task<IActionResult> Edit([FromForm] CategoryEditViewModel model)
    {
        var category = context.Categories.SingleOrDefault(x => x.Id == model.Id);
        if (category == null)
            return NotFound();

        category = mapper.Map(model, category);
        if (model.Image != null)
        {
            string deleteImage = category.Image;
            category.Image = await imageService.SaveImageAsync(model.Image);
            imageService.DeleteImageIfExists(deleteImage);
        }

        context.SaveChanges();
        return Ok(new { id = category.Id });
    }


    [HttpDelete("{id}")]
    public IActionResult Remove(int id)
    {
        var category = context.Categories.SingleOrDefault(x => x.Id == id);
        if (category == null)
            return NotFound();

        if (category.Image != null)
        {
            imageService.DeleteImageIfExists(category.Image);
        }

        context.Categories.Remove(category);
        context.SaveChanges();
        return Ok();
    }

}
