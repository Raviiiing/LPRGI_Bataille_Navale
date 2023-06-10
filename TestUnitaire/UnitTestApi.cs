using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using WireMock.Server;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using API;

namespace TestUnitaire
{
    public class UnitTestApi : IDisposable
    {
        private readonly WireMockServer wireMockServer;
        private readonly string apiUrl;

        public UnitTestApi()
        {
            wireMockServer = WireMockServer.Start();
            apiUrl = $"{wireMockServer.Urls[0]}/api/GetConfig";
        }

        [Fact]
        public async Task CallSuccess()
        {
            var expectedResponse = "{\n  \"nbLignes\": 10,\n  \"nbColonnes\": 10,\n  \"bateaux\": [\n    {\n      \"taille\": 5,\n      \"nom\": \"Porte-avions\"\n    },\n    {\n      \"taille\": 4,\n      \"nom\": \"Croiseur\"\n    },\n    {\n      \"taille\": 3,\n      \"nom\": \"Contre-torpilleur\"\n    },\n    {\n      \"taille\": 3,\n      \"nom\": \"Sous-marin\"\n    },\n    {\n      \"taille\": 2,\n      \"nom\": \"Torpilleur\"\n    }\n  ]\n}";
            var api = new Api("https://api-lprgi.natono.biz/api/GetConfig", "lprgi_api_key_2023");
            var result = await api.GetApiContent();

            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task CallFail()
        {
            wireMockServer
                .Given(Request.Create().WithPath("/api/GetConfig"))
                .RespondWith(Response.Create().WithStatusCode((int)HttpStatusCode.NotFound));

            var api = new Api(apiUrl, "lprgi_api_key_2023");

            await Assert.ThrowsAsync<Exception>(() => api.GetApiContent());
        }

        public void Dispose()
        {
            wireMockServer.Stop();
            wireMockServer.Dispose();
        }
    }
}
