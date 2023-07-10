namespace Infotecs.Monitoring.Domain.Mappers
{
    public partial class DeviceInfoMapper : Infotecs.Monitoring.Domain.Mappers.IDeviceInfoMapper
    {
        public Infotecs.Monitoring.Contracts.DeviceInfo.DeviceInfoDto MapToDto(Infotecs.Monitoring.Dal.Models.DeviceInfo p1)
        {
            return p1 == null ? null : new Infotecs.Monitoring.Contracts.DeviceInfo.DeviceInfoDto()
            {
                Id = p1.Id,
                UserName = p1.UserName,
                OperationSystemType = p1.OperationSystemType,
                OperationSystemInfo = p1.OperationSystemInfo,
                AppVersion = p1.AppVersion,
                LastUpdate = p1.LastUpdate
            };
        }
        public Infotecs.Monitoring.Contracts.DeviceInfo.DeviceInfoDto MapToDto(Infotecs.Monitoring.Dal.Models.DeviceInfo p2, Infotecs.Monitoring.Contracts.DeviceInfo.DeviceInfoDto p3)
        {
            if (p2 == null)
            {
                return null;
            }
            Infotecs.Monitoring.Contracts.DeviceInfo.DeviceInfoDto result = p3 ?? new Infotecs.Monitoring.Contracts.DeviceInfo.DeviceInfoDto();
            
            result.Id = p2.Id;
            result.UserName = p2.UserName;
            result.OperationSystemType = p2.OperationSystemType;
            result.OperationSystemInfo = p2.OperationSystemInfo;
            result.AppVersion = p2.AppVersion;
            result.LastUpdate = p2.LastUpdate;
            return result;
            
        }
        public Infotecs.Monitoring.Dal.Models.DeviceInfo MapFromDto(Infotecs.Monitoring.Contracts.DeviceInfo.DeviceInfoDto p4)
        {
            return p4 == null ? null : new Infotecs.Monitoring.Dal.Models.DeviceInfo()
            {
                Id = p4.Id,
                UserName = p4.UserName,
                OperationSystemType = p4.OperationSystemType,
                OperationSystemInfo = p4.OperationSystemInfo,
                AppVersion = p4.AppVersion,
                LastUpdate = p4.LastUpdate
            };
        }
        public Infotecs.Monitoring.Dal.Models.DeviceInfo MapFromDto(Infotecs.Monitoring.Contracts.DeviceInfo.DeviceInfoDto p5, Infotecs.Monitoring.Dal.Models.DeviceInfo p6)
        {
            if (p5 == null)
            {
                return null;
            }
            Infotecs.Monitoring.Dal.Models.DeviceInfo result = p6 ?? new Infotecs.Monitoring.Dal.Models.DeviceInfo();
            
            result.Id = p5.Id;
            result.UserName = p5.UserName;
            result.OperationSystemType = p5.OperationSystemType;
            result.OperationSystemInfo = p5.OperationSystemInfo;
            result.AppVersion = p5.AppVersion;
            result.LastUpdate = p5.LastUpdate;
            return result;
            
        }
    }
}