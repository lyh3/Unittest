using Intel.MsoAuto.C3.MailService.Notification.Entities;
using Intel.MsoAuto.Mail;
using Intel.MsoAuto.Shared.Extensions;
using Apache.NMS;
using Intel.MsoAuto.C3.MailService.Notification.Services;
using Apache.NMS.ActiveMQ.Commands;
using Newtonsoft.Json;
using Intel.MsoAuto.C3.MailService.Notification.Core;
using Microsoft.Extensions.Configuration;

namespace Intel.MsoAuto.C3.MailService.Notification {
    public partial class MailNotification<T> : IMailNotification<T> {
        public string EmailFrom { get; set; } =  string.Empty;
        public string EmailRedirecTo { get; set; } =  string.Empty;
        public string SmtpServer { get; set; } =  string.Empty;
        public string LinkWebsiteUrl { get; set; } =  string.Empty;
        public string EmailTemplate { get; set; } =  string.Empty;
        public string SendTo { get; set; } =  string.Empty;
        public string CcTo { get; set; } =  string.Empty;
        public string Idsid { get; set;} = string.Empty;
        protected bool _redirect { get; } = false;
        protected string _subjectSuffix { get; set; } = string.Empty;
        public bool? Status { get; set; } = null;
        public MailNotification()
        {
            IConfigurationSection? config = Settings.Configuration.GetSection($"{Constants.CONFIGURATIONS}:{Constants.APP_SETTINGS}");
            EmailFrom = config.GetRequiredAppSettingsValueValidation(Constants.EMAIL_FROM);
            EmailRedirecTo = config.GetRequiredAppSettingsValueValidation(Constants.EMAIL_REDIRECT_TO);
            SmtpServer = config.GetRequiredAppSettingsValueValidation(Constants.SMTP_SERVER);
            LinkWebsiteUrl = Settings.EnvConfig.GetRequiredAppSettingsValueValidation(Constants.LINK_WEBSITE_URL);
            string? environment = Settings.Configuration.GetSection(Constants.ENVIRONMENT).Value;
            if (!string.IsNullOrEmpty(environment))
                _subjectSuffix = $" - [{environment}]";
            _redirect = !string.IsNullOrEmpty(EmailRedirecTo);
        }
        public bool SendEmail(EmailNotificationRequest<NotificationData<T>> request)
        {
            string ccTo = string.Empty;
            bool sucess = false;
            try
            {
                EsclateNotificationToManager();
                if (_redirect || string.IsNullOrEmpty(SendTo)) { SendTo = EmailRedirecTo; }
                if (!string.IsNullOrEmpty(CcTo) && _redirect)
                {
                    ccTo = EmailRedirecTo;
                }
                IEmailTemplate template = new C3CommonServices().GetEmailTemplateByName(request.NotificationData.NotificationTemplateName);
                string subject = $"{template.EmailSubject}{_subjectSuffix}";
                string xmlData = XmlSerialize<EmailNotificationRequest<NotificationData<T>>>(request);
                Transformer transformer = new Transformer();
                string body = transformer.Transform(template.TemplateText, xmlData);
                MailBuilder mail = string.IsNullOrEmpty(ccTo)
                            ? new MailBuilder(EmailFrom, SendTo)
                                .UsingTemplateText(template.TemplateText)
                                .UsingSmtpServer(SmtpServer)
                                .Subject(subject)
                                .Body(body)
                            : new MailBuilder(EmailFrom, SendTo)
                                .UsingTemplateText(template.TemplateText)
                                .UsingSmtpServer(SmtpServer)
                                .Subject(subject)
                                .Body(body)
                                .Cc(ccTo);
                sucess = mail.Send();
             }
            catch (Exception ex)
            {
                Shared.Functions.LogMyError(ex, this);
                throw;
            }
            return sucess;
        }
        public void ConsumerListener(IMessage message)
        {
            ActiveMQTextMessage amqMessage = (ActiveMQTextMessage)message;
            string requestJson = amqMessage.Text;
            EmailNotificationRequest<NotificationData<T>>? request = JsonConvert.DeserializeObject<EmailNotificationRequest<NotificationData<T>>>(requestJson);
            if (request != null)
            {
                request.LinkWebsiteUrl = this.LinkWebsiteUrl;
                Dictionary<string, EmailSendTo<T>> emailSendTo = request.NotificationData.EmailSendTo;
                int count = 0;
                if (null != emailSendTo)
                {
                    bool success = true;
                    foreach (KeyValuePair<string, EmailSendTo<T>> keyVal in emailSendTo)
                    {
                        SendTo = keyVal.Value.SendTo;
                        CcTo = keyVal.Value.CcTo;
                        Idsid = keyVal.Value.Idsid;
                        NotificationData<T> notificationData = keyVal.Value.Content;
                        EmailNotificationRequest<NotificationData<T>> req = new EmailNotificationRequest<NotificationData<T>> {
                            LinkWebsiteUrl = this.LinkWebsiteUrl,
                            NotificationData = notificationData
                        };
                        string json = JsonConvert.SerializeObject(request).Trim();
                        keyVal.Value.HasSent = SendEmail(req);
                        success &= keyVal.Value.HasSent;
                        count++;
                    }
                    Status = success;
                    if (count > 0)
                        SaveNotificationTracking(request, amqMessage.MessageId.ProducerId.ConnectionId, hasSent: true);
                }
            }
            else
                throw new ArgumentException($"Failed to DeserializeObject with input json data {amqMessage.Text}");
        }
        public IEmailNotificationRequest<NotificationData<T>> SaveNotificationTracking(EmailNotificationRequest<NotificationData<T>> request, string messageQueueId = "", bool? hasSent = null)
        {
            var type = typeof(T);
            if (type == typeof(ARAssignedNotificationDatum) || type == typeof(ARAssignedOverdueNotificationDatum))
                return SaveArAssignedNotificationTracking(request, messageQueueId, hasSent);
            else if (type == typeof(ProgressionStatusNotificationDatum))
                return SaveProgressionNotificationTracking(request, messageQueueId, hasSent);
            else if (type == typeof(BottleneckNotificationDatum))
                return SaveBottleneckNotificationTracking(request, messageQueueId, hasSent);
            else
                throw new Exception($"The type of [{type.Name}] is not implemented.") ;
        }
        public static async Task<bool> DispatchNotificationAsync<U> (NotificationData<U> notificationData)
        {
            bool status = false;
            await Task.Run(() =>
            {
                status = Dispatch(notificationData);
            });
            return status;
        }
        public static bool DispatchNotification<U>(NotificationData<U> notificationData)
        {
            return Dispatch(notificationData);
        }
        private static bool Dispatch<U>(NotificationData<U> notificationData)
        {
            using (ActiveMessageQueue<U> messageQueue = new ActiveMessageQueue<U>())
            {
                MailNotification<U> notification = new MailNotification<U>();
                if (notificationData != null)
                {
                    EmailNotificationRequest<NotificationData<U>> notificationResuest = new EmailNotificationRequest<NotificationData<U>> {
                        LinkWebsiteUrl = Settings.EnvConfig.GetRequiredAppSettingsValueValidation(Constants.LINK_WEBSITE_URL),
                        NotificationData = notificationData
                    };

                    if (IsExclusiveEmailType(notificationResuest.NotificationData as NotificationData<T>))
                        return false;

                    string jsonRequest = JsonConvert.SerializeObject(notificationResuest);
                    messageQueue.Consumer.Listener += notification.ConsumerListener;
                    messageQueue.SendMessageToQueue(jsonRequest, notification);
                    Thread.Sleep(5000);
                }
                return notification.Status.HasValue ? notification.Status.Value : false;
            }
        }
    }
}