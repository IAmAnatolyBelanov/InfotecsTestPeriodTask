namespace Monitoring.Domain.Mappers
{
    public partial class DeviceEventLightMapper : Monitoring.Domain.Mappers.IDeviceEventLightMapper
    {
        public Monitoring.Contracts.Dtos.DeviceEvents.DeviceEventLightDto MapToDto(Monitoring.Dal.Models.DeviceEvent p1)
        {
            return p1 == null ? null : new Monitoring.Contracts.Dtos.DeviceEvents.DeviceEventLightDto()
            {
                Name = p1.Name,
                DateTime = p1.DateTime
            };
        }
        public Monitoring.Contracts.Dtos.DeviceEvents.DeviceEventLightDto MapToDto(Monitoring.Dal.Models.DeviceEvent p2, Monitoring.Contracts.Dtos.DeviceEvents.DeviceEventLightDto p3)
        {
            if (p2 == null)
            {
                return null;
            }
            Monitoring.Contracts.Dtos.DeviceEvents.DeviceEventLightDto result = p3 ?? new Monitoring.Contracts.Dtos.DeviceEvents.DeviceEventLightDto();
            
            result.Name = p2.Name;
            result.DateTime = p2.DateTime;
            return result;
            
        }
        public Monitoring.Dal.Models.DeviceEvent MapFromDto(Monitoring.Contracts.Dtos.DeviceEvents.DeviceEventLightDto p4)
        {
            return p4 == null ? null : new Monitoring.Dal.Models.DeviceEvent()
            {
                Name = p4.Name,
                DateTime = p4.DateTime.ToUniversalTime()
            };
        }
        public Monitoring.Dal.Models.DeviceEvent MapFromDto(Monitoring.Contracts.Dtos.DeviceEvents.DeviceEventLightDto p5, Monitoring.Dal.Models.DeviceEvent p6)
        {
            if (p5 == null)
            {
                return null;
            }
            Monitoring.Dal.Models.DeviceEvent result = p6 ?? new Monitoring.Dal.Models.DeviceEvent();
            
            result.Name = p5.Name;
            result.DateTime = p5.DateTime.ToUniversalTime();
            return result;
            
        }
    }
}
