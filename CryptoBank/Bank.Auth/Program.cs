using Bank.Auth.Authorization;
using Bank.Auth.DbAccess;
using Bank.Auth.Features.Auth.Domain;
using Bank.Auth.Features.Auth.Options;
using Bank.Auth.Features.Auth.Services;
using Bank.Auth.Features.Auth.Services.crypto;
using Bank.Auth.Pipelines.Behaviors;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddTransient<Argon2Crypto>();
builder.Services.AddMediatR(cfg => cfg
    .RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
    .AddOpenBehavior(typeof(ValidationBehavior<,>)));

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
    var jwtOptions = builder.Configuration.GetSection("Features:Auth").Get<AuthOptions>()!.Jwt;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidAudience = jwtOptions.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey))
    };
});

builder.Services.AddSingleton<IAuthorizationHandler, RoleRequirementHandler>();

builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(PolicyNames.User, policy => policy.AddRequirements(new RoleRequirement(Role.User)));
    //options.AddPolicy(PolicyNames.User, policy => policy.RequireClaim(ClaimTypes.Role, Role.User.ToString()));
    options.AddPolicy(PolicyNames.Analist, policy => policy.AddRequirements(new RoleRequirement(Role.Analist)));
    //options.AddPolicy(PolicyNames.Admin, policy => policy.AddRequirements(new RoleRequirement(Role.Administrator)));
    options.AddPolicy(PolicyNames.Admin, policy => policy.RequireClaim(ClaimTypes.Role, Role.Administrator.ToString()));
});

builder.Services.AddControllers();
builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection("Features:Auth"));
//builder.Services.Configure<CryptoSalt>(builder.Configuration.GetSection("CryptoOptions"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseHttpsRedirection();


app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
