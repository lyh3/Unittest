using Intel.MsoAuto.C3.MailService.Notification.Core;
using Intel.MsoAuto.C3.PITT.Business.Models;

namespace Intel.MsoAuto.C3.MailService.Notification.Entities {
    public class BottleneckNotificationDatum : NotificationDatum, IComparable<BottleneckNotificationDatum> {
        public BottleneckNotificationDatum()
        {
            thisComparer = new PropertyListComparable<BottleneckNotificationDatum>(this,
                            x => x.siteName, x => x.week, x => x.year,
                            x => x.createdBy, x => x.updatedBy);
        }
        public string siteName { get; set; } = string.Empty;
        public int? week { get; set; } = null;
        public int? year { get; set; } = null;
        public string workWeek { get; set; } = string.Empty;
        public string createdBy { get; set; } = string.Empty;
        public DateTime? createdOn { get; set; } = null;
        public string updatedBy { get; set; } = string.Empty;
        public DateTime? updatedOn { get; set; } = null;
        public BaseProject project { get; set; } = new BaseProject();
        private PropertyListComparable<BottleneckNotificationDatum> thisComparer { get; }
        public int CompareTo(BottleneckNotificationDatum? other)
        {
            if (other == null || project == null) return -1;
            if (base.Comparer.CompareTo(other) == 0
                && thisComparer.CompareTo(other) == 0
                && project.id == other.project.id)
                return 0;
            return -1;
        }
    }
}
