using System.Net.Http.Json;
using Monitoring.Api.Infrastructure;
using Monitoring.Contracts.DeviceEvents;
using Monitoring.Contracts.DeviceInfo;
using Monitoring.Dal.Models;

namespace Monitoring.IntegrationTests.Events;

/// <summary>
/// Тесты для проверки работы с <see cref="DeviceEvent"/>.
/// </summary>
public class EventsTests : IClassFixture<AppFactory>
{
    private readonly AppFactory _factory;

    /// <summary>
    /// Конструктор класса <see cref="EventsTests"/>.
    /// </summary>
    /// <param name="factory"><see cref="AppFactory"/>.</param>
    public EventsTests(AppFactory factory) => _factory = factory;

    /// <summary>
    /// При попытке добавить событие о девайсе, который ещё не зарегистрирован в системе, должен вернуть ошибку.
    /// </summary>
    /// <returns><see cref="Task"/>.</returns>
    [Fact]
    public async Task AddEvent_WithoutRegisteredDevice_ReturnsError()
    {
        var deviceEvent = new DeviceEventDto
        {
            DateTime = new DateTimeOffset(2023, 07, 12, 17, 30, 0, TimeSpan.FromHours(3)),
            DeviceId = Guid.Parse("00000000-0000-0000-0001-000000000001"),
            Name = "00000000-0000-0000-0001-000000000001",
        };

        var client = _factory.CreateClient();

        var response = await AddEvent(client, deviceEvent);

        Assert.Null(response.Data);
        Assert.NotNull(response.Error);
    }

    /// <summary>
    /// Должен успешно добавить информацию о событии.
    /// </summary>
    /// <returns><see cref="Task"/>.</returns>
    [Fact]
    public async Task AddEvent_PossibleEventForRegisteredDevice_Success()
    {
        var device = new DeviceInfoDto
        {
            Id = Guid.Parse("00000000-0000-0000-0002-000000000001"),
            AppVersion = "00000000-0000-0000-0002-000000000001",
            OperationSystemInfo = "Windows 9",
            OperationSystemType = Shared.OperationSystem.OperationSystemType.Windows,
            UserName = "00000000-0000-0000-0002-000000000001",
        };

        var client = _factory.CreateClient();

        await RegisterDevice(client, device);

        var deviceEvent = new DeviceEventDto
        {
            DeviceId = device.Id,
            Name = "00000000-0000-0000-0002-000000000001",
            DateTime = new DateTimeOffset(2023, 07, 12, 18, 15, 0, TimeSpan.FromHours(3)),
        };

        var response = await AddEvent(client, deviceEvent);

        Assert.Null(response.Error);
        Assert.NotNull(response.Data);
        Assert.NotEqual(Guid.Empty, response.Data);
    }

    /// <summary>
    /// Должен успешно получить список событий по девайсу.
    /// </summary>
    /// <returns><see cref="Task"/>.</returns>
    [Fact]
    public async Task GetEventsByDevice_RegisteredDeviceAndAddedEvents_Success()
    {
        var device = new DeviceInfoDto
        {
            Id = Guid.Parse("00000000-0000-0000-0003-000000000001"),
            AppVersion = "00000000-0000-0000-0003-000000000001",
            OperationSystemInfo = "Windows 9",
            OperationSystemType = Shared.OperationSystem.OperationSystemType.Windows,
            UserName = "00000000-0000-0000-0003-000000000001",
        };

        var client = _factory.CreateClient();

        await RegisterDevice(client, device);

        var events = new DeviceEventDto[]
        {
            new DeviceEventDto
            {
                DeviceId = device.Id,
                Name = "00000000-0000-0000-0003-000000000002",
                DateTime = new DateTimeOffset(2023, 07, 13, 10, 10, 0, TimeSpan.FromHours(3)),
            },
            new DeviceEventDto
            {
                DeviceId = device.Id,
                Name = "00000000-0000-0000-0003-000000000003",
                DateTime = new DateTimeOffset(2023, 07, 13, 10, 10, 1, TimeSpan.FromHours(3)),
            },
            new DeviceEventDto
            {
                DeviceId = device.Id,
                Name = "00000000-0000-0000-0003-000000000004",
                DateTime = new DateTimeOffset(2023, 07, 13, 10, 10, 2, TimeSpan.FromHours(3)),
            }
        };

        await AddEvents(client, events);

        var result = await GetEventsByDevice(client, device.Id);

        Assert.Null(result.Error);
        Assert.NotNull(result.Data);
        Assert.Equal(device.Id, result.Data!.DeviceId);
        Assert.Equal(events.Length, result.Data.Events.Count);
        Assert.True(events.All(src => result.Data.Events.Any(dst => src.Name == dst.Name && src.DateTime == dst.DateTime)));
    }

