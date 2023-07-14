using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.Contracts.DeviceEvents;

public class EventCollection
{
    public Guid DeviceId { get; set; }

    public IReadOnlyList<DeviceEventDtoLight> Events { get; set; }
}
