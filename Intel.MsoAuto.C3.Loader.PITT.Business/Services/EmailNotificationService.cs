using Intel.MsoAuto.C3.MailService.Notification;
using Intel.MsoAuto.C3.MailService.Notification.Core;
using Intel.MsoAuto.C3.MailService.Notification.Entities;
using Intel.MsoAuto.C3.MailService.Notification.Services;
using log4net;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Services {
    public class EmailNotificationService {
        private ILog? _log = null;
        private string? _env = null;
        public EmailNotificationService( ILog log, string? env)
        {
            _log = log; _env = env;
            _env = env;
        }
        public void SendEmailNotification()
        {
            SendBottleneckForecastNotification();
            SendArAssignedNotification();
            SendProgressionNotification();
            RetrySendFailedEmailNotifications();
        }
        private void RetrySendFailedEmailNotifications()
        {
            NotificationSentEmails failedEmailSending = new NotificationSentEmails();
            failedEmailSending.AddRange(new C3CommonServices().GetArAssignedNotificationEmails(hasSent: false));
            failedEmailSending.AddRange(new C3CommonServices().GetProgressionNotificationEmails(hasSent: false));
            failedEmailSending.AddRange(new C3CommonServices().GetBottleneckNotificationEmails(hasSent: false));
            foreach (NotificationSentEmail email in failedEmailSending)
            {
                try
                {
                    if (email.EmailTemplate.Equals(Core.Constants.AR_ASSIGNED_TEMPLATE, StringComparison.OrdinalIgnoreCase))
                        DispatchRequest<ARAssignedNotificationDatum>(email.RequestJson);
                    if (email.EmailTemplate.Equals(Core.Constants.AR_ASSIGNED_OVERDUE_TEMPLATE, StringComparison.OrdinalIgnoreCase))
                        DispatchRequest<ARAssignedOverdueNotificationDatum>(email.RequestJson);
                    else if (email.EmailTemplate.Equals(Core.Constants.BOTTLENECK_TEMPLATE, StringComparison.OrdinalIgnoreCase))
                        DispatchRequest<BottleneckNotificationDatum>(email.RequestJson);
                    else if (email.EmailTemplate.Equals(Core.Constants.EPROGRESSION_STATUS_TEMPLATE, StringComparison.OrdinalIgnoreCase))
                        DispatchRequest<ProgressionStatusNotificationDatum>(email.RequestJson);
                }
                catch (Exception ex){
                    ex.ExceptionEmailNotification($"{GetType().Name} - RetrySendFailedEmailNotifications", new Configurations(environment: _env, sendEmail: true));
                }
            }
        }
        private void SendProgressionNotification()
        {
            try
            {
                Dictionary<string, NotificationData<ProgressionStatusNotificationDatum>> workflowStatus = new NotificationMongoDbSevice().GetWorkflowStatus();
                foreach (KeyValuePair<string, NotificationData<ProgressionStatusNotificationDatum>> keyVal in workflowStatus)
                {
                    MailNotification<ProgressionStatusNotificationDatum>.DispatchNotification(keyVal.Value);
                }
                if (_log != null)
                    _log.Info("--> Send Progression statuts change notifications success.");
            }
            catch (Exception ex)
            {
                ex.ExceptionEmailNotification($"{GetType().Name} - SendProgressionNotification", new Configurations(environment: _env, sendEmail: true));
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
                if (_log != null)
                    _log.Info("--> Send Bottleneck forecast notifications success.");
            }
            catch (Exception ex)
            {
                ex.ExceptionEmailNotification($"{GetType().Name} - SendBottleneckForecastNotification", new Configurations(environment: _env, sendEmail: true));
            }
        }
        private void SendArAssignedNotification()
        {
            try
            {
                var arDetails = new NotificationMongoDbSevice().GetArAssigned();
                var arNotificationData = ARAssignedNotificationDatum.TransformNotificationData(arDetails);
                _log.Info($"Get assigned ARs, count = {arNotificationData.Item1.Count}, overdue ARs count = {arNotificationData.Item2.Count}.");
                MailNotification<ARAssignedOverdueNotificationDatum>.DispatchNotification(arNotificationData.Item2);//Ar overdue
                MailNotification<ARAssignedNotificationDatum>.DispatchNotification(arNotificationData.Item1);
                _log.Info("--> Send ArAssigned and ArAssigned Overdure notifications success.");
            }
            catch (Exception ex)
            {
                ex.ExceptionEmailNotification($"{GetType().Name} - SendArAssignedNotification", new Configurations(environment: _env, sendEmail: true));
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
