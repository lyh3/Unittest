using Newtonsoft.Json;

namespace Intel.MsoAuto.C3.MailService.Notification.Core {
    public class DevConfig : EnvironmentConfig, IComparable<DevConfig> {
        public DevConfig() : base()
        {
            C3CommonConnectionString = Constants.DEFAULT_C3COMMON_CONNECTION_STRING_DEV;
            Host1 = Constants.DEFAULT_HOST_1_DEV;
            Host2 = Constants.DEFAULT_HOST_2_DEV;
            Port = Constants.DEFAULT_PORT_DEV;
            AmqUserName = Constants.DEFAULT_AMQ_USER_NAME_DEV;
            CertificateThumbprint = Constants.DEFAULT_CERTIFICATE_THUMBPRINT_DEV;
        }
        public int CompareTo(DevConfig? other)
        {
            return Comparer.CompareTo(other);
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
