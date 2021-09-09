using Microsoft.AspNet.Identity;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;

namespace Posy.V2.Infra.CrossCutting.Identity.Configuration
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            return ConfigSendGridasync(message);
            //return SendMailAsync(message);
        }

        /**
        * Implementação do SendGrid
        * Obtendo a apiKey https://app.sendgrid.com/settings/api_keys
        * API Key Created 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
        * c# https://github.com/sendgrid/sendgrid-csharp#installation
        * https://docs.microsoft.com/pt-br/aspnet/mvc/overview/security/create-an-aspnet-mvc-5-web-app-with-email-confirmation-and-password-reset
        **/
        private async Task ConfigSendGridasync(IdentityMessage message)
        {
            if (ConfigurationManager.AppSettings["mailSend"] != "true")
                return;

            /**
            * Recupere a chave da API das variáveis de ambiente. Veja o projeto README para mais informações sobre como configurar isso.
            **/
            var apiKey = "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";// Environment.GetEnvironmentVariable("SENDGRID_API_KEY");

            var client = new SendGridClient(apiKey);

            /**
            * Enviar um único email usando o Mail Helper
            **/
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(ConfigurationManager.AppSettings["mailAccount"], ConfigurationManager.AppSettings["mailName"]),
                Subject = message.Subject,
                PlainTextContent = message.Body,
                HtmlContent = message.Body
            };
            msg.AddTo(new EmailAddress(message.Destination));

            var response = await client.SendEmailAsync(msg);

            Console.WriteLine(msg.Serialize());
            Console.WriteLine(response.StatusCode);
            Console.WriteLine(response.Headers);
        }

        /**
        * Implementação de e-mail manual
        **/
        private Task SendMailAsync(IdentityMessage message)
        {
            if (ConfigurationManager.AppSettings["Internet"] == "true")
            {
                try
                {
                    var text = HttpUtility.HtmlEncode(message.Body);

                    var msg = new MailMessage();
                    msg.From = new MailAddress(ConfigurationManager.AppSettings["mailAccount"], ConfigurationManager.AppSettings["mailName"]);
                    msg.To.Add(new MailAddress(message.Destination));
                    msg.Subject = message.Subject;
                    msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
                    msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Html));

                    var smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32(587));
                    var credentials = new NetworkCredential(ConfigurationManager.AppSettings["mailAccount"],
                        ConfigurationManager.AppSettings["mailPassword"]);
                    smtpClient.Credentials = credentials;
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(msg);
                }
                catch (Exception)
                {

                }
            }

            return Task.FromResult(0);
        }
    }
}