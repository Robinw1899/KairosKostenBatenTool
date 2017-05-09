using System.IO;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace KairosWeb_Groep6.Models.Domain
{
    public static class EmailSender
    {
        public static async Task<bool> SendRegisterMailWithPassword(string name, string email, string password)
        {
            var message = CreateBaseMessage();
            message.Bcc.Add(new MailboxAddress(name, email));
            message.Subject = @"Welkom bij KAIROS' kosten-baten tool!";

            var builder = new BodyBuilder();
            builder.HtmlBody = CreateRegisterMail(name, email, password);

            message.Body = builder.ToMessageBody();
            
            bool gelukt = await SendMail(message);
            return gelukt;
        }

        public static async Task<bool> SendForgotPasswordMail(string name, string email, string password, string url)
        {
            var message = CreateBaseMessage();
            message.Bcc.Add(new MailboxAddress(name, email));
            message.Subject = "Paswoord KAIROS kosten-baten tool vergeten";

            var builder = new BodyBuilder();
            builder.HtmlBody = CreateForgotPasswordMail(name, password, url);

            message.Body = builder.ToMessageBody();

            bool gelukt = await SendMail(message);
            return gelukt;
        }

        public static async Task<bool> SendMailAdmin(string nameJobcoach, string emailJobcoach, string subject, string body)
        {
            var message = CreateBaseMessage();
            //message.To.Add(new MailboxAddress("Bart Moens", "bart@werkgeversbenadering.be"));
            message.Bcc.Add(new MailboxAddress("Bart Moens", "thomasaelbrecht@live.com"));
            message.Subject = "Melding Kairos: " + subject;

            // instellen dat Bart Moens kan antwoorden op deze mail om te mailen met de jobcoach:
            message.ReplyTo.Clear();
            message.ReplyTo.Add(new MailboxAddress(nameJobcoach, emailJobcoach));

            var builder = new BodyBuilder();
            builder.HtmlBody = CreateMailAdmin(nameJobcoach, emailJobcoach, body);

            message.Body = builder.ToMessageBody();

            bool gelukt = await SendMail(message);
            return gelukt;
        }

        public static async Task<bool> SendResultaat(string naam, string email, string subject, string body, FileInfo file)
        {
            var message = CreateBaseMessage();
            
            message.Bcc.Add(new MailboxAddress(naam, email));
            message.Subject = subject;

            var builder = new BodyBuilder {TextBody = body};
            builder.Attachments.Add(file.FullName);
            message.Body = builder.ToMessageBody();

            bool gelukt = await SendMail(message);
            return gelukt;
        }

        private static MimeMessage CreateBaseMessage()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Kairos", "kairos.opportunit@gmail.com"));

            return message;
        }

        private static async Task<bool> SendMail(MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync("smtp.gmail.com", 465, true);

                    // Note: since we don't have an OAuth2 token, disable
                    // the XOAUTH2 authentication mechanism.
                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    // Note: only needed if the SMTP server requires authentication
                    await client.AuthenticateAsync("kairos.opportunit@gmail.com", "kairos2017");

                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }

        private static string CreateForgotPasswordMail(string name, string password, string url)
        {
            return CreateHtmlHead() + 
            @"s
            <body>"
            + string.Format(@"<p>Beste  {0}</p>", name)
            +
                @"<p>Je hebt aangegeven dat je jouw paswoord vergeten bent.</p>
                   <p>Het onderstaande nieuwe paswoord kan je gebruiken om in te loggen. Nadat je bent ingelogd, kan je jouw pas paswoord wijzigen.</p>"

            + string.Format(
                @"<p><a href='{0}'>Je kan hier je wachtwoord wijzigen.</a></p>", url)
            +
                @"
                    <p class='no-margin'>Hartelijke groet</p>
                    <p class='no-margin'>Het team van KAIROS</p>
            </body>

            </html>";
        }

        private static string CreateRegisterMail(string name, string email, string password)
        {
            return CreateHtmlHead() +
            @"
            <body>"
            + string.Format(@"<p>Beste  {0}</p>", name)
            +
                @"<p>Leuk dat je gebruik wil maken van onze tool om werkgevers meer inzicht te geven in de kosten en baten bij het tewerkstellen van personen met een grote afstand tot de arbeidsmarkt.</p>
                <p>Je kan nu inloggen op <a href='http://localhost:25242'>localhost:25242</a> met deze gebruikersnaam en paswoord:</p>"

            + string.Format(
                @"<p>Gebruikersnaam: {0}</p>
                <p>Paswoord: {1}</p>", email, password)
            +
            @"<p>Na het inloggen kan je je paswoord veranderen.<br />
                    Veel succes met het gebruik van onze tool!</p>"
            +
                @"<p>Wil je meer weten over wie we zijn en wat we doen, surf naar <a href='www.hetmomentvooriedereen.be'>www.hetmomentvooriedereen.be</a>.</p>
                <br>

                <p class='no-margin'>Hartelijke groet</p>
                <p class='no-margin'>Het team van KAIROS</p>
            </body>

            </html>";
        }

        private static string CreateMailAdmin(string nameJobcoach, string emailJobcoach, string message)
        {
            return CreateHtmlHead() +
            @"
            <body>
                <img src='https://static.wixstatic.com/media/192f9b_a49f1a3533c149a2a803ee4ab519ef2e~mv2.png/v1/crop/x_2,y_0,w_1257,h_515/fill/w_800,h_328,al_c,usm_0.66_1.00_0.01/192f9b_a49f1a3533c149a2a803ee4ab519ef2e~mv2.png' alt='Logo Kairos' width='50%' style='margin: 0 auto; display: block;' />
                <h3>Beste Bart Moens</h3>"
+ string.Format(@"
                <p>{0} had een vraag/opmerking rond Kairos:</p>
                <br/>

                <hr />
                <p>{1}</p>", nameJobcoach, message)
+ string.Format(@"
                <hr />
                <br/>

                <p>
                    <a href='mailto:{0}'>Klik hier om de jobcoach te contacteren.</a>", emailJobcoach)
+
               @" Of beantwoordt simpel deze mail!</p>
                <br>

                <h3 class='no-margin'>Groeten</h3>
                <p class='no-margin'>Kairos</p>
            </body>

            </html>";
        }

        private static string CreateHtmlHead()
        {
            return @"<!doctype html>
            <html>

            <head>
                <meta charset='UTF-8'>
                <title>Welkom bij Kairos!</title>
                <link href='https://fonts.googleapis.com/css?family=Asap' rel='stylesheet'>
                <style type='text/css'>
                    body {
                        font-family: Asap, Arial, sans-serif;
                        font-size: 1em;
                    }
        
                    h1 {
                        margin-bottom: 5px;
                    }
        
                    strong {
                        color: #97C900;
                    }
        
                    a,
                    a:hover,
                    a:active,
                    a:visited {
                        color: #97C900;
                    }
        
                    .no-margin {
                        margin: 0;
                    }

                </style>
            </head>";
        }
    }
}
