using DotNet.Cli.Driver.Commands;
using DotNet.Cli.Driver.Types;

namespace DotNet.Cli.Driver
{
    public class DotNetCLI
    {
        private readonly string _workingDirectory;

        public DotNetCLI(string workingDirectory)
        {
            _workingDirectory = workingDirectory;
        }

        public BuildCommand Build(BuildConfiguration configuration)
        {
            return new BuildCommand(configuration)
            {
                WorkingDirectory = _workingDirectory
            };
        }
    }
}
