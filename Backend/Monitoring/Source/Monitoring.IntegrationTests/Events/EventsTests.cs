using System.Net.Http.Json;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using FluentAssertions.Execution;
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
    /// <param name="deviceEvent">Событие, добавление которого должно оборваться с ошибкой.</param>
    /// <returns><see cref="Task"/>.</returns>
    [Theory]
    [AutoData]
    public async Task AddEvent_WithoutRegisteredDevice_ReturnsError(DeviceEventDto deviceEvent)
    {
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
    /// <param name="device">Девайс, для которого будет добавлено событие.</param>
    /// <param name="deviceEvent">Событие.</param>
    /// <returns><see cref="Task"/>.</returns>
    [Theory]
    [AutoData]
    public async Task AddEvent_PossibleEventForRegisteredDevice_Success(DeviceInfoDto device, DeviceEventDto deviceEvent)
    {
        deviceEvent.DeviceId = device.Id;

        var client = _factory.CreateClient();

        await RegisterDevice(client, device);

        var result = await AddEvent(client, deviceEvent);

        using (new AssertionScope())
        {
            result.Error.Should().BeNull();
            result.Data.Should().NotBeNull();
            result.Data!.Value.Should().NotBeEmpty();
        }
    }

    /// <summary>
    /// Должен успешно получить список событий по девайсу.
    /// </summary>
    /// <param name="device">Девайс, который будет зарегистрирован.</param>
    /// <param name="eventsGenerator"><see cref="Generator{T}"/> для <see cref="DeviceEventDto"/>.</param>
    /// <returns><see cref="Task"/>.</returns>
    [Theory]
    [AutoData]
    public async Task GetEventsByDevice_RegisteredDeviceAndAddedEvents_Success(DeviceInfoDto device, Generator<DeviceEventDto> eventsGenerator)
    {
        var client = _factory.CreateClient();

        await RegisterDevice(client, device);

        var events = eventsGenerator.Take(10).ToList();
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
            result.Data!.DeviceId.Should().Be(device.Id);
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
    /// <param name="deviceId">Id девайса.</param>
    /// <returns><see cref="Task"/>.</returns>
    [Theory]
    [AutoData]
    public async Task GetEventsByDevice_WithoutRegisteredDevice_ReturnsEmptyCollectionOfEvents(Guid deviceId)
    {
        var client = _factory.CreateClient();

        var result = await GetEventsByDevice(client, deviceId);

        using (new AssertionScope())
        {
            result.Error.Should().BeNull();
            result.Data.Should().NotBeNull();
        }

        using (new AssertionScope())
        {
            result.Data!.DeviceId.Should().Be(deviceId);
            result.Data.Events.Should().BeEmpty();
        }
    }

    /// <summary>
    /// При попытке добавить события о девайсе, который ещё не зарегистрирован в системе, должен вернуть ошибку.
    /// </summary>
    /// <param name="eventsGenerator"><see cref="Generator{T}"/> для <see cref="DeviceEventDto"/>.</param>
    /// <returns><see cref="Task"/>.</returns>
    [Theory]
    [AutoData]
    public async Task AddEvents_WithoutRegisteredDevice_ReturnsError(Generator<DeviceEventDto> eventsGenerator)
    {
        var fixture = new Fixture();

        var deviceEvents = eventsGenerator.Take(10).ToArray();

        var client = _factory.CreateClient();

        var result = await AddEvents(client, deviceEvents);

        using (new AssertionScope())
        {
            result.Error.Should().NotBeNull();
            result.Data.Should().BeNull();
        }
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
