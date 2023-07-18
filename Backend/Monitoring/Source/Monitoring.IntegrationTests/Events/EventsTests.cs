using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Mapster;
using Microsoft.AspNetCore.Http;
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
        var fixture = new Fixture();
        var deviceEvent = fixture.Create<DeviceEventDto>();

        var client = _factory.CreateClient();

        var result = await AddEvent(client, deviceEvent);

        using (new AssertionScope())
        {
            result.Error.Should().NotBeNullOrWhiteSpace();
            result.Data.Should().BeNull();
        }
    }

    /// <summary>
    /// Должен успешно добавить информацию о событии.
    /// </summary>
    /// <returns><see cref="Task"/>.</returns>
    [Fact]
    public async Task AddEvent_PossibleEventForRegisteredDevice_Success()
    {
        var fixture = new Fixture();

        var device = fixture.Create<DeviceInfoDto>();

        var client = _factory.CreateClient();

        await RegisterDevice(client, device);

        var deviceEvent = fixture.Create<DeviceEventDto>();
        deviceEvent.DeviceId = device.Id;
        
        var result = await AddEvent(client, deviceEvent);

        using (new AssertionScope())
        {
            result.Error.Should().BeNull();
            result.Data.Should().NotBeNull();
            result.Data.Value.Should().NotBeEmpty();
        }
    }

    /// <summary>
    /// Должен успешно получить список событий по девайсу.
    /// </summary>
    /// <returns><see cref="Task"/>.</returns>
    [Fact]
    public async Task GetEventsByDevice_RegisteredDeviceAndAddedEvents_Success()
    {
        var fixture = new Fixture();

        var device = fixture.Create<DeviceInfoDto>();

        var client = _factory.CreateClient();

        await RegisterDevice(client, device);

        var events = new List<DeviceEventDto>
        {
            fixture.Create<DeviceEventDto>(),
            fixture.Create<DeviceEventDto>(),
            fixture.Create<DeviceEventDto>(),
        };
        events.ForEach(x => x.DeviceId = device.Id);

        await AddEvents(client, events);

        var result = await GetEventsByDevice(client, device.Id);

        using (new AssertionScope())
        {
            result.Error.Should().BeNull();
            result.Data.Should().NotBeNull();
        }

        using (new AssertionScope())
        {
            result.Data.DeviceId.Should().Be(device.Id);
            result.Data.Events.Should().HaveSameCount(events);
            foreach (var @event in result.Data.Events)
            {
                events.Should().ContainEquivalentOf(@event, options => options.Excluding(x => x.DateTime))
                    .And.Contain(x => @event.DateTime.ToUnixTimeSeconds() == x.DateTime.ToUnixTimeSeconds());
            }
        }
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
