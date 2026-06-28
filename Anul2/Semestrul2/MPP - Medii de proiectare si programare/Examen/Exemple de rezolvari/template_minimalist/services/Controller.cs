using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace services;

[ApiController]
[Route("api/game")]
public class TController : ControllerBase
{
    private readonly Service _service;
    private readonly IHubContext<THub> _hubContext;

    public TController(Service service, IHubContext<THub> hubContext)
    {
        _service = service;
        _hubContext = hubContext;
    }
    
    /*
     // DE AICI INCEPE REST

    [HttpPost("join")]
    public async Task<IActionResult> JoinGame([FromBody] JoinGameRequest request)
    {
        try
        {
            var result = await _service.JoinGame(request.PlayerP);
            await _hubContext.Clients.All.SendAsync("GameChanged", "status");
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
     */
}

//////////////////////// CLASELE PENTRU REQUESTS ///////////////////////////////
/*
 public class JoinGameRequest
{
    public string PlayerP { get; set; }
}
*/