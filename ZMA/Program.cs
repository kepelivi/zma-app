using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ZMA.Data;
using ZMA.Services.Authentication;
using ZMA.Utility;
using Host = ZMA.Model.Host;
using ILogger = ZMA.Utility.ILogger;

var builder = WebApplication.CreateBuilder(args);

ConfigureSwagger();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var config =
    new ConfigurationBuilder()
        .AddUserSecrets<Program>()
        .Build();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ILogger, ConsoleLogger>();
builder.Services.AddScoped<AuthenticationSeeder>();

builder.Services.AddDbContext<ZMAContext>((container, options) =>
    options.UseSqlServer(config["ConnectionString"]));

AddAuthentication();
AddIdentity();
AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("MyAllowSpecificOrigins");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var authenticationSeeder = scope.ServiceProvider.GetRequiredService<AuthenticationSeeder>();

authenticationSeeder.AddRole();

authenticationSeeder.AddHost();

app.Run();

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

void AddCors()
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: "MyAllowSpecificOrigins",
            policy  =>
            {
                policy
                    //.WithOrigins("*") //doesn't work with credentials included
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .SetIsOriginAllowed(origin =>
                    {
                        if (string.IsNullOrWhiteSpace(origin)) return false;
                        // Only add this to allow testing with localhost, remove this line in production!
                        if (origin.ToLower().StartsWith("http://localhost")) return true;
                        // Insert your production domain here.
                        if (origin.ToLower().StartsWith("https://okztfpy-anonymous-8081.exp.direct")) return true;
                        return false;
                    });
            });
    });
}

void AddAuthentication()
{
    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            var issuerSignInKey = config["IssuerSigningKey"] != null
                ? config["IssuerSigningKey"] : Environment.GetEnvironmentVariable("ISSUERSIGNINGKEY");
            var validIssuer = config["ValidIssuer"] != null
                ? config["ValidIssuer"] : Environment.GetEnvironmentVariable("VALIDISSUER");
            var validAudience = config["ValidAudience"] != null
                ? config["ValidAudience"] : Environment.GetEnvironmentVariable("VALIDAUDIENCE");
        
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ClockSkew = TimeSpan.Zero,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = validIssuer,
                ValidAudience = validAudience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(issuerSignInKey)
                ),
            };
            options.Events = new JwtBearerEvents();
            options.Events.OnMessageReceived = context =>
             {
                 if (context.Request.Cookies.ContainsKey("Host"))
                 {
                     context.Token = context.Request.Cookies["Host"];
                 }
            
                 return Task.CompletedTask;
             };
        });
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<ITokenService, TokenService>();
}

void AddIdentity()
{
    builder.Services
        .AddIdentityCore<Host>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ZMAContext>();
    
}