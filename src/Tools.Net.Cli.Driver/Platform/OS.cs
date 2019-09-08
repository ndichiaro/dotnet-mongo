using System;
using System.Runtime.InteropServices;

namespace Tools.Net.Cli.Driver.Platform
{
    /// <summary>
    /// Represents an operating system platform
    /// </summary>
    internal static class OS
    {
        /// <summary>
        /// The current Operating System
        /// </summary>
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
