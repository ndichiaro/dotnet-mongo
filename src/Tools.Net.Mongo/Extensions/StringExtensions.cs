using ConsoleTables;
using System;
using System.Linq;

namespace Tools.Net.Mongo.Extensions
{
    /// <summary>
    /// Provides additional string functionality
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Builds a Console Table from a CSV formatted string
        /// </summary>
        /// <param name="csv"></param>
        /// <returns></returns>
        public static ConsoleTable BuildConsoleTable(this string csv)
        {
            if (string.IsNullOrEmpty(csv))
            {
                throw new ArgumentNullException("csv");
            }

            var rows = csv.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries
            ).ToList();

            // return an empty console table if no rows exist
            if (rows.Count == 0)
            {
                return new ConsoleTable();
            }

            // grab title row from csv
            var titleRowItems = rows[0].Split(',');
            // add title items to row
            var table = new ConsoleTable(titleRowItems);

            // loop through the remaining rows
            foreach (var row in rows.GetRange(1, rows.Count - 1))
            {
                table.AddRow(row.Split(','));
            }
            return table;
        }
    }
}
