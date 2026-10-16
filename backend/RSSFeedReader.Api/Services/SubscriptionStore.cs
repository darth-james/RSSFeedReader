using RSSFeedReader.Api.Models;

namespace RSSFeedReader.Api.Services;

public sealed class SubscriptionStore
{
    private readonly object _gate = new();
    private readonly List<SubscriptionResponse> _subscriptions = [];

    public IReadOnlyList<SubscriptionResponse> GetAll()
    {
        lock (_gate)
        {
            return _subscriptions.ToArray();
        }
    }

    public bool TryAdd(string? url, out SubscriptionResponse? subscription)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            subscription = null;
            return false;
        }

        var added = new SubscriptionResponse(url);
        lock (_gate)
        {
            _subscriptions.Add(added);
        }

        subscription = added;
        return true;
    }
}
