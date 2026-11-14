using Microsoft.AspNetCore.Server.Kestrel.Core;
using Orders.Server;

Console.OutputEncoding = System.Text.Encoding.UTF8;
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// HTTP/2 без TLS лише для локальної розробки.
builder.WebHost.ConfigureKestrel(kestrel =>
    kestrel.ListenLocalhost(5001,
        listen => listen.Protocols = HttpProtocols.Http2));

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

WebApplication app = builder.Build();
app.MapGrpcService<OrderApi>();
app.MapGrpcReflectionService();      // для grpcurl і Rider
Console.WriteLine("gRPC-сервер: http://localhost:5001");
app.Run();
