using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Criteo.Api.Exam.Demo.ITest.v1
{
    public class BeersControllerTest
    {
        private static HttpClient Client => ITestSetUpFixture.Client;

        [Test]
        public async Task TestGet()
        {
            var response = await Client.GetAsync("v1/beers");

            response.EnsureSuccessStatusCode();
        }
    }
}
