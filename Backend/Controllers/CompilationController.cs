using Microsoft.AspNetCore.Mvc;
using Backend.Services;
using System.Threading.Tasks;

namespace Backend.Controllers
{
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
      // Assuming imageName is predetermined for this example.
      // You might want to dynamically select or verify the image name based on the request.
      var imageName = "c_compiler_image";
      var result = await _executionService.CompileAndRun(submission.Code, imageName);
      return Ok(result);
    }
  }

  public class CodeSubmission
  {
    public string Code { get; set; }
  }
}