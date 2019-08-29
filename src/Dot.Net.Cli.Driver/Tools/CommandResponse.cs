namespace DotNet.Cli.Driver.Tools
{
    internal class CommandResponse
    {
        internal int Code { get; set; }
        internal string StdOut { get; set; }
        internal string StdErr { get; set; }
    }
}
