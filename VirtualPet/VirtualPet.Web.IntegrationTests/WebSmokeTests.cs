using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace VirtualPet.Web.IntegrationTests
{
    public sealed class WebSmokeTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public WebSmokeTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task HomePage_ShouldReturnSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();

            //Act
            var response = await client.GetAsync("/");

            //Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Health_ShouldReturnSuccess()
        {
            //Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/health");

            //Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task PetPage_WhenAnonymous_ShouldRedirectToLogin()
        {
            // Arrange
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            // Act
            var response = await client.GetAsync("/Pet");

            //Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.NotNull(response.Headers.Location);
            Assert.Contains("/Account/Login", response.Headers.Location.OriginalString);
        }
    }
   
}
