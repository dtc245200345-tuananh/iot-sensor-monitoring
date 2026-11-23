using System.Collections.Concurrent;
using SensorApi.Models;

namespace SensorApi.Services;

public class SensorStore
{
    private readonly ConcurrentDictionary<Guid, Sensor> _sensors = new();

    private readonly ConcurrentDictionary<Guid, ConcurrentQueue<SensorReading>>
        _readings = new();

    public Sensor Create(CreateSensorRequest request)
    {
        var sensor = new Sensor
        {
            Id = Guid.NewGuid(),
            Name = request.Name!.Trim(),
            Type = request.Type!.Trim(),
            Location = request.Location?.Trim(),
            AlertThreshold = request.AlertThreshold,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };

        _sensors.TryAdd(sensor.Id, sensor);

        return sensor;
    }

    public IReadOnlyCollection<Sensor> GetAll()
    {
        return _sensors.Values.ToArray();
    }

    public Sensor? GetById(Guid id)
    {
        _sensors.TryGetValue(id, out var sensor);

        return sensor;
    }

    public SensorReading CreateReading(
        Guid sensorId,
        CreateReadingRequest request)
    {
        var reading = new SensorReading
        {
            Id = Guid.NewGuid(),
            SensorId = sensorId,
            Value = request.Value!.Value,
            MeasuredAtUtc = request.MeasuredAtUtc?.ToUniversalTime()
                            ?? DateTimeOffset.UtcNow
        };

        var queue = _readings.GetOrAdd(
            sensorId,
            _ => new ConcurrentQueue<SensorReading>()
        );

        queue.Enqueue(reading);

        return reading;
    }
}