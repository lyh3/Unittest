using Intel.MsoAuto.C3.MailService.Notification.Core;
using Intel.MsoAuto.Shared.Extensions;
using Microsoft.Extensions.Configuration;

namespace Intel.MsoAuto.C3.MailService.Notification.Entities {
    public class FailureNotificationDatum : NotificationDatum {
        public FailureNotificationDatum()
        {
            IConfiguration config = Settings.Configuration;
            Email = config.GetSection($"{Constants.CONFIGURATIONS}:{Constants.APP_SETTINGS}:{Constants.ERROR_NOTIFY_TO}").Value.ToStringSafely();
        }
        public string ErrorMessage { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
    }
}
