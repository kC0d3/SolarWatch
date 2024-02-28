using System.Text;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SolarWatch.Data;
using SolarWatch.Services;
using SolarWatch.Services.Authentication;
using SolarWatch.Services.CityDataProvider;
using SolarWatch.Services.CityService;
using SolarWatch.Services.JsonProcessor;
using SolarWatch.Services.Repository;
using SolarWatch.Services.SolarDataProvider;
using SolarWatch.Services.SolarService;

var connectionString = ConnectionString.GetConnectionString();

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

var root = Directory.GetCurrentDirectory();
var dotenv = Path.Combine(root, "..", "..", ".env");
Env.Load(dotenv);

var builder = WebApplication.CreateBuilder(args);

AddServices();
ConfigureSwagger();
AddDbContext();
AddAuthentication();
AddIdentity();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseAuthentication();
//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.Services.InitializeDbAsync();
var rolesService = app.Services.GetService<IRolesService>();
rolesService.AddRoles(app);
rolesService.AddAdmin(app);


app.Run();

void AddServices()
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSingleton<ICityDataProvider, CityDataProvider>();
    builder.Services.AddSingleton<ISolarDataProvider, SolarDataProvider>();
    builder.Services.AddSingleton<IJsonProcessor, JsonProcessor>();
    builder.Services.AddSingleton<ICityRepository, CityRepository>();
    builder.Services.AddSingleton<ISolarRepository, SolarRepository>();
    builder.Services.AddSingleton<ICityService, CityService>();
    builder.Services.AddSingleton<ISolarService, SolarService>();
    builder.Services.AddSingleton(configuration);
    builder.Services.AddSingleton(dotenv);
    builder.Services.AddSingleton<IRolesService, RolesService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<ITokenService, TokenService>();
}

void ConfigureSwagger()
{
    builder.Services.AddSwaggerGen(option =>
    {
        option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        });
    });
}

void AddDbContext()
{
    builder.Services.AddDbContext<SolarWatchContext>(optionsBuilder =>
        optionsBuilder.UseSqlServer(connectionString));
    builder.Services.AddDbContext<UsersContext>(optionsBuilder =>
        optionsBuilder.UseSqlServer(connectionString));
}

void AddAuthentication()
{
    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ClockSkew = TimeSpan.Zero,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["ValidIssuer"],
                ValidAudience = configuration["ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("ISSUERSIGNINGKEY"))
                ),
            };
        });
}

void AddIdentity()
{
    builder.Services
        .AddIdentityCore<IdentityUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<UsersContext>();
}