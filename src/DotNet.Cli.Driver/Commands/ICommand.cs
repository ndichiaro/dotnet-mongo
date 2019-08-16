namespace DotNet.Cli.Driver.Commands
{
    internal interface ICommand
    {
        string WorkingDirectory { get; set; }
        void Create();
    }
}
