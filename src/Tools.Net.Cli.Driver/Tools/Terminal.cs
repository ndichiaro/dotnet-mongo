using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Tools.Net.Cli.Driver.Platform;

namespace Tools.Net.Cli.Driver.Tools
{
    /// <summary>
    /// Represents a systems terminal application
    /// </summary>
    internal static class Terminal
    {
        /// <summary>
        /// The terminal file based on the system
        /// </summary>
        private static string File
        {
            get
            {
                if (OS.Current == OSPlatform.Windows) return "cmd.exe";
                return "/bin/bash";
            }
        }

        /// <summary>
        /// Formats the command to be executed in the terminal
        /// </summary>
        /// <param name="command">The command to be executed</param>
        /// <returns></returns>
        private static string FormatCommand(string command)
        {
            if (OS.Current == OSPlatform.Windows)
                return $"/c \"{command}\"";

            return $"-c \"{command}\"";
        }

        /// <summary>
        /// Executes the command from the terminal
        /// </summary>
        /// <param name="workingDirectory">The executing directory</param>
        /// <param name="command">The command to be executed</param>
        /// <returns></returns>
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
