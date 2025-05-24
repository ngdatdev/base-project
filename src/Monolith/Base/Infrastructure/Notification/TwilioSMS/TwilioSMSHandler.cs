using System;
using System.Threading.Tasks;
using BaseApiReference.Abstractions.Notification.SMS;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Infrastructure.Notification.TwilioSMS;

/// <summary>
/// Twilio SMS Handler.
/// </summary>
internal class TwilioSMSHandler : ISMSHandler
{
    private readonly TwilioSMSOption _option;

    public TwilioSMSHandler(TwilioSMSOption option)
    {
        _option = option;
    }

    public async Task<bool> SendNotification(string to, string body)
    {
        var accountSid = _option.AccountSid;
        var authToken = _option.AuthToken;
        var isCreated = false;
        try
        {
            TwilioClient.Init(accountSid, authToken);
            var messageOptions = new CreateMessageOptions(new PhoneNumber(to));
            messageOptions.From = new PhoneNumber(_option.FromPhoneNumber);
            messageOptions.Body = body;
            var message = await MessageResource.CreateAsync(messageOptions);
            isCreated = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            isCreated = false;
        }

        return isCreated;
    }
}
