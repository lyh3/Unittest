using Intel.MsoAuto.C3.MailService.Notification.Core;
using Intel.MsoAuto.C3.MailService.Notification.Entities;
using Intel.MsoAuto.C3.MailService.Notification.Services;
using Intel.MsoAuto.Shared.DirectoryServices;
using Newtonsoft.Json;
using System.Text;
using System.Xml.Serialization;

namespace Intel.MsoAuto.C3.MailService.Notification {
    public partial class MailNotification<T> {
        public static String XmlSerialize<U>(object o)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(U));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            StringBuilder sb = new StringBuilder();
            using (StringWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, o);
            }
            string xml = sb.ToString().Replace(Constants.XML_TRIM_1, string.Empty).Replace(Constants.XML_TRIM_2, string.Empty);
            return xml;
        }
        internal static bool ExceptonEmailNotification(Exception? ex, string source)
        {
            bool success = false;
            if (ex != null)
            {
                string errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                errorMessage = $"[{DateTime.Now}] - Error caught from [{source}], error was : {errorMessage}";
                Shared.Functions.LogMyError(ex, source);
                var request = new EmailNotificationRequest<NotificationData<FailureNotificationDatum>> {
                    NotificationData = new NotificationData<FailureNotificationDatum>()
                };
                dynamic? section = Settings.Configuration.GetSection($"{Constants.CONFIGURATIONS}:{Constants.APP_SETTINGS}:{Constants.ERROR_NOTIFY_TO}");
                if (section != null)
                {
                    string sendTo = section.Value.ToString();
                    var datum = new FailureNotificationDatum {
                        UserName = sendTo,
                        ErrorMessage = errorMessage,
                        Source = source
                    };
                    request.NotificationData.Add(datum);

                    if (IsExclusiveEmailType(request.NotificationData as NotificationData<T>))
                        return false;

                    var notification = new MailNotification<FailureNotificationDatum>();
                    notification.SendTo = sendTo;
                    success = notification.SendEmail(request);
                    if (success)
                    {
                        string msg = $"Send failure email notification to {datum.Email} success. Error message = {errorMessage}";
                        Shared.Functions.LogMyInfo(msg, source);
                        Console.WriteLine(msg);
                    }
                }
                else
                    throw new Exception("Mission configuration section [ErrorNotifyTo]");
            }
            return success;
        }
        private void EsclateNotificationToManager()
        {
            Type bottlenecktype = typeof(BottleneckNotificationDatum);
            if (typeof(T) == bottlenecktype)
            {
                try
                {
                    if (!string.IsNullOrEmpty(Idsid))
                    {
                        Employee? manager = new EmployeeDataProvider().GetUserManager(Idsid);
                        if (manager != null)
                        {
                            CcTo = SendTo;
                            SendTo = manager.Email;
                        }
                    }
                    else
                        Shared.Functions.LogMyWarning(new ApplicationException($"To esclate an email to manager with the notification bottlenecktype of {bottlenecktype.Name}, the idsid is required."), this);
                }
                catch (Exception ex)
                {
                    Shared.Functions.LogMyError(ex, this);
                }
            }
        }
        private IEmailNotificationRequest<NotificationData<T>> SaveProgressionNotificationTracking(EmailNotificationRequest<NotificationData<T>> request, string messageQueueId = "", bool? hasSent = null)
        {
            int count = 0;
            ProgressionNotificationSentEmails existEmails = new ProgressionNotificationSentEmails();
            ProgressionNotificationSentEmails notificationEmails = new ProgressionNotificationSentEmails();
            IEmailNotificationRequest<NotificationData<T>>? results = new EmailNotificationRequest<NotificationData<T>>();
            results.LinkWebsiteUrl = request.LinkWebsiteUrl;
            results.NotificationData = new NotificationData<T>();
            try
            {
                request.LinkWebsiteUrl = this.LinkWebsiteUrl;
                ProgressionNotificationSentEmails sentEmals = new ProgressionNotificationSentEmails();
                C3CommonServices c3CommonServices = new C3CommonServices();
                NotificationData<T>? notificationData = null;
                foreach (KeyValuePair<string, EmailSendTo<T>> keyVal in request.NotificationData.EmailSendTo)
                {
                    notificationData = keyVal.Value.Content;
                    string emailTemplateName = notificationData.NotificationTemplateName;
                    string json = JsonConvert.SerializeObject(request).Trim();
                    foreach (var x in keyVal.Value.Content)
                    {
                        ProgressionStatusNotificationDatum? datum = x as ProgressionStatusNotificationDatum;
                        if (datum != null)
                        {
                            sentEmals.Add(new ProgressionNotificationSentEmail {
                                SentTo = datum.Email,
                                ProjectId = datum.ProjectId,
                                ProgressionStatus = datum.ProgressionStatus,
                                StateId = datum.StateId,
                                EmailTemplate = emailTemplateName,
                                MessageQueueId = messageQueueId,
                                RequestJson = json,
                                HasSent = hasSent,
                            });
                        }
                    }
                }
                existEmails.AddRange(c3CommonServices.LookupTrackedProgressionNotifications(sentEmals, hasSent: hasSent));
                foreach (ProgressionNotificationSentEmail e in sentEmals)
                {
                    if (existEmails.Lookup(e, hasSent) == (hasSent == null ? false : hasSent))
                    {
                        notificationEmails.Add(e);
                        dynamic? d = request.NotificationData.FirstOrDefault(x => DatumTypeCast(x)?.ProjectId == e.ProjectId
                                                                                 && DatumTypeCast(x)?.Email == e.SentTo
                                                                                 && DatumTypeCast(x)?.StateId == e.StateId);
                        if (d != null)
                            results.NotificationData.Add(d);
                    }
                }
                if (notificationEmails.Count > 0)
                {
                    count = notificationEmails.Count;
                    c3CommonServices.SaveProgressionNotificationEmails(notificationEmails);
                }
            }
            catch (Exception ex)
            {
                Shared.Functions.LogError(ex);
                throw;
            }
            return results;
        }
        private IEmailNotificationRequest<NotificationData<T>> SaveArAssignedNotificationTracking(EmailNotificationRequest<NotificationData<T>> request, string messageQueueId = "", bool? hasSent = null)
        {
            int count = 0;
            ARAssignedNotificationSentEmails existEmails = new ARAssignedNotificationSentEmails();
            ARAssignedNotificationSentEmails notificationEmails = new ARAssignedNotificationSentEmails();
            IEmailNotificationRequest<NotificationData<T>>? results = new EmailNotificationRequest<NotificationData<T>>();
            results.LinkWebsiteUrl = request.LinkWebsiteUrl;
            results.NotificationData = new NotificationData<T>();
            try
            {
                request.LinkWebsiteUrl = this.LinkWebsiteUrl;
                ARAssignedNotificationSentEmails sentEmals = new ARAssignedNotificationSentEmails();
                C3CommonServices c3CommonServices = new C3CommonServices();
                foreach (KeyValuePair<string, EmailSendTo<T>> keyVal in request.NotificationData.EmailSendTo)
                {
                    NotificationData<T>? notificationData = keyVal.Value.Content;
                    string json = JsonConvert.SerializeObject(request).Trim();
                    foreach (var x in keyVal.Value.Content)
                    {
                        ARAssignedNotificationDatum? datum = x as ARAssignedNotificationDatum;
                        if (datum != null)
                        {
                            sentEmals.Add(new ARAssignedNotificationSentEmail {
                                ArId = datum.ArId,
                                SentTo = datum.Email,
                                ProjectId = datum.ProjectId,
                                EmailTemplate = keyVal.Value.Content.NotificationTemplateName,
                                MessageQueueId = messageQueueId,
                                RequestJson = json,
                                AR = datum.AR,
                                ArStatus = datum.ArStatus,
                                ExpectedCompletionDate = datum.ExpectedCompletionDate,
                                Comments = datum.Comments,
                                HasSent = keyVal.Value.HasSent,
                            });
                        }
                    }

                }
                existEmails.AddRange(c3CommonServices.LookupARAssignedTrackedNotifications(sentEmals, hasSent: hasSent));
                foreach (ARAssignedNotificationSentEmail e in sentEmals)
                {
                    if (existEmails.Lookup(e, hasSent) == (hasSent == null ? false : hasSent))
                    {
                        notificationEmails.Add(e);
                        dynamic? d = request.NotificationData.FirstOrDefault(x => DatumTypeCast(x)?.ArId == e.ArId);

                        if (d != null)
                            results.NotificationData.Add(d);
                    }
                }
                if (notificationEmails.Count > 0)
                {
                    count = notificationEmails.Count;
                    c3CommonServices.SaveArAssignedNotificationEmails(notificationEmails);
                }
            }
            catch (Exception ex)
            {
                Shared.Functions.LogError(ex);
                throw;
            }
            return results;

        }
        private IEmailNotificationRequest<NotificationData<T>> SaveBottleneckNotificationTracking(EmailNotificationRequest<NotificationData<T>> request, string messageQueueId = "", bool? hasSent = null)
        {
            int count = 0;
            BottleneckNotificationSentEmails existEmails = new BottleneckNotificationSentEmails();
            BottleneckNotificationSentEmails notificationEmails = new BottleneckNotificationSentEmails();
            IEmailNotificationRequest<NotificationData<T>>? results = new EmailNotificationRequest<NotificationData<T>>();
            results.LinkWebsiteUrl = request.LinkWebsiteUrl;
            results.NotificationData = new NotificationData<T>();
            try
            {
                request.LinkWebsiteUrl = this.LinkWebsiteUrl;
                BottleneckNotificationSentEmails sentEmals = new BottleneckNotificationSentEmails();
                C3CommonServices c3CommonServices = new C3CommonServices();
                NotificationData<T>? notificationData = null;
                foreach (KeyValuePair<string, EmailSendTo<T>> keyVal in request.NotificationData.EmailSendTo)
                {
                    notificationData = keyVal.Value.Content;
                    string emailTemplateName = notificationData.NotificationTemplateName;
                    string json = JsonConvert.SerializeObject(request).Trim();
                    foreach (var x in keyVal.Value.Content)
                    {
                        BottleneckNotificationDatum? datum = x as BottleneckNotificationDatum;
                        if (datum != null)
                        {
                            sentEmals.Add(new BottleneckNotificationSentEmail {
                                SentTo = datum.Email,
                                ProjectId = datum.ProjectId,
                                EmailTemplate = emailTemplateName,
                                MessageQueueId = messageQueueId,
                                RequestJson = json,
                                SiteName = datum.siteName,
                                Week = datum.week,
                                Year = datum.year,
                                CreatedBy = datum.createdBy,
                                UpdatedBy = datum.updatedBy,
                                HasSent = hasSent,
                            });
                        }
                    }
                }
                existEmails.AddRange(c3CommonServices.LookupTrackedBottleneckNotifications(sentEmals, hasSent: hasSent));
                foreach (BottleneckNotificationSentEmail e in sentEmals)
                {
                    if (existEmails.Lookup(e, hasSent) == (hasSent == null ? false : hasSent))
                    {
                        notificationEmails.Add(e);
                        dynamic? d = request.NotificationData.FirstOrDefault(x => DatumTypeCast(x)?.ProjectId == e.ProjectId
                                                                                 && DatumTypeCast(x)?.Email == e.SentTo
                                                                                 && DatumTypeCast(x)?.siteName == e.SiteName
                                                                                 && DatumTypeCast(x)?.week == e.Week
                                                                                 && DatumTypeCast(x)?.year == e.Year
                                                                                 && DatumTypeCast(x)?.createdBy == e.CreatedBy
                                                                                 && DatumTypeCast(x)?.updatedBy == e.UpdatedBy
                                                                                 );
                        if (d != null)
                            results.NotificationData.Add(d);
                    }
                }
                if (notificationEmails.Count > 0)
                {
                    count = notificationEmails.Count;
                    c3CommonServices.SaveBottleneckNotificationEmails(notificationEmails);
                }
            }
            catch (Exception ex)
            {
                Shared.Functions.LogError(ex);
                throw;
            }
            return results;
        }
        private static Func<T, dynamic?> DatumTypeCast = x => {
            var type = typeof(T);
            if (type == typeof(ARAssignedNotificationDatum))
                return x as ARAssignedNotificationDatum;
            if (type == typeof(ARAssignedOverdueNotificationDatum))
                return x as ARAssignedOverdueNotificationDatum;
            if (type == typeof(BottleneckNotificationDatum))
                return x as BottleneckNotificationDatum;
            if (type == typeof(ProgressionStatusNotificationDatum))
                return x as ProgressionStatusNotificationDatum;
            return null;
        };
        private static bool IsExclusiveEmailType(NotificationData<T>? data)
        {
            bool ret = false;
            if (data != null)
            {
                string notificationTypeName = data.NotificationTemplateName.Replace(Constants.TEMPLATE, string.Empty);
                List<string> exclusiveEmails = new List<string>();
                string? emailTypeExclud = Settings.Configuration.GetSection(Settings.EmailExcludeConfigurationPath).Value;
                if (emailTypeExclud != null)
                {
                    exclusiveEmails.AddRange(emailTypeExclud.Split(','));
                    if (exclusiveEmails != null && exclusiveEmails.Contains(notificationTypeName))
                    {
                        Shared.Functions.LogInfo($"The notification of [{notificationTypeName}] is skipped by configuration.");
                        ret = true;
                    }
                }
            }
            return ret;
        }
    }
}
