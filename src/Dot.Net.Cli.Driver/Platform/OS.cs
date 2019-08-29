using System;
using System.Runtime.InteropServices;

namespace DotNet.Cli.Driver.Platform
{
    internal static class OS
    {
        internal static OSPlatform Current
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return OSPlatform.Windows;

                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) return OSPlatform.OSX;

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) return OSPlatform.Linux;

                throw new PlatformNotSupportedException();
            }
        }
    }
}
