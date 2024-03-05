using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Backend.Services
{
    public class CodeExecutionService
    {
        public async Task<string> CompileAndRun(string code, string imageName)
        {
            // Save the code to a temporary file
            var codePath = Path.Combine(Path.GetFullPath("./temp"), "code.c");
            await File.WriteAllTextAsync(codePath, code);

            // Adjust the Docker command
            var startInfo = new ProcessStartInfo
            {
                FileName = "docker",
                Arguments = $"run --rm -v {Path.GetFullPath("./temp")}:/app {imageName} sh -c 'gcc -o /app/output /app/code.c && /app/output'",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };
            using var process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            string output = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();

            return string.IsNullOrEmpty(error) ? output : error;
        }
    }
}