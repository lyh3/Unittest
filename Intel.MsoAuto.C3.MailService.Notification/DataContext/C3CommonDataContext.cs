using Intel.MsoAuto.C3.MailService.Notification.Core;
using Intel.MsoAuto.C3.MailService.Notification.Entities;
using Intel.MsoAuto.DataAccess;
using Intel.MsoAuto.DataAccess.Sql;
using Intel.MsoAuto.Shared.Extensions;
using System.Data;

namespace Intel.MsoAuto.C3.MailService.Notification.DataContext {
    public class C3CommonDataContext : IC3CommonDataContext {
        private string _c3CommonConnectionString => Settings.EnvConfig.GetRequiredAppSettingsValueValidation(Constants.C3_COMMON_CONNECTION_STRING);
        public IEmailTemplate GetEmailTemplateByName(string templateName)
        {
            ISqlDataAccess? dataAccess = null;
            EmailTemplate result = new EmailTemplate();
            try
            {
                dataAccess = new DataAccessFactory().CreateSqlDataAccess(_c3CommonConnectionString, Constants.GET_EMAIL_TEMPLATE_BY_NAME);
                dataAccess.AddInputParameter("@TemplateName", templateName);
                dataAccess.SetTimeout(Constants.SQL_EXEC_TIMEOUT);
                dataAccess.ExecuteReader();
                if (dataAccess.DataReader.IsNotNull()
                    && dataAccess.DataReader.Read())
                {
                    bool isActive = dataAccess.DataReader["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataAccess.DataReader["IsActive"].ToString());
                    result.EmailTypeId = dataAccess.DataReader["EmailTypeId"].ToIntegerSafely();
                    result.TemplateName = dataAccess.DataReader["TemplateName"].ToStringSafely();
                    result.TemplateText = dataAccess.DataReader["TemplateText"].ToStringSafely();
                    result.TemplateDataFormat = dataAccess.DataReader["TemplateDataFormat"].ToStringSafely();
                    result.LanguageCode = dataAccess.DataReader["LanguageCode"].ToStringSafely();
                    result.EmailSubject = dataAccess.DataReader["EmailSubject"].ToStringSafely();
                    result.IsActive = isActive;
                }
            }
            catch (Exception ex)
            {
                Shared.Functions.LogMyError(ex, this);
                throw;
            }
            finally
            {
                if (null != dataAccess)
                    dataAccess.Close();
            }
            return result;
        }
        public NotificationSentEmails GetArAssignedNotificationEmails(bool hasSent)
        {
            ISqlDataAccess? dataAccess = null;
            var results = new NotificationSentEmails();
            try
            {
                dataAccess = new DataAccessFactory().CreateSqlDataAccess(_c3CommonConnectionString, Constants.GET_AR_ASSIGNED_NOTIFICATION_EMAILS);
                dataAccess.AddInputParameter("@HasSent", hasSent);
                dataAccess.SetTimeout(Constants.SQL_EXEC_TIMEOUT);
                dataAccess.ExecuteReader();
                if (dataAccess.DataReader.IsNotNull())
                {
                    while (dataAccess.DataReader.Read())
                    {
                        results.Add(new ARAssignedNotificationSentEmail {
                            ProjectId = dataAccess.DataReader["projectId"].ToStringSafely(),
                            EmailTemplate = dataAccess.DataReader["emailTemplate"].ToStringSafely(),
                            MessageQueueId = dataAccess.DataReader["messageQueueId"].ToStringSafely(),
                            RequestJson = dataAccess.DataReader["requestJson"].ToStringSafely(),
                            ArId = dataAccess.DataReader["ArId"].ToStringSafely(),
                            ArStatus = dataAccess.DataReader["ArStatus"].ToStringSafely(),
                            AR = dataAccess.DataReader["Ar"].ToStringSafely(),
                            ExpectedCompletionDate = dataAccess.DataReader["ExpectedCompletionDate"].ToStringSafely(),
                            Comments = dataAccess.DataReader["comments"].ToStringSafely(),
                            SentTo = dataAccess.DataReader["sentTo"].ToStringSafely(),
                            HasSent = dataAccess.DataReader["HasSent"].ToNullableBooleanSafely(),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Shared.Functions.LogMyError(ex, this);
                throw;
            }
            finally
            {
                if (null != dataAccess)
                    dataAccess.Close();
            }
            return results;
        }
        public ProgressionNotificationSentEmails LookupTrackedProgressionNotifications(ProgressionNotificationSentEmails notificationEmails, bool? hasSent = null)
        {
            ISqlDataAccess? dataAccess = null;
            var results = new ProgressionNotificationSentEmails();
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ProjectId", typeof(string));
                dt.Columns.Add("EmailTemplate", typeof(string));
                dt.Columns.Add("MessageQueueId", typeof(string));
                dt.Columns.Add("RequestJson", typeof(string));
                dt.Columns.Add("ProgressionStatus", typeof(string));
                dt.Columns.Add("StateId", typeof(string));
                dt.Columns.Add("SentTo", typeof(string));
                dt.Columns.Add("HasSent", typeof(bool));
                foreach (ProgressionNotificationSentEmail e in notificationEmails)
                {
                    var r = dt.NewRow();
                    r["ProjectId"] = e.ProjectId;
                    r["EmailTemplate"] = e.EmailTemplate;
                    r["MessageQueueId"] = e.MessageQueueId;
                    r["RequestJson"] = e.RequestJson;
                    r["ProgressionStatus"] = e.ProgressionStatus;
                    r["StateId"] = e.StateId;
                    r["SentTo"] = e.SentTo;
                    r["HasSent"] = (e.HasSent != null && e.HasSent.HasValue) ? e.HasSent.Value : DBNull.Value;
                    dt.Rows.Add(r);
                }
                dt.AcceptChanges();

                dataAccess = new DataAccessFactory().CreateSqlDataAccess(_c3CommonConnectionString, Constants.LOOKUP_TRACKED_PROGRESSION_NOTIFICATION);
                dataAccess.AddInputParameter("@NotificationEmails", dt);
                dataAccess.AddInputParameter("@HasSent", hasSent);
                dataAccess.SetTimeout(Constants.SQL_EXEC_TIMEOUT);
                dataAccess.ExecuteReader();
                if (dataAccess.DataReader.IsNotNull())
                {
                    while (dataAccess.DataReader.Read())
                    {
                        results.Add(new ProgressionNotificationSentEmail {
                            ProjectId = dataAccess.DataReader["projectId"].ToStringSafely(),
                            EmailTemplate = dataAccess.DataReader["emailTemplate"].ToStringSafely(),
                            MessageQueueId = dataAccess.DataReader["messageQueueId"].ToStringSafely(),
                            RequestJson = dataAccess.DataReader["requestJson"].ToStringSafely(),
                            ProgressionStatus = dataAccess.DataReader["progressionStatus"].ToStringSafely(),
                            StateId = dataAccess.DataReader["StateId"].ToStringSafely(),
                            SentTo = dataAccess.DataReader["sentTo"].ToStringSafely(),
                            HasSent = dataAccess.DataReader["HasSent"].ToNullableBooleanSafely(),
                        });
                    }
                }
           }
            catch (Exception ex)
            {
                Shared.Functions.LogMyError(ex, this);
                throw;
            }
            finally
            {
                if (null != dataAccess)
                    dataAccess.Close();
            }
            return results;
        }
        public void SaveProgressionNotificationEmails(ProgressionNotificationSentEmails sentEmails)
        {
            ISqlDataAccess? dataAccess = null;
            var dt = new DataTable();
            try
            {
                dataAccess = new DataAccessFactory().CreateSqlDataAccess(_c3CommonConnectionString, Constants.SAVE_PROGRESSION_NOTIFICATION_EMAILS);
                dt.Columns.Add("projectId", typeof(string));
                dt.Columns.Add("emailTemplate", typeof(string));
                dt.Columns.Add("MessageQueueId", typeof(string));
                dt.Columns.Add("requestJson", typeof(string));
                dt.Columns.Add("progressionStatus", typeof(string));
                dt.Columns.Add("stateId", typeof(string));
                dt.Columns.Add("sentTo", typeof(string));
                dt.Columns.Add("HasSent", typeof(bool));
                foreach (var item in sentEmails)
                {
                    var r = dt.NewRow();
                    r["projectId"] = item.ProjectId;
                    r["emailTemplate"] = item.EmailTemplate;
                    r["MessageQueueId"] = item.MessageQueueId;
                    r["requestJson"] = item.RequestJson.Trim();
                    r["progressionStatus"] = item.ProgressionStatus;
                    r["stateId"] = item.StateId;
                    r["sentTo"] = item.SentTo;
                    r["HasSent"] = (item.HasSent != null) ? item.HasSent : DBNull.Value;
                    dt.Rows.Add(r);
                }
                dt.AcceptChanges();
                dataAccess.AddInputParameter("@NotificationEmails", dt);
                dataAccess.SetTimeout(Constants.SQL_EXEC_TIMEOUT);
                dataAccess.Execute();
            }
            catch (Exception ex)
            {
                Shared.Functions.LogMyError(ex, this);
                throw;
            }
            finally
            {
                if (null != dataAccess)
                    dataAccess.Close();
            }
        }
        public ProgressionNotificationSentEmails GetProgressionNotificationEmails(bool hasSent)
        {
            ISqlDataAccess? dataAccess = null;
            var results = new ProgressionNotificationSentEmails();
            try
            {
                dataAccess = new DataAccessFactory().CreateSqlDataAccess(_c3CommonConnectionString, Constants.GET_PROGRESSION_NOTIFICATION_EMAILS);
                dataAccess.AddInputParameter("@HasSent", hasSent);
                dataAccess.SetTimeout(Constants.SQL_EXEC_TIMEOUT);
                dataAccess.ExecuteReader();
                if (dataAccess.DataReader.IsNotNull())
                {
                    while (dataAccess.DataReader.Read())
                    {
                        results.Add(new ProgressionNotificationSentEmail {
                            EmailTemplate = dataAccess.DataReader["emailTemplate"].ToStringSafely(),
                            RequestJson = dataAccess.DataReader["requestJson"].ToStringSafely(),
                            StateId = dataAccess.DataReader["stateId"].ToStringSafely(),
                            SentTo = dataAccess.DataReader["sentTo"].ToStringSafely(),
                            HasSent = dataAccess.DataReader["HasSent"].ToNullableBooleanSafely(),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Shared.Functions.LogMyError(ex, this);
                throw;
            }
            finally
            {
                if (null != dataAccess)
                    dataAccess.Close();
            }
            return results;
        }
        //--- ArAssigned notification
        public ARAssignedNotificationSentEmails GetARAssignedNotificationEmails(bool hasSent)
        {
            ISqlDataAccess? dataAccess = null;
            var results = new ARAssignedNotificationSentEmails();
            try
            {
                dataAccess = new DataAccessFactory().CreateSqlDataAccess(_c3CommonConnectionString, Constants.GET_AR_ASSIGNED_NOTIFICATION_EMAILS);
                dataAccess.AddInputParameter("@HasSent", hasSent);
                dataAccess.SetTimeout(Constants.SQL_EXEC_TIMEOUT);
                dataAccess.ExecuteReader();
                if (dataAccess.DataReader.IsNotNull())
                {
                    while (dataAccess.DataReader.Read())
                    {
                        results.Add(new ARAssignedNotificationSentEmail {
                            ArId = dataAccess.DataReader["ArId"].ToStringSafely(),
                            EmailTemplate = dataAccess.DataReader["emailTemplate"].ToStringSafely(),
                            RequestJson = dataAccess.DataReader["requestJson"].ToStringSafely(),
                            AR = dataAccess.DataReader["Ar"].ToStringSafely(),
                            ArStatus = dataAccess.DataReader["arStatus"].ToStringSafely(),
                            ExpectedCompletionDate = dataAccess.DataReader["ExpectedCompletionDate"].ToStringSafely(),
                            Comments = dataAccess.DataReader["Comments"].ToStringSafely(),
                            SentTo = dataAccess.DataReader["sentTo"].ToStringSafely(),
                            HasSent = dataAccess.DataReader["HasSent"].ToNullableBooleanSafely(),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Shared.Functions.LogMyError(ex, this);
                throw;
            }
            finally
            {
                if (null != dataAccess)
                    dataAccess.Close();
            }
            return results;
        }
        public ARAssignedNotificationSentEmails LookupARAssignedTrackedNotifications(ARAssignedNotificationSentEmails notificationEmails, bool? hasSent = null)
        {
            ISqlDataAccess? dataAccess = null;
            var results = new ARAssignedNotificationSentEmails();
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ArId", typeof(string));
                dt.Columns.Add("ProjectId", typeof(string));
                dt.Columns.Add("EmailTemplate", typeof(string));
                dt.Columns.Add("MessageQueueId", typeof(string));
                dt.Columns.Add("RequestJson", typeof(string));
                dt.Columns.Add("Ar", typeof(string));
                dt.Columns.Add("ArStatus", typeof(string));
                dt.Columns.Add("ExpectedCompletionDate", typeof(string));
                dt.Columns.Add("Comments", typeof(string));
                dt.Columns.Add("SentTo", typeof(string));
                dt.Columns.Add("HasSent", typeof(bool));
                foreach (ARAssignedNotificationSentEmail e in notificationEmails)
                {
                    var r = dt.NewRow();
                    r["ArId"] = e.ArId;
                    r["ProjectId"] = e.ProjectId;
                    r["EmailTemplate"] = e.EmailTemplate;
                    r["MessageQueueId"] = e.MessageQueueId;
                    r["RequestJson"] = e.RequestJson;
                    r["Ar"] = e.AR;
                    r["ArStatus"] = e.ArStatus;
                    r["ExpectedCompletionDate"] = e.ExpectedCompletionDate;
                    r["Comments"] = e.Comments;
                    r["SentTo"] = e.SentTo;
                    r["HasSent"] = (e.HasSent != null && e.HasSent.HasValue) ? e.HasSent.Value : DBNull.Value;
                    dt.Rows.Add(r);
                }
                dt.AcceptChanges();

                dataAccess = new DataAccessFactory().CreateSqlDataAccess(_c3CommonConnectionString, Constants.LOOKUP_AR_ASSIGNED_TRACKED_NOTIFICATION);
                dataAccess.AddInputParameter("@NotificationEmails", dt);
                dataAccess.AddInputParameter("@HasSent", hasSent);
                dataAccess.SetTimeout(Constants.SQL_EXEC_TIMEOUT);
                dataAccess.ExecuteReader();
                if (dataAccess.DataReader.IsNotNull())
                {
                    while (dataAccess.DataReader.Read())
                    {
                        results.Add(new ARAssignedNotificationSentEmail {
                            ArId = dataAccess.DataReader["ArId"].ToStringSafely(),
                            ProjectId = dataAccess.DataReader["projectId"].ToStringSafely(),
                            EmailTemplate = dataAccess.DataReader["emailTemplate"].ToStringSafely(),
                            MessageQueueId = dataAccess.DataReader["messageQueueId"].ToStringSafely(),
                            RequestJson = dataAccess.DataReader["requestJson"].ToStringSafely(),
                            AR = dataAccess.DataReader["Ar"].ToStringSafely(),
                            ArStatus = dataAccess.DataReader["arStatus"].ToStringSafely(),
                            ExpectedCompletionDate = dataAccess.DataReader["ExpectedCompletionDate"].ToStringSafely(),
                            Comments = dataAccess.DataReader["Comments"].ToStringSafely(),
                            SentTo = dataAccess.DataReader["sentTo"].ToStringSafely(),
                            HasSent = dataAccess.DataReader["HasSent"].ToNullableBooleanSafely(),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Shared.Functions.LogMyError(ex, this);
                throw;
            }
            finally
            {
                if (null != dataAccess)
                    dataAccess.Close();
            }
            return results;
        }
        public void SaveArAssignedNotificationEmails(ARAssignedNotificationSentEmails sentEmails)
        {
            ISqlDataAccess? dataAccess = null;
            var dt = new DataTable();
            try
            {
                dataAccess = new DataAccessFactory().CreateSqlDataAccess(_c3CommonConnectionString, Constants.SAVE_AR_ASSIGNED_NOTIFICATION_EMAILS);
                dt.Columns.Add("ArId", typeof(string));
                dt.Columns.Add("projectId", typeof(string));
                dt.Columns.Add("emailTemplate", typeof(string));
                dt.Columns.Add("MessageQueueId", typeof(string));
                dt.Columns.Add("requestJson", typeof(string));
                dt.Columns.Add("Ar", typeof(string));
                dt.Columns.Add("arStatus", typeof(string));
                dt.Columns.Add("ExpectedCompletionDate", typeof(string));
                dt.Columns.Add("Comments", typeof(string));
                dt.Columns.Add("sentTo", typeof(string));
                dt.Columns.Add("HasSent", typeof(bool));
                foreach (var item in sentEmails)
                {
                    var r = dt.NewRow();
                    r["ArId"] = item.ArId;
                    r["projectId"] = item.ProjectId;
                    r["emailTemplate"] = item.EmailTemplate;
                    r["MessageQueueId"] = item.MessageQueueId;
                    r["requestJson"] = item.RequestJson.Trim();
                    r["Ar"] = item.AR;
                    r["arStatus"] = item.ArStatus;
                    r["ExpectedCompletionDate"] = item.ExpectedCompletionDate;
                    r["Comments"] = item.Comments;
                    r["sentTo"] = item.SentTo;
                    r["HasSent"] = item.HasSent != null ? item.HasSent : DBNull.Value;
                    dt.Rows.Add(r);
                }
                dt.AcceptChanges();
                dataAccess.AddInputParameter("@NotificationEmails", dt);
                dataAccess.SetTimeout(Constants.SQL_EXEC_TIMEOUT);
                dataAccess.Execute();
            }
            catch (Exception ex)
            {
                Shared.Functions.LogMyError(ex, this);
            }
            finally
            {
                if (null != dataAccess)
                    dataAccess.Close();
            }
        }
        //--- Bottleneck notification
        public BottleneckNotificationSentEmails LookupTrackedBottleneckNotifications(BottleneckNotificationSentEmails notificationEmails, bool? hasSent = null)
        {
            ISqlDataAccess? dataAccess = null;
            var results = new BottleneckNotificationSentEmails();
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ProjectId", typeof(string));
                dt.Columns.Add("EmailTemplate", typeof(string));
                dt.Columns.Add("MessageQueueId", typeof(string));
                dt.Columns.Add("RequestJson", typeof(string));
                dt.Columns.Add("SiteName", typeof(string));
                dt.Columns.Add("Week", typeof(int));
                dt.Columns.Add("Year", typeof(int));
                dt.Columns.Add("CreatedBy", typeof(string));
                dt.Columns.Add("UpdatedBy", typeof(string));
                dt.Columns.Add("SentTo", typeof(string));
                dt.Columns.Add("HasSent", typeof(bool));
                foreach (BottleneckNotificationSentEmail e in notificationEmails)
                {
                    var r = dt.NewRow();
                    r["ProjectId"] = e.ProjectId;
                    r["EmailTemplate"] = e.EmailTemplate;
                    r["MessageQueueId"] = e.MessageQueueId;
                    r["RequestJson"] = e.RequestJson;
                    r["SiteName"] = e.SiteName;
                    r["Week"] = (e.Week != null && e.Week.HasValue) ? e.Week.Value : DBNull.Value;
                    r["Year"] = (e.Year != null && e.Year.HasValue) ? e.Year.Value : DBNull.Value;
                    r["CreatedBy"] = e.CreatedBy;
                    r["UpdatedBy"] = e.UpdatedBy;
                    r["SentTo"] = e.SentTo;
                    r["HasSent"] = (e.HasSent != null && e.HasSent.HasValue) ? e.HasSent.Value : DBNull.Value;
                    dt.Rows.Add(r);
                }
                dt.AcceptChanges();

                dataAccess = new DataAccessFactory().CreateSqlDataAccess(_c3CommonConnectionString, Constants.LOOKUP_TRACKED_BOTTLENECK_NOTIFICATION);
                dataAccess.AddInputParameter("@NotificationEmails", dt);
                dataAccess.AddInputParameter("@HasSent", hasSent);
                dataAccess.SetTimeout(Constants.SQL_EXEC_TIMEOUT);
                dataAccess.ExecuteReader();
                if (dataAccess.DataReader.IsNotNull())
                {
                    while (dataAccess.DataReader.Read())
                    {
                        results.Add(new BottleneckNotificationSentEmail {
                            ProjectId = dataAccess.DataReader["projectId"].ToStringSafely(),
                            EmailTemplate = dataAccess.DataReader["emailTemplate"].ToStringSafely(),
                            MessageQueueId = dataAccess.DataReader["messageQueueId"].ToStringSafely(),
                            RequestJson = dataAccess.DataReader["requestJson"].ToStringSafely(),
                            SentTo = dataAccess.DataReader["sentTo"].ToStringSafely(),
                            SiteName = dataAccess.DataReader["SiteName"].ToStringSafely(),
                            Week = dataAccess.DataReader["Week"].ToIntegerSafely(),
                            Year = dataAccess.DataReader["Year"].ToIntegerSafely(),
                            CreatedBy = dataAccess.DataReader["CreatedBy"].ToStringSafely(),
                            UpdatedBy = dataAccess.DataReader["UpdatedBy"].ToStringSafely(),
                            HasSent = dataAccess.DataReader["HasSent"].ToNullableBooleanSafely(),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Shared.Functions.LogMyError(ex, this);
                throw;
            }
            finally
            {
                if (null != dataAccess)
                    dataAccess.Close();
            }
            return results;
        }
        public void SaveBottleneckNotificationEmails(BottleneckNotificationSentEmails sentEmails)
        {
            ISqlDataAccess? dataAccess = null;
            DataTable dt = new DataTable();
            try
            {
                dataAccess = new DataAccessFactory().CreateSqlDataAccess(_c3CommonConnectionString, Constants.SAVE_BOTTLENECK_NOTIFICATION_EMAILS);
                dt.Columns.Add("projectId", typeof(string));
                dt.Columns.Add("emailTemplate", typeof(string));
                dt.Columns.Add("MessageQueueId", typeof(string));
                dt.Columns.Add("requestJson", typeof(string));
                dt.Columns.Add("SiteName", typeof(string));
                dt.Columns.Add("Week", typeof(int));
                dt.Columns.Add("Year", typeof(int));
                dt.Columns.Add("CreatedBy", typeof(string));
                dt.Columns.Add("UpdatedBy", typeof(string));
                dt.Columns.Add("sentTo", typeof(string));
                dt.Columns.Add("HasSent", typeof(bool));
                foreach (var item in sentEmails)
                {
                    var r = dt.NewRow();
                    r["projectId"] = item.ProjectId;
                    r["emailTemplate"] = item.EmailTemplate;
                    r["MessageQueueId"] = item.MessageQueueId;
                    r["requestJson"] = item.RequestJson.Trim();
                    r["SiteName"] = item.SiteName;
                    r["Week"] = item.Week != null ? item.Week : DBNull.Value;
                    r["Year"] = item.Year != null ? item.Year : DBNull.Value;
                    r["CreatedBy"] = item.CreatedBy;
                    r["UpdatedBy"] = item.UpdatedBy;
                    r["sentTo"] = item.SentTo;
                    r["HasSent"] = item.HasSent != null ? item.HasSent : DBNull.Value;
                    dt.Rows.Add(r);
                }
                dt.AcceptChanges();
                dataAccess.AddInputParameter("@NotificationEmails", dt);
                dataAccess.SetTimeout(Constants.SQL_EXEC_TIMEOUT);
                dataAccess.Execute();
            }
            catch (Exception ex)
            {
                Shared.Functions.LogMyError(ex, this);
                throw;
            }
            finally
            {
                if (null != dataAccess)
                    dataAccess.Close();
            }
        }
        public BottleneckNotificationSentEmails GetBottleneckNotificationEmails(bool hasSent)
        {
            ISqlDataAccess? dataAccess = null;
            var results = new BottleneckNotificationSentEmails();
            try
            {
                dataAccess = new DataAccessFactory().CreateSqlDataAccess(_c3CommonConnectionString, Constants.GET_BOTTLENECK_NOTIFICATION_EMAILS);
                dataAccess.AddInputParameter("@HasSent", hasSent);
                dataAccess.SetTimeout(Constants.SQL_EXEC_TIMEOUT);
                dataAccess.ExecuteReader();
                if (dataAccess.DataReader.IsNotNull())
                {
                    while (dataAccess.DataReader.Read())
                    {
                        results.Add(new BottleneckNotificationSentEmail {
                            EmailTemplate = dataAccess.DataReader["emailTemplate"].ToStringSafely(),
                            RequestJson = dataAccess.DataReader["requestJson"].ToStringSafely(),
                            SentTo = dataAccess.DataReader["sentTo"].ToStringSafely(),
                            SiteName = dataAccess.DataReader["SiteName"].ToStringSafely(),
                            Week = dataAccess.DataReader["Week"].ToIntegerSafely(),
                            Year = dataAccess.DataReader["Year"].ToIntegerSafely(),
                            CreatedBy = dataAccess.DataReader["CreatedBy"].ToStringSafely(),
                            UpdatedBy = dataAccess.DataReader["UpdatedBy"].ToStringSafely(),
                            HasSent = dataAccess.DataReader["HasSent"].ToNullableBooleanSafely(),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Shared.Functions.LogMyError(ex, this);
                throw;
            }
            finally
            {
                if (null != dataAccess)
                    dataAccess.Close();
            }
            return results;
        }
    }
}
