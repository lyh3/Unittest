namespace Intel.MsoAuto.C3.MailService.Notification.Entities {
    public class BottleneckNotificationSentEmails : List<BottleneckNotificationSentEmail> {
        public BottleneckNotificationSentEmails() { }
        public BottleneckNotificationSentEmails(IEnumerable<BottleneckNotificationSentEmail> sentEmails)
        {
            this.AddRange(sentEmails);
        }
        public bool Lookup(BottleneckNotificationSentEmail? sentEmail, bool? hasSent = null)
        {
            bool found = false;
            if (sentEmail != null)
            {
                foreach (BottleneckNotificationSentEmail email in this)
                {
                    found = email.CompareTo(sentEmail) == 0;
                    if (hasSent == null)
                        found &= email.HasSent.HasValue && email.HasSent.Value;
                    if (found)
                        break;
                }
            }
            return found;
        }

    }
}
