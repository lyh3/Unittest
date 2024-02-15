using Intel.MsoAuto.C3.MailService.Notification.Core;

namespace Intel.MsoAuto.C3.MailService.Notification.Entities {
    public class BottleneckNotificationSentEmail : NotificationSentEmail, IComparable<BottleneckNotificationSentEmail> {
        public BottleneckNotificationSentEmail() {
            thisComparer = new PropertyListComparable<BottleneckNotificationSentEmail>(this,
                           x => x.SiteName, x => x.Week, x => x.Year,
                           x => x.CreatedBy, x => x.UpdatedBy);
        }
        public string SiteName { get; set; } = string.Empty;
        public int? Week { get; set; } = null;
        public int? Year { get; set; } = null;
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;

        public int CompareTo(BottleneckNotificationSentEmail? other)
        {
            if (other != null
                && Comparer.CompareTo(other) == 0
                && thisComparer.CompareTo(other) == 0)
                return 0;
            return -1;
        }
        private PropertyListComparable<BottleneckNotificationSentEmail> thisComparer { get; } = null;
    }
}
