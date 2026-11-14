HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
IConfiguration config = builder.Configuration;

string? host = config["Smtp:Host"];               // рядок або null
int port = config.GetValue<int>("Smtp:Port");     // перетворення типу
bool ssl = config.GetValue("Smtp:UseSsl", defaultValue: true);
IConfigurationSection smtp = config.GetSection("Smtp");
string password = smtp["Password"] ?? "(не задано)";

Console.WriteLine($"{builder.Environment.EnvironmentName}: " +
    $"{host}:{port}, SSL {ssl}, пароль {password}");
