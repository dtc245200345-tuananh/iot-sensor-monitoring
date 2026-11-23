namespace SensorApi.Models;

public class SensorReading
{
    public Guid Id { get; set; }

    public Guid SensorId { get; set; }

    public double Value { get; set; }

    public DateTimeOffset MeasuredAtUtc { get; set; }
}