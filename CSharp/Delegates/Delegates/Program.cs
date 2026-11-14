Thermostat thermostat = new("Серверна", maxAllowed: 27);

thermostat.TemperatureChanged += Thermostat_TemperatureChanged;

void Thermostat_TemperatureChanged(
    object? sender, TemperatureChangedEventArgs e)
{
    throw new NotImplementedException();
}