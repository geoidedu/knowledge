using Bank.Auth.Authorization;
using Bank.Auth.Features.Auth.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Auth.Features.Auth.Requests.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("register")]
        public async Task<Authenticate.Response> Auth(Authenticate.Request request, CancellationToken cancellationToken) =>
        await _mediator.Send(request, cancellationToken);


        [HttpPost("auth")]
        public async Task<Auth.Response> Auth(Auth.Request request, CancellationToken cancellationToken) =>
        await _mediator.Send(request, cancellationToken);


        [Authorize]
        [HttpGet("info")]
        public Task<UserInfo.Response> GetUserInfo(CancellationToken cancellationToken) =>
       _mediator.Send(new UserInfo.Request(User), cancellationToken);

    }
}
