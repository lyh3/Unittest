using Intel.MsoAuto.C3.PITT.Business.Models;

namespace Intel.MsoAuto.C3.MailService.Notification.Entities {
    public class ARAssignedOverdueNotificationDatum : ARAssignedNotificationDatum {
        public ARAssignedOverdueNotificationDatum() { }
        public ARAssignedOverdueNotificationDatum(NotificationArDetail source) : base(source) {}
    }
}
