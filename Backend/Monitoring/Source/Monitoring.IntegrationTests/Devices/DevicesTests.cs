using System.Net.Http.Json;
using Monitoring.Api.Infrastructure;
using Monitoring.Contracts.DeviceInfo;
using Monitoring.Dal.Models;

namespace Monitoring.IntegrationTests.Devices;

/// <summary>
/// Тесты для проверки работы с <see cref="DeviceInfo"/>.
/// </summary>
public class DevicesTests : IClassFixture<AppFactory>
{
    private readonly AppFactory _factory;

    /// <summary>
    /// Конструктор класса <see cref="DevicesTests"/>.
    /// </summary>
    /// <param name="appFactory"><see cref="AppFactory"/>.</param>
    public DevicesTests(AppFactory appFactory) => _factory = appFactory;

    /// <summary>
    /// Должен успешно зарегистрировать девайс.
    /// </summary>
    /// <returns><see cref="Task"/>.</returns>
    [Fact]
    public async Task RegisterDevice_PossibleDevice_Success()
    {
        var device = new DeviceInfoDto
        {
            Id = Guid.Parse("00000000-0000-0000-0001-000000000001"),
            AppVersion = "00000000-0000-0000-0001-000000000001",
            OperationSystemInfo = "Windows 9",
            OperationSystemType = Shared.OperationSystem.OperationSystemType.Windows,
            UserName = "00000000-0000-0000-0001-000000000001",
        };

        var client = _factory.CreateClient();

        var (httpResponse, result) = await RegisterDevice(client, device);

        Assert.True(httpResponse.IsSuccessStatusCode);

        Assert.Null(result.Data);
        Assert.Null(result.Error);
    }

    /// <summary>
    /// Должен успешно получить информацию о зарегистрированном девайсе.
    /// </summary>
    /// <returns><see cref="Task"/>.</returns>
    [Fact]
    public async Task GetDevice_RegisteredDevice_Success()
    {
        var device = new DeviceInfoDto
        {
            Id = Guid.Parse("00000000-0000-0000-0002-000000000001"),
            AppVersion = "00000000-0000-0000-0002-000000000001",
            OperationSystemInfo = "Windows 9",
            OperationSystemType = Shared.OperationSystem.OperationSystemType.Windows,
            UserName = "00000000-0000-0000-0002-000000000001",
            LastUpdate = DateTimeOffset.UtcNow,
        };

        var client = _factory.CreateClient();

        await RegisterDevice(client, device);

        var result = await GetDevice(client, device.Id);

        Assert.Null(result.Error);
        Assert.NotNull(result.Data);

        Assert.Equal(device.Id, result.Data!.Id);
        Assert.Equal(device.AppVersion, result.Data.AppVersion);
        Assert.Equal(device.OperationSystemInfo, result.Data.OperationSystemInfo);
        Assert.Equal(device.OperationSystemType, result.Data.OperationSystemType);
        Assert.Equal(device.UserName, result.Data.UserName);
        Assert.Equal(device.LastUpdate.ToUnixTimeSeconds(), result.Data.LastUpdate.ToUnixTimeSeconds());
    }

    /// <summary>
    /// Должен успешно обновить информацию о зарегистрированном девайсе.
    /// </summary>
    /// <returns><see cref="Task"/>.</returns>
    [Fact]
    public async Task RegisterDevice_UpdateExistingDevice_Success()
    {
        var deviceId = Guid.Parse("00000000-0000-0000-0003-000000000001");

        var firstVariantDevice = new DeviceInfoDto
        {
            Id = deviceId,
            AppVersion = "00000000-0000-0000-0003-000000000002",
            OperationSystemInfo = "Windows 9",
            OperationSystemType = Shared.OperationSystem.OperationSystemType.Windows,
            UserName = "00000000-0000-0000-0003-000000000002",
            LastUpdate = DateTimeOffset.UtcNow,
        };

        var secondVariantDevice = new DeviceInfoDto
        {
            Id = deviceId,
            AppVersion = "00000000-0000-0000-0003-000000000003",
            OperationSystemInfo = "MacOs Home Edition",
            OperationSystemType = Shared.OperationSystem.OperationSystemType.MacOs,
            UserName = "00000000-0000-0000-0003-000000000003",
            LastUpdate = DateTimeOffset.UtcNow,
        };

        var client = _factory.CreateClient();

        await RegisterDevice(client, firstVariantDevice);
        var firstResult = await GetDevice(client, firstVariantDevice.Id);

        await RegisterDevice(client, secondVariantDevice);
        var secondResult = await GetDevice(client, secondVariantDevice.Id);

        Assert.Null(firstResult.Error);
        Assert.NotNull(firstResult.Data);
        Assert.Null(secondResult.Error);
        Assert.NotNull(secondResult.Data);

        Assert.Equal(firstVariantDevice.Id, firstResult.Data!.Id);
        Assert.Equal(firstVariantDevice.AppVersion, firstResult.Data.AppVersion);
        Assert.Equal(firstVariantDevice.OperationSystemInfo, firstResult.Data.OperationSystemInfo);
        Assert.Equal(firstVariantDevice.OperationSystemType, firstResult.Data.OperationSystemType);
        Assert.Equal(firstVariantDevice.UserName, firstResult.Data.UserName);
        Assert.Equal(firstVariantDevice.LastUpdate.ToUnixTimeSeconds(), firstResult.Data.LastUpdate.ToUnixTimeSeconds());

        Assert.Equal(secondVariantDevice.Id, secondResult.Data!.Id);
        Assert.Equal(secondVariantDevice.AppVersion, secondResult.Data.AppVersion);
        Assert.Equal(secondVariantDevice.OperationSystemInfo, secondResult.Data.OperationSystemInfo);
        Assert.Equal(secondVariantDevice.OperationSystemType, secondResult.Data.OperationSystemType);
        Assert.Equal(secondVariantDevice.UserName, secondResult.Data.UserName);
        Assert.Equal(secondVariantDevice.LastUpdate.ToUnixTimeSeconds(), secondResult.Data.LastUpdate.ToUnixTimeSeconds());
    }

    /// <summary>
    /// Должен получить пустой ответ при запросе не зарегистрированного девайса.
    /// </summary>
    /// <returns><see cref="Task"/>.</returns>
    [Fact]
    public async Task GetDevice_NotRegistered_Success()
    {
        var deviceId = Guid.Parse("00000000-0000-0000-0004-000000000001");

        var client = _factory.CreateClient();

        var result = await GetDevice(client, deviceId);

        Assert.Null(result.Error);
        Assert.Null(result.Data);
    }

    private async Task<(HttpResponseMessage HttpResponse, BaseResponse<object> BaseResponse)> RegisterDevice(HttpClient client, DeviceInfoDto device)
    {
        using var responseMessage = await client.PostAsJsonAsync("/api/devices/register-device", device);
        var responseContent = await responseMessage.Content.ReadAsStringAsync();

        var response = Newtonsoft.Json.JsonConvert.DeserializeObject<BaseResponse<object>>(responseContent)!;

        return (responseMessage, response);
    }

    private async Task<BaseResponse<DeviceInfoDto>> GetDevice(HttpClient client, Guid deviceId)
    {
        using var responseMessage = await client.GetAsync($"/api/devices/{deviceId}");
        var responseContent = await responseMessage.Content.ReadAsStringAsync();

        var response = Newtonsoft.Json.JsonConvert.DeserializeObject<BaseResponse<DeviceInfoDto>>(responseContent)!;

        return response;
    }
}
