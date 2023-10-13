using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Newtonsoft.Json;
using src.DTO;

namespace src.IntegrationTests;

public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
        private readonly WebApplicationFactory<Program> _factory;
        private readonly string _host = "http://localhost:8080";

    public IntegrationTests(WebApplicationFactory<Program> factory)
    {
        // Create an instance of HttpClient using the test server provided by WebApplicationFactory
        _factory = factory;
        
    }

    [Fact]
    public async Task GetSwaggerPage_Success_Test()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(_host + "/swagger");

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task PostGame_ValidInput_Success_Test()
    {
        // Arrange
        HttpClient client = _factory.CreateClient();

        PostGameDTO validPostGameRequest = new PostGameDTO {
            HomeTeam = "Northwestern",
            AwayTeam = "Illinois",
            HomeScore = 78,
            AwayScore = 65
        };

        JsonContent content = JsonContent.Create(validPostGameRequest);

        // Act
        var response = await client.PostAsync("/api/RecordedGames", content);

        String objResponseStr = await response.Content.ReadAsStringAsync();
        PostGameDTO? objResponse = JsonConvert.DeserializeObject<PostGameDTO>(objResponseStr);

        // Assert
        Assert.NotNull(objResponse);
        Assert.Equal("ILLINOIS", objResponse.AwayTeam);
        Assert.Equal("NORTHWESTERN", objResponse.HomeTeam);
        Assert.Equal("NORTHWESTERN", objResponse.Winner);
        Assert.Equal(78, objResponse.HomeScore);
        Assert.Equal(65, objResponse.AwayScore);
        Assert.NotNull(objResponse.Date);
    }
}
