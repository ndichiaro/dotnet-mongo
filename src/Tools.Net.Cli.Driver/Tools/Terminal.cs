using Tools.Net.Cli.Driver.Platform;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Tools.Net.Cli.Driver.Tools
{
    internal static class Terminal
    {
        private static string File
        {
            get
            {
                if (OS.Current == OSPlatform.Windows) return "cmd.exe";
                return "/bin/bash";
            }
        }

        private static string FormatCommand(string command)
        {
            if (OS.Current == OSPlatform.Windows)
                return $"/c \"{command}\"";

            return $"-c \"{command}\"";
        }

        internal static CommandResponse Execute(string workingDirectory, string command)
        {
            var result = new CommandResponse();
            var stderr = new StringBuilder();
            var stdout = new StringBuilder();

            var startInfo = new ProcessStartInfo
            {
                FileName = File,
                Arguments = FormatCommand(command),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WorkingDirectory = workingDirectory
            };

            using (var process = Process.Start(startInfo))
            {
                while (!process.StandardOutput.EndOfStream)
                {
                    var line = process.StandardOutput.ReadLine();
                    stdout.AppendLine(line);
                }

                while (!process.StandardError.EndOfStream)
                {
                    var line = process.StandardError.ReadLine();
                    stderr.AppendLine(line);
                }
                process.WaitForExit();

                result.StdOut = stdout.ToString();
                result.StdErr = stderr.ToString();
                result.Code = process.ExitCode;
            }
            return result;
        }
    }
}
