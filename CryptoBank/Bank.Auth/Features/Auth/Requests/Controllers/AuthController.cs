using MediatR;
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
        public async Task<Authenticate.Response> Auth(Authenticate.Request request, CancellationToken cancellationToken)
        {
            return await _mediator.Send(request, cancellationToken);
        }


    }
}
