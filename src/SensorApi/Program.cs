using SensorApi.Models;
using SensorApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký dịch vụ lưu trữ cảm biến
builder.Services.AddSingleton<SensorStore>();

var app = builder.Build();

// API kiểm tra trạng thái
app.MapGet("/", () =>
{
    return Results.Ok(new
    {
        service = "SensorApi",
        status = "running"
    });
});

// API đăng ký cảm biến
app.MapPost("/sensors", (CreateSensorRequest request, SensorStore store) =>
{
    // Kiểm tra tên cảm biến
    if (string.IsNullOrWhiteSpace(request.Name))
    {
        return Results.BadRequest(new
        {
            message = "Sensor name is required."
        });
    }

    // Kiểm tra loại cảm biến
    if (string.IsNullOrWhiteSpace(request.Type))
    {
        return Results.BadRequest(new
        {
            message = "Sensor type is required."
        });
    }

    // Kiểm tra ngưỡng cảnh báo
    if (!double.IsFinite(request.AlertThreshold))
    {
        return Results.BadRequest(new
        {
            message = "Alert threshold must be a finite number."
        });
    }

    var sensor = store.Create(request);

    return Results.Created($"/sensors/{sensor.Id}", sensor);
});

// API lấy danh sách cảm biến
app.MapGet("/sensors", (SensorStore store) =>
{
    var sensors = store.GetAll();

    return Results.Ok(sensors);
});

// API tiếp nhận dữ liệu đo từ cảm biến
app.MapPost(
    "/sensors/{id:guid}/readings",
    (Guid id, CreateReadingRequest request, SensorStore store) =>
{
    // Kiểm tra cảm biến có tồn tại không
    var sensor = store.GetById(id);

    if (sensor is null)
    {
        return Results.NotFound(new
        {
            message = "Sensor not found."
        });
    }

    // Kiểm tra dữ liệu đo
    if (request.Value is null || !double.IsFinite(request.Value.Value))
    {
        return Results.BadRequest(new
        {
            message = "A valid reading value is required."
        });
    }

    // Tạo và lưu bản ghi đo
    var reading = store.CreateReading(id, request);

    return Results.Created(
        $"/sensors/{id}/readings",
        reading
    );
});

app.Run();