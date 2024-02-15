namespace Intel.MsoAuto.C3.MailService.Notification.Entities {
    public class EmailNotificationRequest<T> : IEmailNotificationRequest<T> where T : class, new() {
        public EmailNotificationRequest() { }
        public EmailNotificationRequest(string linkWebsiteUrl, T notificationData) 
        {
            LinkWebsiteUrl = linkWebsiteUrl;
            NotificationData = notificationData;
        }
        public string LinkWebsiteUrl { get; set; } = string.Empty;
        public T NotificationData { get; set; } = null;
    }
}
