using Newtonsoft.Json;
namespace Intel.MsoAuto.C3.MailService.Notification.Core {
    public class ProdConfig : EnvironmentConfig , IComparable<ProdConfig>{
        public ProdConfig():base()
        {
            C3CommonConnectionString = Constants.DEFAULT_C3COMMON_CONNECTION_STRING_PROD;
            Host1 = Constants.DEFAULT_HOST_1_PROD;
            Host2 = Constants.DEFAULT_HOST_2_PROD;
            Port = Constants.DEFAULT_PORT_PROD;
            AmqUserName = Constants.DEFAULT_AMQ_USER_NAME_PROD;
            CertificateThumbprint = Constants.DEFAULT_CERTIFICATE_THUMBPRINT_PROD;
        }
        public int CompareTo(ProdConfig? other)
        {
            return Comparer.CompareTo(other);
        }        
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
