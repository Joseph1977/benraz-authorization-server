using Authorization.Infrastructure.Gateways.MicrosoftGraph;
using Authorization.Infrastructure.Gateways.MicrosoftGraph.Messages;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Authorization.Infrastructure.Tests
{
    [TestFixture]
    [Ignore("Microsoft graph gateway integration tests")]
    public class MicrosoftGraphGatewayTests
    {
        private const string ACCESS_TOKEN = "eyJ0eXAiOiJKV1QiLCJub25jZSI6Ik02OXZza3MzVzltQjNXbXlUMHctY2ExMHFpbFpWQ3dqaGw0WjVwSmQzVlUiLCJhbGciOiJSUzI1NiIsIng1dCI6InBpVmxsb1FEU01LeGgxbTJ5Z3FHU1ZkZ0ZwQSIsImtpZCI6InBpVmxsb1FEU01LeGgxbTJ5Z3FHU1ZkZ0ZwQSJ9.eyJhdWQiOiIwMDAwMDAwMy0wMDAwLTAwMDAtYzAwMC0wMDAwMDAwMDAwMDAiLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC8wM2YzZWY5OC1mN2VmLTQzOWItYWZhZS02Y2Q2Y2RhZTlmOTYvIiwiaWF0IjoxNTc3Mjc3Nzg4LCJuYmYiOjE1NzcyNzc3ODgsImV4cCI6MTU3NzI4MTY4OCwiYWNjdCI6MSwiYWNyIjoiMSIsImFpbyI6IkFUUUF5LzhOQUFBQXlmQnNZU0pMRmkxY05OM0c5eVc5MTJ5UVJja0IvYUhWYVBzR0ZyakhCMW5ocWJsQnlWMFAzY1IrWEJwaDcvbCsiLCJhbHRzZWNpZCI6IjE6bGl2ZS5jb206MDAwMzdGRkUxOEMzMTlDMSIsImFtciI6WyJwd2QiXSwiYXBwX2Rpc3BsYXluYW1lIjoiUGF5b25lZXIuVHJlYXN1cnkuRGV2IiwiYXBwaWQiOiI1ODM1Y2E4ZC01MjViLTRhYjEtYjNhMC01ZWFkMjEyYmNiMjUiLCJhcHBpZGFjciI6IjEiLCJlbWFpbCI6ImRzc3VkYWtvdkBnbWFpbC5jb20iLCJmYW1pbHlfbmFtZSI6IlN1ZGFrb3YiLCJnaXZlbl9uYW1lIjoiRG1pdHJ5IiwiaWRwIjoibGl2ZS5jb20iLCJpcGFkZHIiOiI4NS4yMzcuMjM0LjEzOSIsIm5hbWUiOiJkc3N1ZGFrb3YiLCJvaWQiOiI0ZDk1MGJkMi0xYzAzLTQzM2ItODBhMC0xZDcxZGU2MjVjODIiLCJwbGF0ZiI6IjMiLCJwdWlkIjoiMTAwMzIwMDA4MUQ2N0Q1MCIsInNjcCI6Ikdyb3VwLlJlYWQuQWxsIFVzZXIuUmVhZCBwcm9maWxlIG9wZW5pZCBlbWFpbCIsInN1YiI6ImF6VWhBb1c1ZEZBSjBneTZ3NURVQ0M5WWMxQ0lGT2t1Vmo0Z0NmakNxWW8iLCJ0aWQiOiIwM2YzZWY5OC1mN2VmLTQzOWItYWZhZS02Y2Q2Y2RhZTlmOTYiLCJ1bmlxdWVfbmFtZSI6ImxpdmUuY29tI2Rzc3VkYWtvdkBnbWFpbC5jb20iLCJ1dGkiOiJXLUpKaUd1YTFVeUY0SHFfYXVJckFBIiwidmVyIjoiMS4wIiwieG1zX3N0Ijp7InN1YiI6IkpKX1ZtV0RXazhwc3hvUTdaUDNkZllxUTJZTVB1NHJHZUZoNEVjbVc2MEEifSwieG1zX3RjZHQiOjE1MjAyMzc3Nzh9.eIkd4p9yerCLKm_440JqXcfBeMPMD_WDRYPVu7W-O8aXu-UF4sasrCYl81IKFpQ5_aFU63VbCVKNMwGJzhBIQumZi1d3b5G0Lk7iW1S5l9ZDtzr4-OcvWcqbSw0Qd8iizQB5VZsAJmR1hjxwDa15heN8wWzo71S54ZNP6qWY7PcU8w9G15yMEBeE1lvaNucb3sdksR_q8s5Z0XKCoP_waYiyLFqxpXllDw-CeVd0zJsdcc3k70ulEcGZlJOmuORyhQTBF84d7VbJP_1Hu8pXYMg1R_7i9y5YwaXaDSACMqiwrPmY0j0LQV-1WDQahVkTp7bvhDyOwEXJaJJvbqXwIA";

        private MicrosoftGraphGateway _gateway;

        [SetUp]
        public void SetUp()
        {
            var settings = new MicrosoftGraphGatewaySettings
            {
                BaseUrl = "https://graph.microsoft.com/v1.0/",
                ProfileEndpoint = "me",
                MemberGroupsEndpoint = "me/getMemberGroups",
                MemberOfEndpoint = "me/memberOf",
                TransitiveMemberOfEndpoint = "me/transitiveMemberOf",
                GroupsEndpoint = "groups"
            };

            var optionsSnapshotMock = new Mock<IOptionsSnapshot<MicrosoftGraphGatewaySettings>>();
            optionsSnapshotMock.Setup(x => x.Value).Returns(settings);

            _gateway = new MicrosoftGraphGateway(optionsSnapshotMock.Object);
        }

        [Test]
        public async Task SendAsync_GetMemberGroupsWithoutAccessToken_ReturnsError()
        {
            var request = new GetMemberGroupsRequest();
            var response = await _gateway.SendAsync(request);

            response.Should().NotBeNull();
            response.Error.Should().NotBeNull();
            response.Error.InnerError.Should().NotBeNull();
        }

        [Test]
        public async Task SendAsync_GetMemberGroupsWithValidAccessToken_ReturnsGroups()
        {
            var request = new GetMemberGroupsRequest
            {
                AccessToken = ACCESS_TOKEN
            };
            var response = await _gateway.SendAsync(request);

            response.Should().NotBeNull();
            response.Error.Should().BeNull();
            response.GroupIds.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task SendAsync_GetGroups_ReturnGroups()
        {
            var request = new GetGroupsRequest
            {
                AccessToken = ACCESS_TOKEN
            };
            var response = await _gateway.SendAsync(request);

            response.Should().NotBeNull();
            response.Error.Should().BeNull();
            response.Groups.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task SendAsync_GetProfile_ReturnsProfile()
        {
            var request = new GetProfileRequest
            {
                AccessToken = ACCESS_TOKEN
            };
            var response = await _gateway.SendAsync(request);

            response.Should().NotBeNull();
            response.Error.Should().BeNull();
            response.Id.Should().NotBeEmpty();
        }

        [Test]
        public async Task SendAsync_GetMemberOfRequest_ReturnsResponse()
        {
            var request = new GetMemberOfRequest
            {
                AccessToken = ACCESS_TOKEN
            };
            var response = await _gateway.SendAsync(request);

            response.Should().NotBeNull();
            response.Error.Should().BeNull();
        }

        [Test]
        public async Task SendAsync_GetTransitiveMemberOfRequest_ReturnsResponse()
        {
            var request = new GetTransitiveMemberOfRequest
            {
                AccessToken = ACCESS_TOKEN
            };
            var response = await _gateway.SendAsync(request);

            response.Should().NotBeNull();
            response.Error.Should().BeNull();
        }

    }
}


