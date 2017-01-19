using System;
using System.Net.Http;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions;

using Moq;

using Xunit;

namespace PrecompiledLibraries.Tests
{
    /// <summary>
    /// This represents the test entity for the <see cref="MyHttpTriggerWithDependencies"/> class.
    /// </summary>
    public class MyHttpTriggerWithDependenciesTest
    {
        /// <summary>
        /// Tests whether the method should return result or not.
        /// </summary>
        /// <param name="name">Name to be passed to querystring.</param>
        /// <param name="value">Value to be passed to response.</param>
        [Theory]
        [InlineData("Azure", "Functions")]
        [InlineData("Dependency", "Test")]
        public async void Given_NameWithDependency_HttpTrigger_ShouldReturn_Result(string name, string value)
        {
            Mock<IDependency> mocked = new Mock<IDependency>();
            mocked.SetupGet(p => p.SomeValue).Returns(value);

            MyHttpTriggerWithDependencies.Locator.Dependency = mocked.Object;

            var req = new HttpRequestMessage()
            {
                Content = new StringContent(string.Empty),
                RequestUri = new Uri($"http://localhost?name={name}")
            };

            var log = new TraceMonitor();

            var result = await MyHttpTriggerWithDependencies.Run(req, log).ConfigureAwait(false);

            var content = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
            content.Should().ContainEquivalentOf($"Hello {name}, here's the dependency value of **{value}**");
        }
    }
}
