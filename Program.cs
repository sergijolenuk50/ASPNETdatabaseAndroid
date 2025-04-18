using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebCatApi.Abstract;
using WebCatApi.Data;
using WebCatApi.Data.Entities.Identity;
using WebCatApi.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<WebCatDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("CarConnection")));

//User manager
builder.Services.AddIdentity<UserEntity, RoleEntity>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 5;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
}).AddEntityFrameworkStores<WebCatDbContext>().AddDefaultTokenProviders();

builder.Services.AddScoped<IImageService, ImageService>();
//де зустрічається інтерфейс буде створюватись обєкт
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();


//automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
// }

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web Cat v1"));

app.UseAuthorization();

app.MapControllers();

await app.SeedAsync();

app.Run();
