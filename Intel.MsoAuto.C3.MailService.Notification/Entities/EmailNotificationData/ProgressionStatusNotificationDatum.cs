using Intel.MsoAuto.C3.MailService.Notification.Core;
using Intel.MsoAuto.C3.PITT.Business.Models;

namespace Intel.MsoAuto.C3.MailService.Notification.Entities {
    public class ProgressionStatusNotificationDatum : NotificationDatum, IComparable<ProgressionStatusNotificationDatum> {
        public ProgressionStatusNotificationDatum() : base()
        {
            ThisComparer = new PropertyListComparable<ProgressionStatusNotificationDatum>(this,
                        x => x.ProgressionStatus, x => x.StateId);
        }
        public string ProgressionStatus { get; set; } = string.Empty;
        public string StateId { get; set; } = string.Empty;
        public BaseProject project { get; set; } = null;
        private PropertyListComparable<ProgressionStatusNotificationDatum> ThisComparer { get; }
        public int CompareTo(ProgressionStatusNotificationDatum? other)
        {
            if (other == null || project == null) return -1;
            if (base.CompareTo(other) == 0 && ThisComparer.CompareTo(other) == 0
                && project.id == other.project.id)
                return 0;
            return -1;
        }
    }
}
