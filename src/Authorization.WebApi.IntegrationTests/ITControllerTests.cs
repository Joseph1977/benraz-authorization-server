using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

namespace Authorization.WebApi.IntegrationTests
{
    [TestFixture]
    class ITControllerTests : ControllerTestsBase
    {
        [Test]
        public async Task IsAliveReturnsOkAsync()
        {
            var response = await HttpClient.GetAsync("/isalive");

            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContentString = await response.Content.ReadAsStringAsync();
            responseContentString.Should().Contain("OK");
        }
    }
}