using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PasswordManager.Configuration;
using PasswordManager.Models;
using PasswordManager.Services;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("https://192.168.0.111:5050");

builder.Services.AddCors();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDb")!));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services
    .ConfigureSecretOptions(builder.Configuration)
    .AddJwtAuthentication(builder.Configuration)
    .AddBusinessServices()
    .AddClaimsPrincipal()
    .AddTokenService();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors(builder => 
{
    builder.AllowAnyHeader()
        .AllowAnyMethod()
        .WithOrigins("https://192.168.0.111:44400",
            "https://192.168.0.111:5050",
            "https://localhost:44400", 
            "http://localhost:44400" 
        );
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");;

app.Run();