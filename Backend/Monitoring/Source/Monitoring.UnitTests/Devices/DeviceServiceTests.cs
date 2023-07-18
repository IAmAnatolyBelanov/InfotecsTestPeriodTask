using AutoFixture;
using AutoFixture.AutoMoq;
using Monitoring.Dal.Models;
using Monitoring.Dal.Repositories;
using Monitoring.Dal.Sessions;
using Monitoring.Domain.DeviceServices;
using Moq;

namespace Monitoring.UnitTests.Devices;

/// <summary>
/// Тесты для проверки работы с <see cref="DeviceService"/>.
/// </summary>
public class DeviceServiceTests
{
    /// <summary>
    /// Должен получить пустую статистику по не зарегистрированному девайсу.
    /// </summary>
    /// <returns><see cref="Task"/>.</returns>
    [Fact]
    public async Task GetStatistics_NotRegisteredDevice_ReturnsEmptyResult()
    {
        var fixture = new Fixture();
        fixture.Customize(new AutoMoqCustomization());

        var id = fixture.Create<Guid>();

        var deviceRepository = new Mock<IDeviceRepository>();
        deviceRepository.Setup(x => x.GetDevice(It.IsAny<ISession>(), id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(DeviceInfo));
        fixture.Inject(deviceRepository.Object);

        var deviceService = fixture.Create<DeviceService>();

        var result = await deviceService.GetStatistics(id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(id, result.DeviceId);
        Assert.Null(result.LastUpdate);
    }

    /// <summary>
    /// Должен успешно получить статистику по девайсу.
    /// </summary>
    /// <returns><see cref="Task"/>.</returns>
    [Fact]
    public async Task GetStatistics_Correct_Success()
    {
        var fixture = new Fixture();
        fixture.Customize(new AutoMoqCustomization());

        var device = fixture.Create<DeviceInfo>();

        var deviceRepository = new Mock<IDeviceRepository>();
        deviceRepository.Setup(x => x.GetDevice(It.IsAny<ISession>(), device.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(device);
        fixture.Inject(deviceRepository.Object);

        var deviceService = fixture.Create<DeviceService>();

        var result = await deviceService.GetStatistics(device.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(device.Id, result.DeviceId);
        Assert.Equal(device.LastUpdate, result.LastUpdate);
    }
}
