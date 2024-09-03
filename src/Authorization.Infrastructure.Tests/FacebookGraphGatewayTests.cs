using Authorization.Infrastructure.Gateways.FacebookGraph;
using Authorization.Infrastructure.Gateways.FacebookGraph.Messages;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authorization.Infrastructure.Tests
{
    [TestFixture]
    [Ignore("Facebook graph gateway integration tests")]
    public class FacebookGraphGatewayTests
    {
        private const string ACCESS_TOKEN = "";

        private FacebookGraphGateway _gateway;

        [SetUp]
        public void SetUp()
        {
            var settings = new FacebookGraphGatewaySettings
            {
                BaseUrl = "https://graph.facebook.com/"
            };

            var optionsSnapshotMock = new Mock<IOptionsSnapshot<FacebookGraphGatewaySettings>>();
            optionsSnapshotMock.Setup(x => x.Value).Returns(settings);

            _gateway = new FacebookGraphGateway(optionsSnapshotMock.Object);
        }

        [Test]
        public async Task SendAsync_GetUserRequestWithoutAccessToken_ReturnsError()
        {
            var request = new GetUserRequest
            {
                Id = "me"
            };
            var response = await _gateway.SendAsync(request);

            response.Should().NotBeNull();
            response.Error.Should().NotBeNull();
        }

        [Test]
        public async Task SendAsync_GetUserRequestWithValidAccessToken_ReturnsUser()
        {
            var request = new GetUserRequest
            {
                AccessToken = ACCESS_TOKEN,
                Id = "me",
                Fields = new List<string> { "name", "email" }

            };
            var response = await _gateway.SendAsync(request);

            response.Should().NotBeNull();
            response.Error.Should().BeNull();
            response.Email.Should().NotBeNullOrEmpty();
        }
    }
}


