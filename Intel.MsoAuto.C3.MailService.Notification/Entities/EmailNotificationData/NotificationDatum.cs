using Intel.MsoAuto.C3.MailService.Notification.Core;

namespace Intel.MsoAuto.C3.MailService.Notification.Entities {
    public class NotificationDatum : IComparable<NotificationDatum> {
        public NotificationDatum()
        {
            Comparer = new PropertyListComparable<NotificationDatum>(this,
                x => x.id, x => x.ProjectId, x => x.UserName, x => x.Email,
                x => x.Wwid, x => x.Idsid, x => x.ProjectProcessId);
        }
        public string id { get; set; } = string.Empty;
        public string ProjectId { get; set; } = string.Empty;
        public string ProjectProcessId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Wwid { get; set; } = string.Empty;
        public string Idsid { get; set; } = string.Empty;
        protected PropertyListComparable<NotificationDatum> Comparer { get; }
        public int CompareTo(NotificationDatum? other) => Comparer.CompareTo(other);
    }
}
