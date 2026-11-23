namespace SensorApi.Models;

public class Sensor
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public string? Location { get; set; }

    public double AlertThreshold { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }
}