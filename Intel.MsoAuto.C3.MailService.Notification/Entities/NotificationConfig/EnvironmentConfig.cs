using Newtonsoft.Json;

namespace Intel.MsoAuto.C3.MailService.Notification.Core {
    public class EnvironmentConfig : IComparable<EnvironmentConfig>, IEnvironmentConfig {
        public EnvironmentConfig()
        {
            Comparer = new PropertyListComparable<EnvironmentConfig>(this,
                            x => x.C3CommonConnectionString, x => x.LinkWebsiteUrl, x => x.QueueTopicName,
                            x => x.Host1, x => x.Host2, x => x.Port,
                            x => x.AmqUserName, x => x.AppId, x => x.SafeName,
                            x => x.CertificateThumbprint);
        }
        public string C3CommonConnectionString { get; set; } = string.Empty;
        public string LinkWebsiteUrl { get; set; } = Constants.DEFAULT_LINK_WEB_URL;
        public string QueueTopicName { get; set; } = Constants.DEFAULT_NOTIFICATION_TOPIC_NAME;
        public string Host1 { get; set; } = string.Empty;
        public string Host2 { get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;
        public string AmqUserName { get; set; } = Constants.DEFAULT_AMQ_USER_NAME_DEV;
        public string AppId { get; set; } = Constants.DEFAULT_APP_ID;
        public string SafeName { get; set; } = Constants.DEFAULT_SAFE_NAME;
        public string CertificateThumbprint { get; set; } = string.Empty;
        protected PropertyListComparable<EnvironmentConfig> Comparer { get; }
        public int CompareTo(EnvironmentConfig? other) => Comparer.CompareTo(other);
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
