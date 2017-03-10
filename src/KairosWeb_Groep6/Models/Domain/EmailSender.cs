using MailKit.Net.Smtp;
using MimeKit;

namespace KairosWeb_Groep6.Models.Domain
{
    public static class EmailSender
    {
        public static void SendRegisterMailWithPassword(string name, string email, string password)
        {
            var message = CreateBaseMessage();
            message.To.Add(new MailboxAddress(name, email));
            message.Subject = "Welkom bij Kairos!";

            var builder = new BodyBuilder();
            builder.HtmlBody = CreateRegisterMail(name, email, password);

            message.Body = builder.ToMessageBody();

            SendMail(message);
        }

        public static void SendForgotPasswordMail(string name, string email, string password)
        {
            var message = CreateBaseMessage();
            message.To.Add(new MailboxAddress(name, email));
            message.Subject = "Welkom bij Kairos!";

            var builder = new BodyBuilder();
            builder.HtmlBody = CreateForgotPasswordMail(name, password);

            message.Body = builder.ToMessageBody();

            SendMail(message);
        }

        private static MimeMessage CreateBaseMessage()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Kairos", "kairos.opportunit@gmail.com"));

            return message;
        }

        private static void SendMail(MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 465, true);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("kairos.opportunit@gmail.com", "kairos2017");

                client.Send(message);
                client.Disconnect(true);
            }
        }

        private static string CreateForgotPasswordMail(string name, string password)
        {
            return
            @"<!doctype html>
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
            </head>

            <body>
                <img src='https://static.wixstatic.com/media/192f9b_a49f1a3533c149a2a803ee4ab519ef2e~mv2.png/v1/crop/x_2,y_0,w_1257,h_515/fill/w_800,h_328,al_c,usm_0.66_1.00_0.01/192f9b_a49f1a3533c149a2a803ee4ab519ef2e~mv2.png' alt='Logo Kairos' width='50%' style='margin: 0 auto; display: block;' />
                <h1>Ben je je wachtwoord vergeten?</h1>"

            + string.Format(@"<h3>Hallo, {0}!</h3>", name)
            +
                @"<p>Je hebt gevraagd om je wachtwoord te resetten. Hieronder vind je een tijdelijk wachtwoord waar je maar één maal kan aanmelden.</p>
                   
                  <p><strong>Je doorloopt hierdoor opnieuw de procedure van een eerste keer aanmelden!</strong> 
                    Uiteraard ben je géén analyses of andere gegevens kwijt!</p>"

            + string.Format(
                @"<p>Je tijdelijk wachtwoord is <strong>{0}</strong>.</p>", password)
            +
                @"<p>Bij vragen of problemen, mag je steeds mailen naar <a href='mailto:bart@werkgeversbenadering.be'>bart@werkgeversbenadering.be</a></p>
                <br>

                <h3 class='no-margin'>Veel plezier met Kairos!</h3>
                <p class='no-margin'>Het Kairos-team</p>
            </body>

            </html>";
        }

        private static string CreateRegisterMail(string name, string email, string password)
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
            </head>

            <body>
                <img src='https://static.wixstatic.com/media/192f9b_a49f1a3533c149a2a803ee4ab519ef2e~mv2.png/v1/crop/x_2,y_0,w_1257,h_515/fill/w_800,h_328,al_c,usm_0.66_1.00_0.01/192f9b_a49f1a3533c149a2a803ee4ab519ef2e~mv2.png' alt='Logo Kairos' width='50%' style='margin: 0 auto; display: block;' />
                <h1>Welkom bij Kairos!</h1>
                <h2><span style='color:#97C900'>nu</span> is het moment voor iedereen</h2>"

            + string.Format(@"<h3>Hallo, {0}!</h3>", name)
            +
                @"<p>We zijn blij je te mogen verwelkomen in onze gloednieuwe webapplicatie! Hieronder vind je je wachtwoord waarmee je een eerste keer kan aanmelden.</p>
                <p>Na de eerste keer aanmelden wordt je een nieuw wachtwoord gevraagd, het onderstaande wachtwoord is dus slechts eenmalig geldig.</p>"

            + string.Format(
                @"<p>Je registreerde met <strong>{0}</strong> als email. Hiermee dien je steeds in te loggen.</p>
                <p>Je tijdelijk wachtwoord is <strong>{1}</strong>.</p>", email, password)
            +
                @"<p>Bij vragen of problemen, mag je steeds mailen naar <a href='mailto:bart@werkgeversbenadering.be'>bart@werkgeversbenadering.be</a></p>
                <br>

                <h3 class='no-margin'>Veel plezier met Kairos!</h3>
                <p class='no-margin'>Het Kairos-team</p>
            </body>

            </html>";
        }
    }
}
