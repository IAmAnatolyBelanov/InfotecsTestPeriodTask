namespace Monitoring.Domain.Mappers
{
    public partial class DeviceEventMapper : Monitoring.Domain.Mappers.IDeviceEventMapper
    {
        public Monitoring.Contracts.DeviceEvents.DeviceEventDto MapToDto(Monitoring.Dal.Models.DeviceEvent p1)
        {
            return p1 == null ? null : new Monitoring.Contracts.DeviceEvents.DeviceEventDto()
            {
                DeviceId = p1.DeviceId,
                Name = p1.Name,
                Date = p1.Date.ToUniversalTime()
            };
        }
        public Monitoring.Contracts.DeviceEvents.DeviceEventDto MapToDto(Monitoring.Dal.Models.DeviceEvent p2, Monitoring.Contracts.DeviceEvents.DeviceEventDto p3)
        {
            if (p2 == null)
            {
                return null;
            }
            Monitoring.Contracts.DeviceEvents.DeviceEventDto result = p3 ?? new Monitoring.Contracts.DeviceEvents.DeviceEventDto();
            
            result.DeviceId = p2.DeviceId;
            result.Name = p2.Name;
            result.Date = p2.Date.ToUniversalTime();
            return result;
            
        }
        public Monitoring.Dal.Models.DeviceEvent MapFromDto(Monitoring.Contracts.DeviceEvents.DeviceEventDto p4)
        {
            return p4 == null ? null : new Monitoring.Dal.Models.DeviceEvent()
            {
                DeviceId = p4.DeviceId,
                Name = p4.Name,
                Date = p4.Date
            };
        }
        public Monitoring.Dal.Models.DeviceEvent MapFromDto(Monitoring.Contracts.DeviceEvents.DeviceEventDto p5, Monitoring.Dal.Models.DeviceEvent p6)
        {
            if (p5 == null)
            {
                return null;
            }
            Monitoring.Dal.Models.DeviceEvent result = p6 ?? new Monitoring.Dal.Models.DeviceEvent();
            
            result.DeviceId = p5.DeviceId;
            result.Name = p5.Name;
            result.Date = p5.Date;
            return result;
            
        }
    }
}