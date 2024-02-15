namespace Intel.MsoAuto.C3.MailService.Notification.Entities {
    public class ProgressionNotificationSentEmails : List<ProgressionNotificationSentEmail> {
        public ProgressionNotificationSentEmails() { }
        public ProgressionNotificationSentEmails(IEnumerable<ProgressionNotificationSentEmail> sentEmails)
        {
            this.AddRange(sentEmails);
        }
        public bool Lookup(ProgressionNotificationSentEmail? sentEmail, bool? hasSent = null)
        {
            bool found = false;
            if (sentEmail != null)
            {
                foreach (ProgressionNotificationSentEmail email in this)
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
