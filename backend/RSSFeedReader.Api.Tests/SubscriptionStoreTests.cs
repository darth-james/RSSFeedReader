using RSSFeedReader.Api.Models;
using RSSFeedReader.Api.Services;

namespace RSSFeedReader.Api.Tests;

public sealed class SubscriptionStoreTests
{
    [Fact]
    public void GetAll_WhenStoreIsCreated_ReturnsEmptyList()
    {
        var store = new SubscriptionStore();

        Assert.Empty(store.GetAll());
    }

    [Fact]
    public void TryAdd_PreservesInsertionOrderAndRepeatedValues()
    {
        var store = new SubscriptionStore();

        Assert.True(store.TryAdd("first", out _));
        Assert.True(store.TryAdd("second", out _));
        Assert.True(store.TryAdd("first", out _));

        Assert.Equal(
            new[] { "first", "second", "first" },
            store.GetAll().Select(subscription => subscription.Url));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t\r\n")]
    public void TryAdd_WhenValueIsEmptyOrWhitespace_ReturnsFalseAndDoesNotChangeList(string url)
    {
        var store = new SubscriptionStore();

        var added = store.TryAdd(url, out var subscription);

        Assert.False(added);
        Assert.Null(subscription);
        Assert.Empty(store.GetAll());
    }
}
