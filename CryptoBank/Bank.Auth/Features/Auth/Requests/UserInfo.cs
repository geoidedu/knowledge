using Bank.Auth.DbAccess;
using Bank.Auth.Features.Auth.Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Bank.Auth.Features.Auth.Requests;
public static class UserInfo
{
    public record Request(ClaimsPrincipal Principal) : IRequest<Response>;
    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator() => RuleFor(x => x.Principal).NotNull();
    }

    public record Response(string? Name, DateTime DateOfRegister, DateTime? DateOfBirth);

    public class RequestHandler : IRequestHandler<Request, Response>
    {
        private readonly AppDbContext _db;
        public RequestHandler(AppDbContext db) => _db = db;

        public Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var nameString = request.Principal.FindFirst(ClaimTypes.Name)?.Value ?? null;

            User user = _db.Users.Where(p => p.UserName == nameString).Single();

            return Task.FromResult(
                new Response(nameString, 
                Convert.ToDateTime(user.DateOfRegister), 
                Convert.ToDateTime(user.DateOfBirth)));
        }
    }
}