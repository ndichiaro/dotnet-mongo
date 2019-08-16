using DotNet.Cli.Driver.Tools;
using DotNet.Cli.Driver.Types;

namespace DotNet.Cli.Driver.Commands
{
    public class BuildCommand : ICommand
    {
        private readonly BuildConfiguration _configuration;

        public string WorkingDirectory { get; set; }

        internal BuildCommand(BuildConfiguration configuration)
        {
            _configuration = configuration;

            Terminal.Execute("dotnet --version");
        }

        public void Create()
        {
            throw new System.NotImplementedException();
        }
    }
}
