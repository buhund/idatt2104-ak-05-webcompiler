using System.Diagnostics;
using System.Threading.Tasks;
using System.IO;

namespace Backend.Services;

public class CodeExecutionService
{
    public async Task<string> CompileAndRunCCode(string code)
    {
        // Ensuring the temp directory exists
        var tempPath = Path.Combine(Path.GetFullPath("./temp"), "code.c");
        Directory.CreateDirectory(Path.GetFullPath("./temp")); // Ensure the directory exists

        await File.WriteAllTextAsync(tempPath, code);

        var startInfo = new ProcessStartInfo
        {
            FileName = "docker",
            Arguments = $"run --rm --cpus=\"1.0\" --memory=\"512m\" -v \"{Path.GetDirectoryName(tempPath)}:/app\" gcc:latest gcc /app/code.c -o /app/output && /app/output",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        using var process = new Process { StartInfo = startInfo };
        process.Start();
        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();

        // Consider cleaning up the temp file after execution
        File.Delete(tempPath);

        // Handling the output
        if (!string.IsNullOrEmpty(error))
        {
            return $"Error: {error}";
        }
        return output;
    }
}