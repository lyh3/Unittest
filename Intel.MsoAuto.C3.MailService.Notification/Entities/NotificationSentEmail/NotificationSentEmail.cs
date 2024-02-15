using Intel.MsoAuto.C3.MailService.Notification.Core;

namespace Intel.MsoAuto.C3.MailService.Notification.Entities {
    public class NotificationSentEmail : IComparable<NotificationSentEmail> {
        public NotificationSentEmail() {
            Comparer = new PropertyListComparable<NotificationSentEmail>(this,
                        x => x.ProjectId, x => x.EmailTemplate, 
                         x => x.SentTo);
        }
        public string ProjectId { get; set; } = string.Empty;
        public string EmailTemplate { get; set; } = string.Empty;
        public string MessageQueueId { get; set; } = string.Empty;
        public string RequestJson { get; set; } = string.Empty;
        //public string? ProgressionStatus { get; set; } = null;
        public string SentTo { get; set; } = string.Empty;
        public bool? HasSent { get; set; } = false;
        protected PropertyListComparable<NotificationSentEmail> Comparer { get; }
        public int CompareTo(NotificationSentEmail? other)
        {
            int ret = -1;
            if (other != null && Comparer.CompareTo(other) == 0)
            {
                if (HasSent != null && other.HasSent != null)
                    ret = Convert.ToByte(HasSent) ^ Convert.ToByte(other.HasSent);
            }
            return ret;
        } 
    }
}
