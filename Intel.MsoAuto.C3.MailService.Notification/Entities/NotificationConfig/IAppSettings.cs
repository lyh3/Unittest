namespace Intel.MsoAuto.C3.MailService.Notification.Core {
    public interface IAppSettings {
        string EmailFrom { get; set; }
        string EmailRedirectTo { get; set; }
        string EmailTypeExclusive { get; set; }
        string ErrorNotifyTo { get; set; }
        DevConfig Dev { get; set; }
        IntConfig Int { get; set; }
        ProdConfig Prod { get; set; }
        string SMTPSERVER { get; set; }
    }
}