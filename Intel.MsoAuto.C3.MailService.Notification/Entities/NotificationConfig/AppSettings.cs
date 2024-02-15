using Newtonsoft.Json;

namespace Intel.MsoAuto.C3.MailService.Notification.Core {
    public class AppSettings : IAppSettings {
        public string SMTPSERVER { get; set; } = Constants.DEFAULT_SMTPSERVER;
        public string EmailRedirectTo { get; set; } = Constants.DEFAULT_REDIRECT_TO;
        public string EmailFrom { get; set; } = Constants.DEFAULT_EMAIL_FROM;
        public string ErrorNotifyTo { get; set; } = Constants.DEFAULT_ERROR_NOTIFY_TO;
        public string EmailTypeExclusive { get; set; } = string.Empty;
        public DevConfig Dev { get; set; } = new DevConfig();
        public IntConfig Int { get; set; } = new IntConfig();
        public ProdConfig Prod { get; set; } = new ProdConfig();
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
