using System;
using System.IO;

namespace Tools.Net.Mongo.Migrate.Logging
{
    public sealed class MigrationLogger
    {
        private static readonly Lazy<MigrationLogger> _lazy =
            new Lazy<MigrationLogger>(() => new MigrationLogger());

        public static MigrationLogger Instance { get { return _lazy.Value; } }

        private MigrationLogger()
        {
        }

        public string Log(string migrationName, string content)
        {
            var filePath = $".logs/{migrationName}.log";
            var logFile = new FileInfo(filePath);

            logFile.Directory.Create();

            File.WriteAllText(logFile.FullName, content);

            return filePath;
        }
    }
}
