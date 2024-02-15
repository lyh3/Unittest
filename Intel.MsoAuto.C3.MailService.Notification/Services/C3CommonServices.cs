using Intel.MsoAuto.C3.MailService.Notification.DataContext;
using Intel.MsoAuto.C3.MailService.Notification.Entities;

namespace Intel.MsoAuto.C3.MailService.Notification.Services {
    public class C3CommonServices : IC3CommonServices {
        public IEmailTemplate GetEmailTemplateByName(string templateName)
        {
            return new C3CommonDataContext().GetEmailTemplateByName(templateName);
        }
        public NotificationSentEmails GetArAssignedNotificationEmails(bool hasSent)
        {
            return new C3CommonDataContext().GetArAssignedNotificationEmails(hasSent);
        }
        //--- Progression notification
        public void SaveProgressionNotificationEmails(ProgressionNotificationSentEmails sentEmails)
        {
            new C3CommonDataContext().SaveProgressionNotificationEmails(sentEmails);
        }
        public ProgressionNotificationSentEmails GetProgressionNotificationEmails(bool hasSent)
        {
            return new C3CommonDataContext().GetProgressionNotificationEmails(hasSent);
        }
        public ProgressionNotificationSentEmails LookupTrackedProgressionNotifications(ProgressionNotificationSentEmails notificationEmails, bool? hasSent = null)
        {
            return new C3CommonDataContext().LookupTrackedProgressionNotifications(notificationEmails, hasSent);
        }
        //--- ArAssigned notification
        public void SaveARAssignedNotificationEmails(ARAssignedNotificationSentEmails sentEmails)
        {
            new C3CommonDataContext().SaveArAssignedNotificationEmails(sentEmails);
        }
        public ARAssignedNotificationSentEmails GetARAssignedNotificationEmails(bool hasSent)
        {
            return new C3CommonDataContext().GetARAssignedNotificationEmails(hasSent);
        }
        public ARAssignedNotificationSentEmails LookupARAssignedTrackedNotifications(ARAssignedNotificationSentEmails notificationEmails, bool? hasSent = null)
        {
            return new C3CommonDataContext().LookupARAssignedTrackedNotifications(notificationEmails, hasSent);
        }
        public void SaveArAssignedNotificationEmails(ARAssignedNotificationSentEmails sentEmails)
        {
            new C3CommonDataContext().SaveArAssignedNotificationEmails(sentEmails);
        }
        //--- Bottleneck notification
        public void SaveBottleneckNotificationEmails(BottleneckNotificationSentEmails sentEmails)
        {
            new C3CommonDataContext().SaveBottleneckNotificationEmails(sentEmails);
        }
        public BottleneckNotificationSentEmails GetBottleneckNotificationEmails(bool hasSent)
        {
            return new C3CommonDataContext().GetBottleneckNotificationEmails(hasSent);
        }
        public BottleneckNotificationSentEmails LookupTrackedBottleneckNotifications(BottleneckNotificationSentEmails notificationEmails, bool? hasSent = null)
        {
            return new C3CommonDataContext().LookupTrackedBottleneckNotifications(notificationEmails, hasSent);
        }
    }
}
