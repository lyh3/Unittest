using Intel.MsoAuto.C3.PITT.Business.Models;

namespace Intel.MsoAuto.C3.MailService.Notification.Entities {
    public class NotificationArDetail : ActionRequestDetail {
        public string Idsid { get; set; } = string.Empty; //allow to look up emloyee manager from LADP
        public string AR { get; set;} = string.Empty;
        public string? Comments { get; set;} = string.Empty;
    }
}
