using Abstraction;
using Application;
using Cqrs.Handlers;
using Microsoft.AspNetCore.Mvc;
using Service;
using System.Net.Mime;

namespace Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IpLookupController : ControllerBase
    {
        private readonly ILogger<IpLookupController> _logger;
        private readonly IIpLookupApi _ipLookupApi;
        private readonly IRequestHandler<GetIpQuery, IpData> _handler;

        public IpLookupController(ILogger<IpLookupController> logger, IRequestHandler<GetIpQuery, IpData> handler)
        {
            _logger = logger;
            _handler = handler;
        }

        [HttpGet("{IpAddress}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IpData), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Get([FromRoute] QueryParams queryParams)
        {
            var response = await _handler.Handle(new GetIpQuery(queryParams.IpAddress), new CancellationToken());
            return response.Status == Cqrs.Model.HandlerStatus.Success
                ? Ok(response.Result)
                : BadRequest(response.Message);
        }
    }
}