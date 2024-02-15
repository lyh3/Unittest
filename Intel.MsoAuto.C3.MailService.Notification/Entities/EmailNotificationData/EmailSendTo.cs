namespace Intel.MsoAuto.C3.MailService.Notification.Entities {
    public class EmailSendTo<U> {
        public string UserName { get; set; } = string.Empty;
        public string SendTo { get; set; } = string.Empty;
        public string CcTo { get; set; } = string.Empty;
        public string Idsid { get; set; } = string.Empty;
        public bool HasSent { get; set; } = false;
        public NotificationData<U> Content { get; set; } = new NotificationData<U>();
    }
}