using Intel.MsoAuto.C3.MailService.Notification.Entities;

namespace Intel.MsoAuto.C3.MailService.Notification.DataContext {
    public interface IC3CommonDataContext {
        IEmailTemplate GetEmailTemplateByName(string templateName);
        void SaveArAssignedNotificationEmails(ARAssignedNotificationSentEmails sentEmails);
        void SaveProgressionNotificationEmails(ProgressionNotificationSentEmails sentEmails);
        void SaveBottleneckNotificationEmails(BottleneckNotificationSentEmails sentEmails);
        NotificationSentEmails GetArAssignedNotificationEmails(bool hasSent);
        ARAssignedNotificationSentEmails GetARAssignedNotificationEmails(bool hasSent);
        ProgressionNotificationSentEmails GetProgressionNotificationEmails(bool hasSent);
        ProgressionNotificationSentEmails LookupTrackedProgressionNotifications(ProgressionNotificationSentEmails notificationEmails, bool? hasSent = null);
        ARAssignedNotificationSentEmails LookupARAssignedTrackedNotifications(ARAssignedNotificationSentEmails notificationEmails, bool? hasSent = null);
        BottleneckNotificationSentEmails GetBottleneckNotificationEmails(bool hasSent);
        BottleneckNotificationSentEmails LookupTrackedBottleneckNotifications(BottleneckNotificationSentEmails notificationEmails, bool? hasSent = null);
    }
}