using Microsoft.AspNetCore.Http.HttpResults;
using RSSFeedReader.Api.Models;
using RSSFeedReader.Api.Services;

namespace RSSFeedReader.Api.Endpoints;

public static class SubscriptionEndpoints
{
    public static RouteGroupBuilder MapSubscriptionEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/subscriptions");
        group.MapGet("", GetSubscriptions);
        group.MapPost("", AddSubscription);
        return group;
    }

    private static Ok<IReadOnlyList<SubscriptionResponse>> GetSubscriptions(SubscriptionStore store) =>
        TypedResults.Ok(store.GetAll());

    private static Results<Ok<SubscriptionResponse>, BadRequest<ErrorResponse>> AddSubscription(
        SubscriptionRequest? request,
        SubscriptionStore store)
    {
        if (request is null || !store.TryAdd(request.Url, out var subscription))
        {
            return TypedResults.BadRequest(new ErrorResponse("A subscription URL is required."));
        }

        return TypedResults.Ok(subscription);
    }
}
