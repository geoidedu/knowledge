using Bank.Auth.DbAccess;
using Bank.Auth.Features.Auth.Domain;
using Bank.Auth.Features.Auth.Options;
using Bank.Auth.Features.Auth.Services.crypto;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Bank.Auth.Features.Auth.Requests
{
    public static class Authenticate
    {
        public record Request(string Name, string Password) : IRequest<Response>;

        public class RequestValidator : AbstractValidator<Request>
        {
            public RequestValidator()
            {
                RuleFor(x => x.Name).NotEmpty();
                // TODO: password strength
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public record Response(string Jwt);

        public class RequestHandler : IRequestHandler<Request, Response>
        {
            private readonly AuthOptions _options;
            private readonly AppDbContext _db;
            private readonly Argon2Crypto _crypto;

            public RequestHandler(IOptions<AuthOptions> options, AppDbContext db, Argon2Crypto crypto)
            {
                _options = options.Value;
                _db = db;
                _crypto = crypto;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                User? user = await _db.Users.Where(o => o.UserName == request.Name).FirstOrDefaultAsync();
                
                if (user != null)
                {
                    throw new Exception("User существует");
                }

                user = new User { Id = Guid.NewGuid().ToString(), UserName = request.Name, Password = Convert.ToBase64String(_crypto.HashPassword(request.Password)), DateOfRegister = DateTime.Now.ToString(), };
                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();

                var jwt = GenerateJwt(user.UserName, _db.UserRoles.Where(o => o.UserId == user.Id).ToArray());

                return await Task.FromResult(new Response(jwt));
            }

            private string GenerateJwt(string name, UserRole[] roles)
            {
                var claims = new List<Claim>
            {
                new(ClaimTypes.Name, name),
            };

                foreach (var role in roles)
                {
                    claims.Add(new(ClaimTypes.Role, role.ToString()));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Jwt.SigningKey));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expires = DateTime.Now + _options.Jwt.Expiration;

                var token = new JwtSecurityToken(
                    _options.Jwt.Issuer,
                    _options.Jwt.Audience,
                    claims,
                    expires: expires,
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }
    }
}
