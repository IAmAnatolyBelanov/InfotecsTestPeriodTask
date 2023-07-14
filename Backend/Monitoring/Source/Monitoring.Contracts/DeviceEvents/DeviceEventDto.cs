using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.Contracts.DeviceEvents;
public class DeviceEventDto
{
    public Guid DeviceId { get; set; }

    public string Name { get; set; }

    public DateTimeOffset Date { get; set; }
}
