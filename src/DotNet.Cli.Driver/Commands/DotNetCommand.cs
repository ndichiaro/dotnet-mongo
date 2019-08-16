using DotNet.Cli.Driver.Types;

namespace DotNet.Cli.Driver.Commands
{
    public class DotNetCommand
    {
        private readonly string _workingDirectory;

        public DotNetCommand(string workingDirectory)
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
