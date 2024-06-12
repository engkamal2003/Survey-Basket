

//namespace SurveyBasket.Controllers;

//[Route("api/[controller]")]
//[ApiController]
//[Authorize]
//public class PollsController : ControllerBase

//{
//    private readonly IPollService _pollService;

//    public PollsController(IPollService pollService)
//    {
//        _pollService = pollService;
//    }

//    [HttpGet("")]
//    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
//    {
//        var polls = await _pollService.GetAllAsync(cancellationToken);
//        var response = polls.Adapt<IEnumerable<PollResponse>>();
//        return Ok(response);
//    }

//    [HttpGet("{id}")]
//    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
//    {
//        var poll = await _pollService.GetAsync(id, cancellationToken);
//        if (poll is null)
//            return NotFound();

//        var response = poll.Adapt<PollResponse>();
//        //var response = _mapper.Map<PollResponse>(poll);

//        return Ok(response);
//    }

//    [HttpPost("")]
//    public async Task<IActionResult> Add([FromBody] PollRequest request,
//        CancellationToken cancellationToken)
//    {
//        var newPoll = await _pollService.AddAsync(request.Adapt<Poll>(), cancellationToken);
//        return CreatedAtAction(nameof(Get), new { id = newPoll.Id }, newPoll.Adapt<PollResponse>());
//    }

//    [HttpPut("{id}")]
//    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request,
//        CancellationToken cancellationToken)
//    {
//        var isUpdated = await _pollService.UpdateAsync(id, request.Adapt<Poll>(), cancellationToken);

//        if (!isUpdated)
//            return NotFound();

//        return NoContent();
//    }

//    [HttpDelete("{id}")]
//    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
//    {
//        var isDeleted = await _pollService.DeleteAsync(id, cancellationToken);

//        if (!isDeleted)
//            return NotFound();

//        return NoContent();
//    }

//    [HttpPut("{id}/togglePublish")]
//    public async Task<IActionResult> TogglePublish([FromRoute] int id, CancellationToken cancellationToken)
//    {
//        var isUpdated = await _pollService.TogglePublishStatusAsync(id, cancellationToken);

//        if (!isUpdated)
//            return NotFound();

//        return NoContent();
//    }
//}

//using Microsoft.AspNetCore.Authorization;

//namespace SurveyBasket.Controllers;

//[Route("api/[controller]")]
//[ApiController]
//[Authorize]
//public class PollsController(IPollService pollService) : ControllerBase
//{
//    private readonly IPollService _pollService = pollService;

//    [HttpGet("")]
//    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
//    {
//        var polls = await _pollService.GetAllAsync(cancellationToken);

//        var response = polls.Adapt<IEnumerable<PollResponse>>();

//        return Ok(response);
//    }

//    [HttpGet("{id}")]
//    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
//    {
//        var result = await _pollService.GetAsync(id, cancellationToken);

//        return result.IsSuccess
//            ? Ok(result.Value)
//            : Problem(statusCode: StatusCodes.Status404NotFound, title: result.Error.Code, detail: result.Error.Description);
//    }

//    [HttpPost("")]
//    public async Task<IActionResult> Add([FromBody] PollRequest request,
//        CancellationToken cancellationToken)
//    {
//        var newPoll = await _pollService.AddAsync(request, cancellationToken);

//        return CreatedAtAction(nameof(Get), new { id = newPoll.Id }, newPoll);
//    }

//    [HttpPut("{id}")]
//    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request,
//        CancellationToken cancellationToken)
//    {
//        var result = await _pollService.UpdateAsync(id, request, cancellationToken);

//        return result.IsSuccess ? NoContent() : NotFound(result.Error);
//    }

//    [HttpDelete("{id}")]
//    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
//    {
//        var result = await _pollService.DeleteAsync(id, cancellationToken);

//        return result.IsSuccess ? NoContent() : Problem(statusCode: StatusCodes.Status404NotFound, title: result.Error.Code, detail: result.Error.Description);
//    }

//    [HttpPut("{id}/togglePublish")]
//    public async Task<IActionResult> TogglePublish([FromRoute] int id, CancellationToken cancellationToken)
//    {
//        var result = await _pollService.TogglePublishStatusAsync(id, cancellationToken);

//        return result.IsSuccess ? NoContent() : Problem(statusCode: StatusCodes.Status404NotFound, title: result.Error.Code, detail: result.Error.Description);
//    }
//}




using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SurveyBasket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PollsController : ControllerBase
    {
        private readonly IPollService _pollService;

        public PollsController(IPollService pollService)
        {
            _pollService = pollService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var polls = await _pollService.GetAllAsync(cancellationToken);

            var response = polls.Adapt<IEnumerable<PollResponse>>();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _pollService.GetAsync(id, cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : Problem(statusCode: StatusCodes.Status404NotFound, title: result.Error.Code, detail: result.Error.Description);
        }

        [HttpPost("")]
        public async Task<IActionResult> Add([FromBody] PollRequest request, CancellationToken cancellationToken)
        {
            var newPoll = await _pollService.AddAsync(request, cancellationToken);

            return CreatedAtAction(nameof(Get), new { id = newPoll.Id }, newPoll);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request, CancellationToken cancellationToken)
        {
            var result = await _pollService.UpdateAsync(id, request, cancellationToken);

            return result.IsSuccess ? NoContent() : NotFound(result.Error);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _pollService.DeleteAsync(id, cancellationToken);

            return result.IsSuccess ? NoContent() : Problem(statusCode: StatusCodes.Status404NotFound, title: result.Error.Code, detail: result.Error.Description);
        }

        [HttpPut("{id}/togglePublish")]
        public async Task<IActionResult> TogglePublish([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _pollService.TogglePublishStatusAsync(id, cancellationToken);

            return result.IsSuccess ? NoContent() : Problem(statusCode: StatusCodes.Status404NotFound, title: result.Error.Code, detail: result.Error.Description);
        }
    }
}
