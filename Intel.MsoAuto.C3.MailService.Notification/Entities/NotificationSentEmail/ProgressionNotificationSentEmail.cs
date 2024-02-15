namespace Intel.MsoAuto.C3.MailService.Notification.Entities {
    public class ProgressionNotificationSentEmail : NotificationSentEmail, IComparable<ProgressionNotificationSentEmail> {
        public ProgressionNotificationSentEmail() { }
        public string StateId { get; set; } = string.Empty;
        public string ProgressionStatus { get; set; } = string.Empty;
        public int CompareTo(ProgressionNotificationSentEmail? other)
        {
            if (other != null 
                && Comparer.CompareTo(other) == 0
                && StateId.Equals(other.StateId))
                return 0;
            return -1;
        }
    }
}
