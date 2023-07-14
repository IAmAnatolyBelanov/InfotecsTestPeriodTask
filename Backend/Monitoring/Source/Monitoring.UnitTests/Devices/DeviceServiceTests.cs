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
    private readonly Mock<ISessionFactory> _sessionFactory;

    /// <summary>
    /// Конструктор класса <see cref="DeviceServiceTests"/>.
    /// </summary>
    public DeviceServiceTests()
    {
        _sessionFactory = new Mock<ISessionFactory>();
        _sessionFactory.Setup(x => x.CreateSession(It.IsAny<bool>()))
            .Returns(new Mock<ISession>().Object);
    }

    /// <summary>
    /// Должен получить пустую статистику по не зарегистрированному девайсу.
    /// </summary>
    /// <returns><see cref="Task"/>.</returns>
    [Fact]
    public async Task GetStatistics_NotRegisteredDevice_ReturnsEmptyResult()
    {
        var deviceRepository = new Mock<IDeviceRepository>();
        deviceRepository.Setup(x => x.GetDevice(It.IsAny<ISession>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(DeviceInfo));

        var deviceService = new DeviceService(_sessionFactory.Object, deviceRepository.Object);

        var id = Guid.Parse("00000000-0000-0000-0001-000000000001");
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
        var id = Guid.Parse("00000000-0000-0000-0002-000000000001");
        var device = new DeviceInfo
        {
            Id = id,
            AppVersion = "00000000-0000-0000-0002-000000000002",
            LastUpdate = new DateTimeOffset(2023, 07, 12, 11, 10, 0, TimeSpan.FromHours(3)),
            OperationSystemInfo = "00000000-0000-0000-0002-000000000002",
            OperationSystemType = Shared.OperationSystem.OperationSystemType.Android,
            UserName = "00000000-0000-0000-0002-000000000002",
        };

        var deviceRepository = new Mock<IDeviceRepository>();
        deviceRepository.Setup(x => x.GetDevice(It.IsAny<ISession>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(device);

        var deviceService = new DeviceService(_sessionFactory.Object, deviceRepository.Object);

        var result = await deviceService.GetStatistics(id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(id, result.DeviceId);
        Assert.Equal(device.LastUpdate, result.LastUpdate);
    }
}
