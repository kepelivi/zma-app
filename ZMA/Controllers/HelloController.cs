using Microsoft.AspNetCore.Mvc;

namespace ZMA.Controllers;

public class HelloController : ControllerBase
{
    [HttpGet("Hello")]
    public async Task<ActionResult<string>> HelloWorld()
    {
        try
        {
            return Ok("Hello World! I am working.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}