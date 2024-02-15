﻿using Intel.MsoAuto.C3.Loader.CCSapHana.Business.DataContext;
using Intel.MsoAuto.C3.MailService.Notification;
using Intel.MsoAuto.C3.MailService.Notification.Core;
using System;

namespace Intel.MsoAuto.C3.Loader.CCSapHana.Business.Services
{
    public class ParentCEIDtoCEIDMappingService : IParentCEIDtoCEIDMappingService
    {
        public bool SyncParentCEIDtoCEIDMappingData()
        {
            try
            {
                ParentCEIDtoCEIDMappingDataContext dc = new ParentCEIDtoCEIDMappingDataContext();
                return dc.SyncParentCEIDtoCEIDMappingData();
            }
            catch (Exception ex)
            {
                string? env = Core.Settings.Config.GetRequiredAppSettingsValueValidation(Constants.ENVIRONMENT);
                ex.ExceptionEmailNotification($"Intel.MsoAuto.C3.Loader.CCSapHana.Business -{GetType().Name}", new Configurations(environment: env, sendEmail: true));
                return false;
            }
        }
    }
}
