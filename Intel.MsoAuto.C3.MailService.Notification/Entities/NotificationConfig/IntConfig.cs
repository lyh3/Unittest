using Newtonsoft.Json;

namespace Intel.MsoAuto.C3.MailService.Notification.Core {
    public class IntConfig : EnvironmentConfig, IComparable<IntConfig> {
        public IntConfig() : base()
        {
            C3CommonConnectionString = Constants.DEFAULT_C3COMMON_CONNECTION_STRING_INT;
            Host1 = Constants.DEFAULT_HOST_1_INT;
            Host2 = Constants.DEFAULT_HOST_2_INT;
            Port = Constants.DEFAULT_PORT_INT;
            AmqUserName = Constants.DEFAULT_AMQ_USER_NAME_INT;
            CertificateThumbprint = Constants.DEFAULT_CERTIFICATE_THUMBPRINT_INT;
        }
        public int CompareTo(IntConfig? other)
        {
            return Comparer.CompareTo(other);
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
