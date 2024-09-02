using Authorization.Infrastructure.Gateways.MicrosoftOAuth2;
using Authorization.Infrastructure.Gateways.MicrosoftOAuth2.Messages;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Authorization.Infrastructure.Tests
{
    [TestFixture]
    [Ignore("Microsoft OAuth2 gateway integration tests")]
    public class MicrosoftOAuth2GatewayTests
    {
        private const string CLIENT_ID = "95e6ea2b-93ff-4195-a149-db2392ea8a4c";
        private const string CLIENT_SECRET = "secret";

        private MicrosoftOAuth2Gateway _gateway;

        [SetUp]
        public void SetUp()
        {
            var settings = new MicrosoftOAuth2GatewaySettings
            {
                AuthorizeEndpointUrl = "https://login.microsoftonline.com/03f3ef98-f7ef-439b-afae-6cd6cdae9f96/oauth2/v2.0/authorize",
                TokenEndpointUrl = "https://login.microsoftonline.com/03f3ef98-f7ef-439b-afae-6cd6cdae9f96/oauth2/v2.0/token"
            };
            _gateway = new MicrosoftOAuth2Gateway(settings);
        }

        [Test]
        public void CreateAuthorizationUrl_AuthorizeRequest_CreatesUrl()
        {
            var request = new AuthorizeRequest
            {
                ClientId = CLIENT_ID,
                ResponseType = "code",
                Scope = "user.read group.read.all",
                State = "123"
            };

            var url = _gateway.CreateAuthorizationUrl(request);

            url.Should().Be(
                "https://login.microsoftonline.com/03f3ef98-f7ef-439b-afae-6cd6cdae9f96/oauth2/v2.0/authorize" +
                "?response_type=code" +
                "&client_id=95e6ea2b-93ff-4195-a149-db2392ea8a4c" +
                "&scope=user.read group.read.all" +
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
                Scope = "user.read group.read.all",
                Code = "OAQABAAIAAABeAFzDwllzTYGDLh_qYbH8tTh0lTwgkUszmvxUXxsFJzUcQ15yCXtgN0T0hOovi1QQ1VdBoZ3ZnVTvhefB-PPXPvu42ASWt39dbZVC8KvD7N8LNB2GH4hZUjUyX974bIX6z97qYwk93lFczb1IUdXBLZ_iWiETXzGZQq3vQIC3kkxdhfhALcumd3jsDlVTgGLWPfVU3dHSejLfzy97_XHvKDWf4As_5zfPxfWKm9d2vBZkBtQ48oA01OzcOBhsmxQKdTr6jrSWNsLtDYj047yCGbVLrKD0W5CcG-nEAI4k25uhn0XQOPMkwP__JQoZeZW-uvwNB_y4K9MJEEBYQUAFed4zG2TEXVAC7FWZrU8cZzyQWf_GQze7dXVut1qID3xygOozt2v2pCbgOYxE97kdFofA6DFR_hduuvYH4mLkS5frfN39SpAKFI6Hz0jHb7kSzHQmbrR4v2mZ28uNoEYTgWJRqE-Y60OUZrACbxHQ2rMby3ZAFkl1JBmHO70eZsiQB9f1COWOAK4kXrVktsWb1cTIho9uxjyRLQmgHRarrD8cgXNy3TKU0Q6aLR2ui8iTc-xyP9a4JyGxEak56vzPV5OY0B6cUYl_7_IkRXvYyl1BCP-AIy6WHlHMfl0DqQ289Rbhw629vKj908dwui06YjIX8McV8UNdwb8mqKj5n9qUl58mT3QoxpPhV1VA4BpJu6Oiiu9iDDNiHtFQqBoEXodUIUh3MD6q2m7pDdTcVQ4wOeQGgvL2jc2e_n8MuUKiNliKGzMIE1R_Z3XuyvaJE-IetcID1k2D5bWq1bt1ECr0lBeRsCBp65j-FSJ5e9KqnhDiHerkniD97lJEzAVUraoyrzTykeN6oy6nU739riND-NNQiB1uxFXivSvHSpLrae8cm-0dPOV3ks1JzjYVy4WxlBK8WLoaqViyxrHEXRLwn5EtfKMf9006GdzeM432ndGZPm4K1RXFsOsI51calf6FwHMVIr8zkw2pFxdPAskXB2j23lspO_AWsFL-fRVDn5ih-N6-ohXN_4fBmXktKTNFp8vF4Tuif1SBuaD55JJkVQBjrZsHtI9fDfP9MaIpN7XtfNg1J7YqmSsEJYwYLR_2i84VdbAYchQQ2_cXNgQkZtV_-967RD8ABHpqZdd4wA3KvAhcO71Avq5Mjrs2ij5nUXPPgY58y4diN3WrRFXepAbjqXVuQ46xMx3zkmvQx0Mi_TiEIq5m1Li6jFmnYzogJ9wkaZEzAx41PSMTtCAA",
            };

            var token = await _gateway.SendAsync(request);

            token.Should().NotBeNull();
            token.AccessToken.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task SendAsync_TokenRequestWithPassword_ReturnsTokenResponse()
        {
            var request = new TokenRequest
            {
                ClientId = CLIENT_ID,
                ClientSecret = CLIENT_SECRET,
                GrantType = "password",
                Scope = "user.read group.read.all",
                Username = "username",
                Password = "password"
            };

            var token = await _gateway.SendAsync(request);

            token.Should().NotBeNull();
            token.AccessToken.Should().NotBeNullOrEmpty();
        }
    }
}


