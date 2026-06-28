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
    
     // DE AICI INCEPE REST

    [HttpPost("join")]
    public async Task<IActionResult> JoinGame([FromBody] JoinGameRequest request)
    {
        try
        {
            var result = await _service.JoinGame(request.porecla);
            //await _hubContext.Clients.All.SendAsync("GameChanged", "status");
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
    [HttpPost("barci")]
    public async Task<IActionResult> AddBarca([FromBody] AddBarcaRequest request)
    {
        try
        {
            var result = await _service.AddBarca(request.poz1,request.poz2,request.poz3);
            //await _hubContext.Clients.All.SendAsync("GameChanged", "status");
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
    [HttpGet("games/jucatori/{porecla}")]
    public async Task<IActionResult> ViewGames(string porecla)
    {
        try
        {
            var result = await _service.ViewGames(porecla);
            //await _hubContext.Clients.All.SendAsync("GameChanged", "status");
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
    [HttpPost("runde")]
    public async Task<IActionResult> Start([FromBody] StartRequest request)
    {
        try
        {
            var result = await _service.ChoosePoz(request.pozitie,request.porecla,request.m);
            //await _hubContext.Clients.All.SendAsync("GameChanged", "status");
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
    [HttpPost("finish")]
    public async Task<IActionResult> Finish([FromBody] FinishRequest request)
    {
        try
        {
            var result = await _service.FinishGame(request.gameId);
            await _hubContext.Clients.All.SendAsync("GameFinished", "status");
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}

//////////////////////// CLASELE PENTRU REQUESTS ///////////////////////////////

 public class JoinGameRequest
{
    public string porecla { get; set; }
}

 public class AddBarcaRequest
 {
     public string poz1 { get; set; }
     public string poz2 { get; set; }
     public string poz3 { get; set; }
 }
 
 public class StartRequest
 {
     public string pozitie { get; set; }
     public string porecla { get; set; }
     public string[][] m { get; set; }
 }
 
 public class FinishRequest
 {
     public int gameId { get; set; }
 }