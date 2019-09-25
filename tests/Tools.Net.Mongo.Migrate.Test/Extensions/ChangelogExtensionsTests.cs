using System;
using System.Collections.Generic;
using Tools.Net.Mongo.Migrate.Models;
using Tools.Net.Mongo.Migrate.Extensions;
using Xunit;

namespace Tools.Net.Mongo.Migrate.Test.Extensions
{
    /// <summary>
    /// A suite of unit tests that valiate the functionality of
    /// the ChangelogExtensions class
    /// </summary>
    public class ChangelogExtensionsTests
    {
        /// <summary>
        /// Tests the the latest change can be retrieved
        /// </summary>
        [Fact]
        public void CanGetLatestChange()
        {
            var logs = new List<Changelog>()
            {
                new Changelog { Id = "1", AppliedAt=new DateTime(2019, 5, 15)},
                new Changelog { Id = "2", AppliedAt=new DateTime(2019, 9, 13)},
                new Changelog { Id = "3", AppliedAt=new DateTime(2019, 7, 1)}
            };

            var latestChage = logs.GetLatestChange();

            // validate the log from 9-13-2019 is returned
            Assert.Equal(logs[1], latestChage);
        }
    }
}
