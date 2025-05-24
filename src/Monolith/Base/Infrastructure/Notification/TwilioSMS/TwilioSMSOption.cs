using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Notification.TwilioSMS;

/// <summary>
/// Twilio SMS Options
/// </summary>
public class TwilioSMSOption
{
    public string AccountSid { get; set; }
    public string AuthToken { get; set; }
    public string FromPhoneNumber { get; set; }
}
