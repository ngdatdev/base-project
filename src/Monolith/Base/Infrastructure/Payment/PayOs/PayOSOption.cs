using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Payment.PayOs;

/// summary
/// The DatabaseOption class is used to hold various payos configuration settings.
/// summary
public class PayOSOption
{
    public string ClientId { get; set; }
    public string ApiKey { get; set; }
    public string ChecksumKey { get; set; }
}
