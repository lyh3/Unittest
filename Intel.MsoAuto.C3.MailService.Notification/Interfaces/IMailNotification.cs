using Apache.NMS;
using Apache.NMS.ActiveMQ.Commands;
using Intel.MsoAuto.C3.MailService.Notification.Entities;

namespace Intel.MsoAuto.C3.MailService.Notification {
    public interface IMailNotification<T> {
        string CcTo { get; set; }
        string EmailFrom { get; set; }
        string EmailRedirecTo { get; set; }
        string EmailTemplate { get; set; }
        string LinkWebsiteUrl { get; set; }
        string SendTo { get; set; }
        string SmtpServer { get; set; }
        void ConsumerListener(IMessage message);
        bool SendEmail(EmailNotificationRequest<NotificationData<T>> requestrequest);
    }
}