using Primes;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.AddServiceDefaults();   // OpenTelemetry, перевірки стану
builder.Services.AddSingleton<Backend>();
builder.Services.AddHostedService(
    sp => sp.GetRequiredService<Backend>());
builder.Services.AddHostedService<PrimeWorker>();
// Скільки чекати завершення поточної частини після SIGTERM.
builder.Services.Configure<HostOptions>(o =>
    o.ShutdownTimeout = TimeSpan.FromSeconds(20));
builder.Build().Run();
