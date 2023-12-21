using Intel.MsoAuto.C3.MailService.Notification;
using Intel.MsoAuto.C3.MailService.Notification.Entities;
using Intel.MsoAuto.C3.MailService.Notification.Services;

namespace Intel.MsoAuto.C3.Loader.CC.Business.Services {
    public class EmailNotificationService 
    {
        public void SendArAssignedNotification()
        {
            var arDetails = new NotificationMongoDbSevice().GetArAssigned();
            var arNotificationData = ARAssignedNotificationDatum.TransformNotificationData(arDetails);
            MailNotification<ARAssignedOverdueNotificationDatum>.DispatchNotification(arNotificationData.Item2);//Ar overdue
            MailNotification<ARAssignedNotificationDatum>.DispatchNotification(arNotificationData.Item1);
        }
        public void RetrySendFailedEmailNotifications()
        {
            var failedEmailSending = new C3CommonServices().GetNotificationEmails(hasSent: false);
            foreach(var email in failedEmailSending)
            {
                if (email.EmailTemplate.Equals("ARAssigned", StringComparison.OrdinalIgnoreCase))
                    DispatchRequest<ARAssignedNotificationDatum>(email.RequestJson);
                if (email.EmailTemplate.Equals("ARAssignedOverdue", StringComparison.OrdinalIgnoreCase))
                    DispatchRequest<ARAssignedOverdueNotificationDatum>(email.RequestJson);
                else if (email.EmailTemplate.Equals("ProjectStatus", StringComparison.OrdinalIgnoreCase))
                    DispatchRequest<ProjectStatusNotificationDatum>(email.RequestJson);
            }
        }
        private static void DispatchRequest<T>(string request)
        {
            //var messageQueue = new ActiveMessageQueue<T>();
            //var notification = new MailNotification<T>();
            //messageQueue.Consumer.Listener += notification.ConsumerListener;
            //messageQueue.SendMessageToQueue(request, notification);
            var notificationRequest = Newtonsoft.Json.JsonConvert.DeserializeObject<EmailNotificationRequest<NotificationData<T>>>(request);
            if (notificationRequest != null)
                MailNotification<T>.DispatchNotification<T>(notificationRequest.NotificationData);
        }
    }
}
