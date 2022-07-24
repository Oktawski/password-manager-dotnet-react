using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PasswordManager.Services;
using Microsoft.AspNetCore.Identity;
using PasswordManager.Entities;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("https://192.168.0.111:5050");

// Add services to the container.

builder.Services.AddCors();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();

builder.Services.AddTransient<ClaimsPrincipal>(sp => sp.GetService<IHttpContextAccessor>()!.HttpContext?.User!);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDb")));

builder.Services.AddSingleton<IConfiguration>(sp => builder.Configuration);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;  
    options.RequireHttpsMetadata = false;  
    options.TokenValidationParameters = new TokenValidationParameters()  
    {  
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Secret"))),
        ValidateIssuer = false,  
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    }; 
});

builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

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