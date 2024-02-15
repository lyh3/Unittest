namespace Intel.MsoAuto.C3.MailService.Notification.Entities {
    public interface IEmailNotificationRequest<T> where T : class, new() {
        string LinkWebsiteUrl { get; set; }
        T NotificationData { get; set; }
    }
}