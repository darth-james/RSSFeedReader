using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using RSSFeedReader.Api.Models;

namespace RSSFeedReader.Api.IntegrationTests;

public sealed class SubscriptionsApiTests
{
    [Fact]
    public async Task GetSubscriptions_WhenEmpty_ReturnsEmptyJsonArray()
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/subscriptions");

        response.EnsureSuccessStatusCode();
        var subscriptions = await response.Content.ReadFromJsonAsync<List<SubscriptionResponse>>();

        Assert.NotNull(subscriptions);
        Assert.Empty(subscriptions);
    }

    [Fact]
    public async Task PostThenGet_ReturnsAddedValuesInOrderIncludingDuplicates()
    {
        const string url = "not-required-to-be-a-valid-feed-url";
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var firstResponse = await client.PostAsJsonAsync("/api/subscriptions", new SubscriptionRequest(url));
        var secondResponse = await client.PostAsJsonAsync("/api/subscriptions", new SubscriptionRequest(url));
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.EnsureSuccessStatusCode();

        var subscriptions = await client.GetFromJsonAsync<List<SubscriptionResponse>>("/api/subscriptions");

        Assert.NotNull(subscriptions);
        Assert.Equal(new[] { url, url }, subscriptions.Select(subscription => subscription.Url));
    }

    [Theory]
    [InlineData("{}")]
    [InlineData("""{"url":""}""")]
    [InlineData("""{"url":"   "}""")]
    public async Task Post_WhenValueIsMissingOrWhitespace_ReturnsBadRequestWithoutAdding(string body)
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();
        using var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");

        var response = await client.PostAsync("/api/subscriptions", content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        using var error = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Contains(
            "required",
            error.RootElement.GetRawText(),
            StringComparison.OrdinalIgnoreCase);

        var subscriptions = await client.GetFromJsonAsync<List<SubscriptionResponse>>("/api/subscriptions");
        Assert.NotNull(subscriptions);
        Assert.Empty(subscriptions);
    }
}