    /// <summary>
    /// При попытке получить информацию о девайсе, который не зарегистрирован в системе, должен получить пустой список событий.
    /// </summary>
    /// <returns><see cref="Task"/>.</returns>
    [Fact]
    public async Task GetEventsByDevice_WithoutRegisteredDevice_ReturnsEmptyCollectionOfEvents()
    {
        var deviceId = Guid.Parse("00000000-0000-0000-0004-000000000001");

        var client = _factory.CreateClient();

        var result = await GetEventsByDevice(client, deviceId);

        Assert.Null(result.Error);
        Assert.NotNull(result.Data);
        Assert.Equal(deviceId, result.Data!.DeviceId);
        Assert.Equal(0, result.Data.Events.Count);
    }

    /// <summary>
    /// При попытке добавить события о девайсе, который ещё не зарегистрирован в системе, должен вернуть ошибку.
    /// </summary>
    /// <returns><see cref="Task"/>.</returns>
    [Fact]
    public async Task AddEvents_WithoutRegisteredDevice_ReturnsError()
    {
        var deviceEvents = new DeviceEventDto[]
        {
            new DeviceEventDto
            {
                DateTime = new DateTimeOffset(2023, 07, 12, 17, 30, 0, TimeSpan.FromHours(3)),
                DeviceId = Guid.Parse("00000000-0000-0000-0005-000000000001"),
                Name = "00000000-0000-0000-0005-000000000001",
            },
            new DeviceEventDto
            {
                DateTime = new DateTimeOffset(2023, 07, 12, 17, 30, 0, TimeSpan.FromHours(3)),
                DeviceId = Guid.Parse("00000000-0000-0000-0005-000000000002"),
                Name = "00000000-0000-0000-0005-000000000002",
            },
        };

        var client = _factory.CreateClient();

        var response = await AddEvents(client, deviceEvents);

        Assert.Null(response.Data);
        Assert.NotNull(response.Error);
    }

    /// <summary>
    /// Регистрирует девайс.
    /// </summary>
    /// <param name="client"><see cref="HttpClient"/>.</param>
    /// <param name="device">Девайс.</param>
    /// <returns><see cref="Task"/>.</returns>
    /// <remarks>Так как это класс тестов <see cref="DeviceEvent"/>, то предполагается, что регистрация девайсов уже протестирована и работает корректно.</remarks>
    private async Task RegisterDevice(HttpClient client, DeviceInfoDto device)
    {
        using var responseMessage = await client.PostAsJsonAsync("/api/devices/register-device", device);
        var responseContent = await responseMessage.Content.ReadAsStringAsync();

        var response = Newtonsoft.Json.JsonConvert.DeserializeObject<BaseResponse<object>>(responseContent)!;

        if (response.Error != null)
        {
            throw new ArgumentException("Can not register device. " + response.Error);
        }
    }

    private async Task<BaseResponse<Guid?>> AddEvent(HttpClient client, DeviceEventDto deviceEvent)
    {
        using var responseMessage = await client.PutAsJsonAsync("/api/events", deviceEvent);
        var responseContent = await responseMessage.Content.ReadAsStringAsync();

        var response = Newtonsoft.Json.JsonConvert.DeserializeObject<BaseResponse<Guid?>>(responseContent)!;

        return response;
    }

    private async Task<BaseResponse<object>> AddEvents(HttpClient client, IReadOnlyCollection<DeviceEventDto> deviceEvents)
    {
        using var responseMessage = await client.PutAsJsonAsync("/api/events/bulk", deviceEvents);
        var responseContent = await responseMessage.Content.ReadAsStringAsync();

        var response = Newtonsoft.Json.JsonConvert.DeserializeObject<BaseResponse<object>>(responseContent)!;

        return response;
    }

    private async Task<BaseResponse<EventCollectionDto>> GetEventsByDevice(HttpClient client, Guid deviceId)
    {
        using var responseMessage = await client.GetAsync($"/api/events/by-device?deviceId={deviceId}");
        var responseContent = await responseMessage.Content.ReadAsStringAsync();

        var response = Newtonsoft.Json.JsonConvert.DeserializeObject<BaseResponse<EventCollectionDto>>(responseContent)!;

        return response;
    }
}
