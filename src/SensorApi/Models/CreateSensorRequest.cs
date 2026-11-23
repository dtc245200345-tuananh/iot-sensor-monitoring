namespace SensorApi.Models;

public class CreateSensorRequest
{
    public string? Name { get; set; }

    public string? Type { get; set; }

    public string? Location { get; set; }

    public double AlertThreshold { get; set; }
}