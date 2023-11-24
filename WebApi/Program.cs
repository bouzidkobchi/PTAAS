using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using WebApi.Data;
using WebApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PTAAS Api",
        Version = "v1",
    });
});

// Adding application db context
builder.Services.AddDbContext<AppDbContext>();

// Adding identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(u =>
{
    // Identity configuration...
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Adding authorization
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddScoped<RoleManager<IdentityRole>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PTAAS Api v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();