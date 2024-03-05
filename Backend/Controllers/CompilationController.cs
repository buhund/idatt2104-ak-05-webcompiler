using Microsoft.AspNetCore.Mvc;
using Backend.Services;
using System.Threading.Tasks;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompilationController : ControllerBase
{
    private readonly CodeExecutionService _executionService;

    public CompilationController(CodeExecutionService executionService)
    {
        _executionService = executionService;
    }

    [HttpPost("compile")]
    public async Task<IActionResult> CompileAndRun([FromBody] CodeSubmission submission)
    {
        // Input validation
        if (string.IsNullOrWhiteSpace(submission.Code))
        {
            return BadRequest("Code submission cannot be empty.");
        }

        try
        {
            var result = await _executionService.CompileAndRunCCode(submission.Code);
            return Ok(result);
        }
        catch (Exception ex)
        {
            // Log the exception here (e.g., using a logging framework or simply writing to console)
            Console.WriteLine(ex.ToString());
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}

public class CodeSubmission
{
    public string Code { get; set; }
}