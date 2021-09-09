using System.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Twilio;

namespace Posy.V2.Infra.CrossCutting.Identity.Configuration
{
    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            if (ConfigurationManager.AppSettings["TwilioSend"] != "true")
                return Task.FromResult(0);

            /**
            * Utilizando TWILIO como SMS Provider.
            * https://www.twilio.com/docs/quickstart/csharp/sms/sending-via-rest
            **/

            const string accountSid = "";
            const string authToken = "";

            var client = new TwilioRestClient(accountSid, authToken);

            //client.SendMessage("+12566662708", message.Destination, message.Body);
            client.SendMessage("+19842071310", "+55" + message.Destination, message.Body);

            return Task.FromResult(0);
        }
    }
}