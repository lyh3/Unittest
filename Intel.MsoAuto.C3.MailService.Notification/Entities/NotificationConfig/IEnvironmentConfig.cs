namespace Intel.MsoAuto.C3.MailService.Notification.Core {
    public interface IEnvironmentConfig {
        string AmqUserName { get; set; }
        string AppId { get; set; }
        string C3CommonConnectionString { get; set; }
        string CertificateThumbprint { get; set; }
        string Host1 { get; set; }
        string Host2 { get; set; }
        string LinkWebsiteUrl { get; set; }
        string Port { get; set; }
        string QueueTopicName { get; set; }
        string SafeName { get; set; }

        int CompareTo(EnvironmentConfig? other);
    }
}