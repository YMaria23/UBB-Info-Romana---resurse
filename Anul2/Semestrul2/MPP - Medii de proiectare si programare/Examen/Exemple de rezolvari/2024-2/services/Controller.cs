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
            var result = await _service.JoinGame(request.PlayerP);
            //await _hubContext.Clients.All.SendAsync("GameChanged", "status");
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
    [HttpPost("config")]
    public async Task<IActionResult> UpdateConfig([FromBody] ConfigRequest request)
    {
        try
        {
            var result = await _service.UpdateConfig(request.opt1, request.opt2, request.opt3,request.opt4,request.id);
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
    public async Task<IActionResult> Choose([FromBody] RundaRequest request)
    {
        try
        {
            var result = await _service.ChooseLitera(request.opt, request.porecla);
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
            var result = await _service.FinishGame(request.porecla, request.gameId);
            await _hubContext.Clients.All.SendAsync("FinishGame", result);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
    [HttpGet("clasament")]
    public async Task<IActionResult> Finish()
    {
        try
        {
            var result = await _service.Clasament();
            //await _hubContext.Clients.All.SendAsync("Clasament", result);
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
    public string PlayerP { get; set; }
}

 public class ConfigRequest
 {
     public int id { get; set; }
     public string opt1 { get; set; }
     public string opt2 { get; set; }
     public string opt3 { get; set; }
     public string opt4 { get; set; }
 }

 public class RundaRequest
 {
     public string porecla { get; set; }
     public string opt { get; set; }
 }

 public class FinishRequest
 {
     public string porecla { get; set; }
     public int gameId { get; set; }
 }
