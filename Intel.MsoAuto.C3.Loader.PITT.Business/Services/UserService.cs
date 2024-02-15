﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intel.MsoAuto.C3.Loader.PITT.Business.DataContext;
using Intel.MsoAuto.C3.Loader.PITT.Business.Services.interfaces;
using Intel.MsoAuto.C3.MailService.Notification.Entities;
using Intel.MsoAuto.C3.MailService.Notification;
using Intel.MsoAuto.C3.MailService.Notification.Core;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Services
{
    public class UserService: IUserService
    {
        private readonly UserDataContext dc;
        private string? _env = null;
        public UserService(string? env)
        {
            dc = new UserDataContext();
            _env = env;
        }

        public void SyncUserDataToMongo()
        {
            try
            {
                dc.SyncUserDataToMongo();
            }
            catch (Exception ex)
            {
                ex.ExceptionEmailNotification(GetType().Name, new Configurations(environment: _env, sendEmail: true));
            }
        }
    }
}
