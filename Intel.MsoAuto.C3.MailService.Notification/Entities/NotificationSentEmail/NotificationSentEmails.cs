namespace Intel.MsoAuto.C3.MailService.Notification.Entities {
    public class NotificationSentEmails : List<NotificationSentEmail> {
        public NotificationSentEmails() { }
        public NotificationSentEmails(IEnumerable<NotificationSentEmail> sentEmails) 
        { 
            this.AddRange(sentEmails);
        }
        public bool Lookup(NotificationSentEmail? sentEmail, bool? hasSent = null)
        {
            bool found = false;
            if (sentEmail != null)
            {
                foreach (NotificationSentEmail email in this) 
                {
                    found = email.CompareTo(sentEmail) == 1;
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
