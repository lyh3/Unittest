using Intel.MsoAuto.C3.MailService.Notification;
using Intel.MsoAuto.C3.MailService.Notification.Entities;
using Intel.MsoAuto.C3.MailService.Notification.Services;
namespace Intel.MsoAuto.C3.Loader.PITT.Business.Services {
    public class EmailNotificationService {
        public void SendEmailNotification()
        {
            SendBottleneckForecastNotification();
            SendArAssignedNotification();
            RetrySendFailedEmailNotifications();
        }
        private void RetrySendFailedEmailNotifications()
        {
            var failedEmailSending = new C3CommonServices().GetNotificationEmails(hasSent: false);
            foreach (var email in failedEmailSending)
            {
                if (email.EmailTemplate.Equals("ARAssigned", StringComparison.OrdinalIgnoreCase))
                    DispatchRequest<ARAssignedNotificationDatum>(email.RequestJson);
                if (email.EmailTemplate.Equals("ARAssignedOverdue", StringComparison.OrdinalIgnoreCase))
                    DispatchRequest<ARAssignedOverdueNotificationDatum>(email.RequestJson);
                else if (email.EmailTemplate.Equals("ProjectStatus", StringComparison.OrdinalIgnoreCase))
                    DispatchRequest<ProjectStatusNotificationDatum>(email.RequestJson);
            }
        }
        private void SendBottleneckForecastNotification()
        {
            try
            {
                var bottlenecksForecast = new NotificationMongoDbSevice().GetBottleneckForeCasts();
                foreach (KeyValuePair<string, NotificationData<BottleneckNotificationDatum>> keyVal in bottlenecksForecast)
                {
                    MailNotification<BottleneckNotificationDatum>.DispatchNotification(keyVal.Value);
                }
            }
            catch (Exception ex)
            {
                Shared.Functions.LogMyError(ex, this);
            }
        }
        private void SendArAssignedNotification()
        {
            try
            {
                var arDetails = new NotificationMongoDbSevice().GetArAssigned();
                var arNotificationData = ARAssignedNotificationDatum.TransformNotificationData(arDetails);
                Shared.Functions.LogInfo($"Get assigned ARs, count = {arNotificationData.Item1.Count}, overdue ARs count = {arNotificationData.Item2.Count}.");
                MailNotification<ARAssignedOverdueNotificationDatum>.DispatchNotification(arNotificationData.Item2);//Ar overdue
                MailNotification<ARAssignedNotificationDatum>.DispatchNotification(arNotificationData.Item1);
            }
            catch (Exception ex)
            {
                Shared.Functions.LogMyError(ex, this);
            }
        }
        private static void DispatchRequest<T>(string request)
        {
            var notificationRequest = Newtonsoft.Json.JsonConvert.DeserializeObject<EmailNotificationRequest<NotificationData<T>>>(request);
            if (notificationRequest != null)
                MailNotification<T>.DispatchNotification<T>(notificationRequest.NotificationData);
        }
    }
}
