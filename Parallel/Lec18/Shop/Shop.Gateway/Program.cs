using System.Threading.RateLimiting;

// API-шлюз: єдина точка входу для клієнтів.
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddServiceDiscoveryDestinationResolver(); // http://orders

// Обмеження частоти: 10 запитів за секунду з однієї IP-адреси.
builder.Services.AddRateLimiter(o =>
{
    o.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    o.AddPolicy("per-ip", ctx => RateLimitPartition
        .GetFixedWindowLimiter(
        ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown",
        _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 10,
            Window = TimeSpan.FromSeconds(1),
        }));
});

WebApplication app = builder.Build();
app.MapDefaultEndpoints();
app.UseRateLimiter();
app.MapReverseProxy();
app.Run();
