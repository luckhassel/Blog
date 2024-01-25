﻿namespace Blog.Application.Settings
{
    public class ApplicationSettings
    {
        public SecuritySettings SecuritySettings { get; set; }
        public MassTransitSettings MassTransitSettings { get; set; }
        public EmailSettings EmailSettings { get; set; }
    }

    public class SecuritySettings
    {
        public string Secret { get; set; }
    }


    public class MassTransitSettings
    {
        public string NewsQueue { get; set; }
        public string Server { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        
    }

    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string SenderPassword { get; set; }
    }
}
