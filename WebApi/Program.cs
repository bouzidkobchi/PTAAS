using Microsoft.AspNetCore.Identity;
using WebApi.Data;
using WebApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// adding application db context
builder.Services.AddDbContext<AppDbContext>();

// adding identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(u =>
{
    u.Password.RequiredUniqueChars = 0;
    u.Password.RequireNonAlphanumeric = false;
    u.Password.RequireDigit = false;
    u.Password.RequireLowercase = false;
    u.Password.RequireUppercase = false;
    u.Password.RequiredLength = 0;

    u.SignIn.RequireConfirmedPhoneNumber = false;
    u.SignIn.RequireConfirmedEmail = false;
    u.SignIn.RequireConfirmedAccount = false;

})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// adding authorization 
// builder.Services.AddScoped<RoleManager<IdentityRole>>();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddScoped<RoleManager<IdentityRole>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
