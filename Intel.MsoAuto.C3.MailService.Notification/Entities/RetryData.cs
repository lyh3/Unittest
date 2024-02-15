namespace Intel.MsoAuto.C3.MailService.Notification.Entities {
    public class RetryData 
    {
        public string NotificationType { get; set; } = string.Empty;
        public string Request { get; set; } = string.Empty;
        public int RetryCount { get; set; } = 3;
    }
}
