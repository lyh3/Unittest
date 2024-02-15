namespace Intel.MsoAuto.C3.MailService.Notification.Entities {
    public class ARAssignedNotificationSentEmails : List<ARAssignedNotificationSentEmail> {
        public ARAssignedNotificationSentEmails() { }
        public ARAssignedNotificationSentEmails(IEnumerable<ARAssignedNotificationSentEmail> sentEmails)
        {
            this.AddRange(sentEmails);
        }
        public bool Lookup(ARAssignedNotificationSentEmail? sentEmail, bool? hasSent = null)
        {
            bool found = false;
            if (sentEmail != null)
            {
                foreach (ARAssignedNotificationSentEmail email in this)
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
