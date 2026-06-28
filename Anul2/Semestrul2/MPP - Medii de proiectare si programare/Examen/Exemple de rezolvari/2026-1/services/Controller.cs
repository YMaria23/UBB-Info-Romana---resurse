using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace services;

[ApiController]
[Route("api/games")]
public class GameController : ControllerBase
{
    private readonly Service _service;
    private readonly IHubContext<GameHub> _hubContext;

    public GameController(Service service, IHubContext<GameHub> hubContext)
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
            await _hubContext.Clients.All.SendAsync("GameChanged", "status");
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }


    [HttpGet("config")]
    public async Task<IActionResult> ChooseConfig()
    {
        try
        {
            var result = await _service.AlegeConfig();
            await _hubContext.Clients.All.SendAsync("GameChanged", "status");
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
    
    [HttpPost("config")]
    public async Task<IActionResult> ChoosenConfig([FromBody] ChooseConfigRequest request)
    {
        try
        {
            var result = await _service.ChooseConfig(request.chosenConfig);
            await _hubContext.Clients.All.SendAsync("ConfigAleasa", request.chosenConfig);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
    [HttpGet("player")]
    public async Task<IActionResult> FilterGames([FromQuery] string porecla)
    {
        try
        {
            var result = await _service.FilterGames(porecla);
            //await _hubContext.Clients.All.SendAsync("ConfigAleasa", request.chosenConfig);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
    [HttpPost("configs")]
    public async Task<IActionResult> SaveConfig([FromBody] AddConfigRequest request)
    {
        try
        {
            var result = await _service.AddConfig(request.n, request.chosenConfig);
            //await _hubContext.Clients.All.SendAsync("ConfigAleasa", request.chosenConfig);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }


    [HttpGet("mutari")]
    public async Task<IActionResult> StartMoves()
    {
        try
        {
            var result = await _service.StartMoves();
            await _hubContext.Clients.All.SendAsync("NewPlayer", result);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
    [HttpPost("mutari")]
    public async Task<IActionResult> GenerateNumber([FromBody] GenerateNumberRequest request)
    {
        try
        {
            var result = await _service.GenerateNumber(request.generated,request.porecla); 
            await _hubContext.Clients.All.SendAsync("NewScore", result);
            
            var turnData = await _service.StartMoves();
            await _hubContext.Clients.All.SendAsync("NewPlayer", turnData);
            
          
            var finishData = await _service.FinishGame(request.porecla);
            if (finishData != null)
                await _hubContext.Clients.All.SendAsync("FinishGame", finishData);
           
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
    [HttpPost("final")]
    public async Task<IActionResult> FinishGame([FromBody] JoinGameRequest request)
    {
        try
        {
            var result = await _service.FinishGame(request.PlayerP); 
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

}


public class JoinGameRequest
{
    public string PlayerP { get; set; }
}

public class ChooseConfigRequest
{
    public string chosenConfig { get; set; }
}

public class AddConfigRequest
{
    public int n { get; set; }
    public string chosenConfig { get; set; }
}

public class GenerateNumberRequest
{
    public int generated { get; set; }
    public string porecla { get; set; }
}