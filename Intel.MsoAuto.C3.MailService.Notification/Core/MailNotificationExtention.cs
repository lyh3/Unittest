using Intel.MsoAuto.C3.MailService.Notification.Core;
using Intel.MsoAuto.C3.MailService.Notification.Entities;

namespace Intel.MsoAuto.C3.MailService.Notification {
    public static class MailNotificationExtention {
        public static bool ExceptionEmailNotification(this Exception? ex, 
                                                      string source, 
                                                      Configurations? configs = null)
        {
            bool ret = false;
            if (configs != null)
            {
                configs.MergeNotificationSettings();
                if (ex != null && configs.SendEmail.HasValue && configs.SendEmail.Value)
                {
                    ret = MailNotification<FailureNotificationDatum>.ExceptonEmailNotification(ex.InnerException != null ? ex.InnerException : ex, source);
                }
            }
            else if (ex != null) 
                Shared.Functions.LogError(ex);
            return ret;
        }
    }
 }
