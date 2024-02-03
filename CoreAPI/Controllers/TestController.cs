
using AppCore.UseCases.DT.Commands;
using AppCore.UseCases.DT.Queries;
using Microsoft.AspNetCore.Mvc;

namespace CoreAPI.Controllers
{

    [ApiController]
    [Route("api/v1/DTCostings")]
    public class TestController : AppControllerBase
    {
        [HttpPost("mains")]
        public async Task<IActionResult> DTCostingMain(DTCostingMain.Command @event)
        {
            return Ok(await Mediator.Send(@event));
        }

        //[HttpPost("search")]
        //public async Task<IActionResult> DTCostingMainSearch(DTCostingMain.Command @event)
        //{
        //    return Ok(await Mediator.Send(@event));
        //}

        //[HttpGet("{keyWord:long}/searchCosting")]
        [HttpGet("searchCosting")]
        public async Task<IActionResult> GetCostingSearch(
        [FromQuery] DTCostingSearch.Query query, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(query, cancellationToken);
            return Ok(result);
        }
        [HttpGet("searchCosting2")]
        public async Task<IActionResult> GetCostingSearch2(
      [FromQuery] DTCostingSearch.Query query, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}