using RazorEngine;
using SendGrid;
using SendGrid.Transport;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Generic.Infra.Utils
{
    public class EmailUtils
    {
        private static readonly string username;
        private static readonly string password;
        private static readonly string from;
        private static readonly NetworkCredential networkCredential;

        public static String ResetPasswordText { get; private set; } = @"Olá {0}, sua senha foi resetada. 
                                                                         Sua nova senha temporária é {1}.
                                                                         ";
        public static String ResetPasswordSubject { get; private set; } = @"Sua senha foi Resetada";


        static EmailUtils()
        {
            //username = ConfigurationManager.AppSettings["SendGridUserName"].ToString();
            //password = ConfigurationManager.AppSettings["SendGridPassword"].ToString();
            //from = ConfigurationManager.AppSettings["SendGridFrom"].ToString();

            networkCredential = new NetworkCredential(username, password);

        }

        public static void SendEmail(string to, string body, string subject)
        {
            try
            {
                SendGridMessage message = new SendGridMessage();

                message.AddTo(to);
                message.Html = body;
                message.Subject = subject;
                message.From = new MailAddress(from, "Generic");

                var transportWeb = new SendGrid.Web(networkCredential);

                transportWeb.Deliver(message);
            }
            catch
            {
                
            }
        }
    }
}
