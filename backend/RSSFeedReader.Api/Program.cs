using RSSFeedReader.Api.Endpoints;
using RSSFeedReader.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<SubscriptionStore>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        var frontendOrigin = builder.Configuration["FrontendOrigin"]
            ?? throw new InvalidOperationException("The FrontendOrigin configuration value is required.");
        policy.WithOrigins(frontendOrigin)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors();
app.MapSubscriptionEndpoints();

app.Run();

public partial class Program { }
