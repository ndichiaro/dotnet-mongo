using DotNet.Cli.Driver.Commands;

namespace DotNet.Cli.Driver
{
    public static class CLI
    {
        public static DotNetCommand DotNet(string workingDirectory)
        {
            return new DotNetCommand(workingDirectory);
        }
    }
}
