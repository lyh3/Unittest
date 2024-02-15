using Intel.MsoAuto.C3.MailService.Notification.Core;

namespace Intel.MsoAuto.C3.MailService.Notification.Entities {
    public class ARAssignedNotificationDatum : NotificationDatum, IComparable<ARAssignedNotificationDatum> {
        public ARAssignedNotificationDatum() : base()
        {
            thisComparer = new PropertyListComparable<ARAssignedNotificationDatum>(this,
            x => x.AR, x => x.ProjectName, x => x.ArId, x => x.ArStatus,
            x => x.ArAssignedTo, x => x.ArAssignedBy, x => x.ExpectedCompletionDate, x => x.Comments);
        }
        public ARAssignedNotificationDatum(NotificationArDetail source) : this()
        {
            ProjectId = source.projectId;
            ProjectProcessId = source.processId;
            Idsid = source.Idsid;
            if (null != source.id)
                ArId = source.id;
            ProjectName = source.projectName;
            if (null != source.assignedTo)
            {
                ArAssignedTo = source.assignedTo.name;
                UserName = source.assignedTo.name;
                Email = source.assignedTo.email;
            }
            if (null != source.assignedBy)
                ArAssignedBy = source.assignedBy.name;
            if (source.startDate.HasValue)
                ArStartDate = source.startDate.Value.ToString();
            ExpectedCompletionDate = source.expectedCompletionDate.ToString();
            if (source.startDate.HasValue)
                ArStartDate = source.startDate.Value.ToString();
            AR = source.task;
            ArStatus = source.status;
        }
        public string ArId { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public string AR { get; set; } = string.Empty;
        public string ArStatus { get; set; } = string.Empty;
        public string ArAssignedTo { get; set; } = string.Empty;
        public string ArAssignedBy { get; set; } = string.Empty;
        public string ArStartDate { get; set; } = string.Empty;
        public string ExpectedCompletionDate { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
        public bool IsArOverdue {
            get
            {
                if (!string.IsNullOrEmpty(ExpectedCompletionDate))
                {
                    if (DateTime.TryParse(ExpectedCompletionDate, out DateTime exprDate))
                    {
                        if (((DateTime.UtcNow - exprDate)).Days > 0)
                            return true;
                    }
                }
                return false;
            }
        }
        public int? ArDurationDays {
            get
            {
                if (!String.IsNullOrEmpty(ArStartDate))
                {
                    if (DateTime.TryParse(ArStartDate, out DateTime arStartDate))
                    {
                        DateTime myDate = new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Utc);//validation against 01/01/0001 00:00:00
                        if (arStartDate != myDate)
                            return ((DateTime.UtcNow - arStartDate)).Days;
                    }
                }
                return null;
            }
        }
        private PropertyListComparable<ARAssignedNotificationDatum> thisComparer { get; }
        public static Tuple<NotificationData<ARAssignedNotificationDatum>, NotificationData<ARAssignedOverdueNotificationDatum>>
                        TransformNotificationData(NotificationArDetails arDetails)
        {
            var arNotificationData = Tuple.Create(new NotificationData<ARAssignedNotificationDatum>(),
                                      new NotificationData<ARAssignedOverdueNotificationDatum>());
            foreach (var ar in arDetails)
            {
                ARAssignedNotificationDatum arNotificationDatum = new ARAssignedNotificationDatum(ar);
                if (arNotificationDatum.IsArOverdue)
                {
                    ARAssignedOverdueNotificationDatum d = new ARAssignedOverdueNotificationDatum(ar);
                    if (null == arNotificationData.Item2.FirstOrDefault(x => x.CompareTo(d) == 0))
                        arNotificationData.Item2.Add(d);
                }
                else
                {
                    if (null == arNotificationData.Item1.FirstOrDefault(x => x.CompareTo(arNotificationDatum) == 0))
                        arNotificationData.Item1.Add(arNotificationDatum);
                }
            }
            return arNotificationData;
        }
        public int CompareTo(ARAssignedNotificationDatum? other)
        {
            if (other == null) return -1;
            if (Comparer.CompareTo(other) == 0 && thisComparer.CompareTo(other) == 0)
                return 0;
            return -1;
        }
    }
}
