using System;
using System.Net.Http;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions;

using Xunit;

namespace PrecompiledLibraries.Tests
{
    /// <summary>
    /// This represents the test entity for the <see cref="MyHttpTrigger"/> class.
    /// </summary>
    public class MyHttpTriggerTest
    {
        /// <summary>
        /// Tests whether the method should return result or not.
        /// </summary>
        /// <param name="name">Name to be passed to querystring.</param>
        [Theory]
        [InlineData("Azure")]
        [InlineData("Dependency")]
        public async void Given_Name_HttpTrigger_ShouldReturn_Result(string name)
        {
            var req = new HttpRequestMessage()
                      {
                          Content = new StringContent(string.Empty),
                          RequestUri = new Uri($"http://localhost?name={name}")
                      };

            var log = new TraceMonitor();

            var result = await MyHttpTrigger.Run(req, log).ConfigureAwait(false);

            var content = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
            content.Should().ContainEquivalentOf($"Hello {name}");
        }
    }
}