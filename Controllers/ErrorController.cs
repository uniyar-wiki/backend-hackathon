using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace UniYarWiki.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ErrorController : ControllerBase
    {
        [HttpPost("log")]
        public async Task<IActionResult> LogError([FromBody] ErrorLogDto errorLog)
        {
            var logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "errorLog.txt");
            var logMessage = $"{errorLog.Timestamp}: {errorLog.Message}\n";

            await System.IO.File.AppendAllTextAsync(logFilePath, logMessage);

            return Ok();
        }
    }

    public class ErrorLogDto
    {
        public string Message { get; set; }
        public string Timestamp { get; set; }
    }
}
