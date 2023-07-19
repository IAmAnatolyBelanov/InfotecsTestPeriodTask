using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using FluentAssertions;
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
    /// <param name="id">Id несуществующего девайса.</param>
    /// <returns><see cref="Task"/>.</returns>
    [Theory]
    [AutoData]
    public async Task GetStatistics_NotRegisteredDevice_ReturnsEmptyResult(Guid id)
    {
        var fixture = new Fixture();
        fixture.Customize(new AutoMoqCustomization());

        var deviceRepository = new Mock<IDeviceRepository>();
        deviceRepository.Setup(x => x.GetDevice(It.IsAny<ISession>(), id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(DeviceInfo));
        fixture.Inject(deviceRepository.Object);

        var deviceService = fixture.Create<DeviceService>();

        var result = await deviceService.GetStatistics(id, CancellationToken.None);

        result.Should().NotBeNull();
        result.DeviceId.Should().Be(id);
        result.LastUpdate.Should().BeNull();
    }

    /// <summary>
    /// Должен успешно получить статистику по девайсу.
    /// </summary>
    /// <param name="device">Девайс.</param>
    /// <returns><see cref="Task"/>.</returns>
    [Theory]
    [AutoData]
    public async Task GetStatistics_Correct_Success(DeviceInfo device)
    {
        var fixture = new Fixture();
        fixture.Customize(new AutoMoqCustomization());

        var deviceRepository = new Mock<IDeviceRepository>();
        deviceRepository.Setup(x => x.GetDevice(It.IsAny<ISession>(), device.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(device);
        fixture.Inject(deviceRepository.Object);

        var deviceService = fixture.Create<DeviceService>();

        var result = await deviceService.GetStatistics(device.Id, CancellationToken.None);

        result.Should().NotBeNull();
        result.DeviceId.Should().Be(device.Id);
        result.LastUpdate.Should().Be(device.LastUpdate);
    }
}
