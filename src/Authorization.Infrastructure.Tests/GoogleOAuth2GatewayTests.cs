using Authorization.Infrastructure.Gateways.GoogleOAuth2;
using Authorization.Infrastructure.Gateways.GoogleOAuth2.Messages;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Authorization.Infrastructure.Tests
{
    [TestFixture]
    [Ignore("Google OAuth2 gateway integration tests")]
    public class GoogleOAuth2GatewayTests
    {
        private const string CLIENT_ID = "997217627490-3nna2sd4okglgf4jrnbqmol0pl4icj0q.apps.googleusercontent.com";
        private const string CLIENT_SECRET = "j6K5DNNj5p010bgaV7sG5Yky";
        private const string SCOPE = "openid profile email";
        private const string REDIRECT_URI = "http://localhost:60341/v1/auth/google-callback";

        private GoogleOAuth2Gateway _gateway;

        [SetUp]
        public void SetUp()
        {
            var settings = new GoogleOAuth2GatewaySettings
            {
                AuthorizeEndpointUrl = "https://accounts.google.com/o/oauth2/v2/auth",
                TokenEndpointUrl = "https://oauth2.googleapis.com/token"
            };
            _gateway = new GoogleOAuth2Gateway(settings);
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
                "https://accounts.google.com/o/oauth2/v2/auth" +
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
                Code = "4/wgFrkLunBUSnvTvLwF3CQKc_nND7YAa_Igf7BMq5WgvswYvMQ6QSsnYWLZKkc1svkguTCHidp9g-HV51v6I7N1w",
            };

            var response = await _gateway.SendAsync(request);

            response.Should().NotBeNull();
            response.Error.Should().BeNull();
            response.AccessToken.Should().NotBeNullOrEmpty();
            response.IdToken.Should().NotBeNullOrEmpty();
        }
    }
}


