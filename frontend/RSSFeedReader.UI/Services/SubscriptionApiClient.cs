using System.Net.Http.Json;
using RSSFeedReader.UI.Models;

namespace RSSFeedReader.UI.Services;

public sealed class SubscriptionApiClient(HttpClient httpClient)
{
    public async Task<IReadOnlyList<Subscription>> GetSubscriptionsAsync(CancellationToken cancellationToken = default)
    {
        var subscriptions = await httpClient.GetFromJsonAsync<List<Subscription>>(
            "api/subscriptions",
            cancellationToken);

        return subscriptions
            ?? throw new InvalidOperationException("The subscriptions API returned an empty response.");
    }

    public async Task<Subscription> AddSubscriptionAsync(
        string url,
        CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PostAsJsonAsync(
            "api/subscriptions",
            new AddSubscriptionRequest(url),
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken);
            throw new HttpRequestException(
                error?.Error ?? "The subscription could not be added.",
                inner: null,
                response.StatusCode);
        }

        return await response.Content.ReadFromJsonAsync<Subscription>(cancellationToken)
            ?? throw new InvalidOperationException("The subscriptions API returned an empty response.");
    }
}
