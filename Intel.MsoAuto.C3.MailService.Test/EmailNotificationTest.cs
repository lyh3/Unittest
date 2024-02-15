using Intel.MsoAuto.C3.MailService.Notification.Entities;
using Intel.MsoAuto.C3.MailService.Notification;
using Intel.MsoAuto.C3.MailService.Notification.Services;

namespace Intel.MsoAuto.C3.MailService.Test {
    public class EmailNotificationTest {
        [Test]
        public void ArAssignedNotificationE2ETest()
        {
            NotificationArDetails arDetails = new NotificationMongoDbSevice().GetArAssigned();

            var arNotificationData = ARAssignedNotificationDatum.TransformNotificationData(arDetails);
            MailNotification<ARAssignedOverdueNotificationDatum>.DispatchNotification(arNotificationData.Item2);
            MailNotification<ARAssignedNotificationDatum>.DispatchNotification(arNotificationData.Item1);
        }
        [Test]
        public void BottlenecksE2ETest()
        {
            Dictionary<string, NotificationData<BottleneckNotificationDatum>> bottlenecksForecast = new NotificationMongoDbSevice().GetBottleneckForeCasts();
            foreach (KeyValuePair<string, NotificationData<BottleneckNotificationDatum>> keyVal in bottlenecksForecast)
            {
                MailNotification<BottleneckNotificationDatum>.DispatchNotification(keyVal.Value);
            }
        }
        [Test]
        public void WorkflowStatusE2ETest()
        {
            Dictionary<string, NotificationData<ProgressionStatusNotificationDatum>> workflowStatus = new NotificationMongoDbSevice().GetWorkflowStatus();
            foreach (KeyValuePair<string, NotificationData<ProgressionStatusNotificationDatum>> keyVal in workflowStatus)
            {
                MailNotification<ProgressionStatusNotificationDatum>.DispatchNotification(keyVal.Value);
            }
        }
    }
}
