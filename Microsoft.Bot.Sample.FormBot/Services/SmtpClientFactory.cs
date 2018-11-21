﻿using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FormBot.Services
{
    class SmtpClientFactory
    {
        public static ISmtpClient GetClient(ClientType type, NetworkCredential credentials, bool withSsl, string host = "", int port = 25)
        {
            ISmtpClient client;

            if (type == ClientType.Gmail)
            {
                client = new GmailClient(credentials, withSsl);
            }
            else if (type == ClientType.Office365)
            {
                client = new Office365Client(credentials, withSsl);
            }
            else if (type == ClientType.Outlook)
            {
                client = new OutlookClient(credentials, withSsl);
            }
            else if (type == ClientType.Yahoo)
            {
                client = new YahooClient(credentials, withSsl);
            }
            else if (type == ClientType.SendGrid)
            {
                client = new SendGridClient(credentials, withSsl);
            }
            else
            {
                // Custom
                if (host == "")
                {
                    throw new Exception("Host name is required for a custom SMTP client.");
                }
                client = new CustomClient(host, port, withSsl, credentials);
            }
            return client;
        }

        public enum ClientType { Gmail, Outlook, Yahoo, Office365, SendGrid, Custom }
    }

    interface ISmtpClient : IDisposable
    {
        string Host { get; }
        int Port { get; set; }
        bool EnforceSsl { get; set; }
        NetworkCredential Credentials { get; set; }
        SmtpClient Client { get; set; }

        Task Send(MailMessage m);
    }

    public class GmailClient : ISmtpClient
    {
        public GmailClient()
        {
            Host = "smtp.gmail.com";
        }

        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnforceSsl { get; set; }
        public NetworkCredential Credentials { get; set; }
        public SmtpClient Client { get; set; }

        public GmailClient(NetworkCredential credentials, bool ssl)
        {
            Credentials = credentials;
            Port = 25;
            EnforceSsl = ssl;
            Host = "smtp.gmail.com";

            if (ssl)
            {
                Port = 587;
            }

            Client = new SmtpClient(Host, Port);
        }

        async Task ISmtpClient.Send(MailMessage message)
        {
            Client.EnableSsl = EnforceSsl;
            Client.Credentials = Credentials;

            // Send
            await Client.SendMailAsync(message);
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }

    public class OutlookClient : ISmtpClient
    {
        public OutlookClient()
        {
            Host = "smtp-mail.outlook.com";
        }

        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnforceSsl { get; set; }
        public NetworkCredential Credentials { get; set; }
        public SmtpClient Client { get; set; }

        public OutlookClient(NetworkCredential credentials, bool ssl)
        {
            Credentials = credentials; 
            Port = 25;
            EnforceSsl = ssl;
            Host = "smtp-mail.outlook.com";

            if (ssl)
            {
                Port = 587;
            }

            Client = new SmtpClient(Host, Port);
        }

        async Task ISmtpClient.Send(MailMessage message)
        {
            Client.EnableSsl = EnforceSsl;
            Client.Credentials = Credentials;

            // Send
            await Client.SendMailAsync(message);
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }

    public class YahooClient : ISmtpClient
    {
        public YahooClient()
        {
            Host = "smtp.mail.yahoo.com";
        }

        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnforceSsl { get; set; }
        public NetworkCredential Credentials { get; set; }
        public SmtpClient Client { get; set; }

        public YahooClient(NetworkCredential credentials, bool ssl)
        {
            Credentials = credentials;
            Port = 25;
            EnforceSsl = ssl;
            Host = "smtp.mail.yahoo.com";

            if (ssl)
            {
                Port = 587;
            }

            Client = new SmtpClient(Host, Port);
        }

        async Task ISmtpClient.Send(MailMessage message)
        {
            Client.EnableSsl = EnforceSsl;
            Client.Credentials = Credentials;

            // Send
            await Client.SendMailAsync(message);
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }

    public class Office365Client : ISmtpClient
    {
        public Office365Client()
        {
            Host = "smtp.office365.com";
        }

        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnforceSsl { get; set; }
        public NetworkCredential Credentials { get; set; }
        public SmtpClient Client { get; set; }

        public Office365Client(NetworkCredential credentials, bool ssl)
        {
            Credentials = credentials; 
            Port = 25;
            EnforceSsl = ssl;
            Host = "smtp.office365.com";

            if (ssl)
            {
                Port = 587;
            }

            Client = new SmtpClient(Host, Port);
        }

        async Task ISmtpClient.Send(MailMessage message)
        {
            Client.EnableSsl = EnforceSsl;
            Client.Credentials = Credentials;

            // Send
            await Client.SendMailAsync(message);
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }

    public class SendGridClient : ISmtpClient
    {
        public SendGridClient()
        {
            Host = "smtp.sendgrid.net";
        }

        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnforceSsl { get; set; }
        public NetworkCredential Credentials { get; set; }
        public SmtpClient Client { get; set; }

        public SendGridClient(NetworkCredential credentials, bool ssl) 
        {
            Credentials = credentials;
            Port = 25;
            EnforceSsl = ssl;
            Host = "smtp.sendgrid.net";

            if (ssl)
            {
                Port = 587;
            }

            Client = new SmtpClient(Host, Port);
        }

        async Task ISmtpClient.Send(MailMessage message)
        {
            Client.EnableSsl = EnforceSsl;
            Client.Credentials = Credentials;

            // Send
            await Client.SendMailAsync(message);
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }

    public class CustomClient : ISmtpClient
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnforceSsl { get; set; }
        public NetworkCredential Credentials { get; set; }
        public SmtpClient Client { get; set; }

        public CustomClient(string host, int port, bool ssl, NetworkCredential credentials)
        {
            // In the custom mode, we can set the host ourself.
            Host = host;
            Port = port;
            Credentials = credentials;
            EnforceSsl = ssl;

            Client = new SmtpClient(Host, Port);
        }

        async Task ISmtpClient.Send(MailMessage message)
        {
            Client.EnableSsl = EnforceSsl;
            Client.Credentials = Credentials;

            // Send
            await Client.SendMailAsync(message);
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }
}
