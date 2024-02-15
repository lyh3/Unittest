using Intel.MsoAuto.C3.MailService.Notification.Core;

namespace Intel.MsoAuto.C3.MailService.Notification.Entities {
    public class ARAssignedNotificationSentEmail : NotificationSentEmail, IComparable<ARAssignedNotificationSentEmail> {
        public ARAssignedNotificationSentEmail() {
            ThisComparer = new PropertyListComparable<ARAssignedNotificationSentEmail>(this,
                          x => x.ArId, x => x.ArStatus, x => x.AR);
        }
        public string ArId { get; set; } = string.Empty;
        public string? ArStatus { get; set; } = null;
        public string? AR { get; set; } = null;
        public string? ExpectedCompletionDate { get; set; } = null;
        public string? Comments { get; set; } = null;
        private PropertyListComparable<ARAssignedNotificationSentEmail> ThisComparer { get; }
        public int CompareTo(ARAssignedNotificationSentEmail? other)
        {
            if (other != null
                && Comparer.CompareTo(other) == 0 
                && ThisComparer.CompareTo(other) == 0)
                return 0;
            return -1;
        }
    }
}
