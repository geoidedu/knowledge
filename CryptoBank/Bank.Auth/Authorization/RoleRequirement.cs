using Bank.Auth.Features.Auth.Domain;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Bank.Auth.Authorization
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public RoleRequirement(Role requiredRole)
        {
            RequiredRole = requiredRole;
        }

        public Role RequiredRole { get; }
    }

    public class RoleRequirementHandler : AuthorizationHandler<RoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            if (context.User.HasClaim(ClaimTypes.Role, requirement.RequiredRole.ToString()))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
