using Authorization.Infrastructure.Gateways.FacebookOAuth2;
using Authorization.Infrastructure.Gateways.FacebookOAuth2.Messages;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Authorization.Infrastructure.Tests
{
    [TestFixture]
    [Ignore("Facebook OAuth2 gateway integration tests")]
    public class FacebookOAuth2GatewayTests
    {
        private const string CLIENT_ID = "618257302066668";
        private const string CLIENT_SECRET = "4a33b450af275109c105e3f97740dadd";
        private const string SCOPE = "email";
        private const string REDIRECT_URI = "http://localhost:60341/v1/auth/facebook-callback";

        private FacebookOAuth2Gateway _gateway;

        [SetUp]
        public void SetUp()
        {
            var settings = new FacebookOAuth2GatewaySettings
            {
                AuthorizeEndpointUrl = "https://www.facebook.com/v6.0/dialog/oauth",
                TokenEndpointUrl = "https://graph.facebook.com/v6.0/oauth/access_token"
            };
            _gateway = new FacebookOAuth2Gateway(settings);
        }

        [Test]
        public void CreateAuthorizationUrl_AuthorizeRequest_CreatesUrl()
        {
            var request = new AuthorizeRequest
            {
                ClientId = CLIENT_ID,
                ResponseType = "code",
                Scope = SCOPE,
                RedirectUri = REDIRECT_URI,
                State = "123"
            };

            var url = _gateway.CreateAuthorizationUrl(request);

            url.Should().Be(
                "https://www.facebook.com/v6.0/dialog/oauth" +
                "?response_type=code" +
                $"&client_id={CLIENT_ID}" +
                $"&scope={SCOPE}" +
                $"&redirect_uri={REDIRECT_URI}" +
                "&state=123");
        }

        [Test]
        public async Task SendAsync_TokenRequestWithCode_ReturnsTokenResponse()
        {
            var request = new TokenRequest
            {
                ClientId = CLIENT_ID,
                ClientSecret = CLIENT_SECRET,
                GrantType = "authorization_code",
                Scope = SCOPE,
                RedirectUri = REDIRECT_URI,
                Code = "AQCx5L5v7ddf7JV6xUpIrhpLhDxl2cdrKOtlvRZNxPnBXvuCbxKOrpS64j76S9WJDQbGeWbYBSD3VucOT97l9ILGdmh21vvL854I_hswjPB4jouR6SmOQ1AJ0bqbl7YmzMg5yfrGMFUgOU0jM1VXACVpchWOhgxOp8MaMF_qAJ2Abx290eCJQFqAiNfguMgv0sRnPwwWA-9ewWAutxpDPFAXK-BI7LfLnutr2IFb7EuAs0Lizjn0cHa-FMzztDqulu_6ogCgdOiXxe4cWczomhmcqogOAzG7MCO9XwzyKXLVqXLKnZNMFBUDFfjqxwwQhjyqz-y2elnJEGcDBWvTvy_Y",
            };

            var response = await _gateway.SendAsync(request);

            response.Should().NotBeNull();
            response.Error.Should().BeNull();
            response.AccessToken.Should().NotBeNullOrEmpty();
        }
    }
}


