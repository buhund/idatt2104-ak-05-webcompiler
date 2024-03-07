using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

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
            Arguments = $"run --rm --cpus=\"1.0\" --memory=\"512m\" -v \"{Path.GetDirectoryName(tempPath)}:/app\" gcc:latest /bin/bash -c \"gcc /app/code.c -o /app/compiled_program && /app/compiled_program\"",
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

        // Log the Docker process output and error
        Console.WriteLine("Docker Output:");
        Console.WriteLine(output);
        if (!string.IsNullOrEmpty(error))
        {
            Console.WriteLine("Docker Error:");
            Console.WriteLine(error);
        }

        // Clean up temp file after execution
        File.Delete(tempPath);

        return string.IsNullOrEmpty(error) ? output : $"Error: {error}";
    }


}