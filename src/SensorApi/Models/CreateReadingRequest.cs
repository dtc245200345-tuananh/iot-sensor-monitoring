namespace SensorApi.Models;

public class CreateReadingRequest
{
    public double? Value { get; set; }

    public DateTimeOffset? MeasuredAtUtc { get; set; }
}