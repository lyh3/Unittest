using Intel.MsoAuto.C3.MailService.Notification.DataContext;
using Intel.MsoAuto.C3.MailService.Notification.Entities;

namespace Intel.MsoAuto.C3.MailService.Notification.Services {
    public class NotificationMongoDbSevice : INotificationMongoDbSevice {
        public NotificationArDetails GetArAssigned()
        {
            return new NotificationMongoDbContext().GetArAssigned();
        }
        public async Task<NotificationArDetails> GetArAssignedAsync()
        {
            return await new NotificationMongoDbContext().GetArAssignedAsync();
        }
        public Dictionary<string, NotificationData<BottleneckNotificationDatum>> GetBottleneckForeCasts()
        {
            return new NotificationMongoDbContext().GetBottleneckforeCasts();
        }
        public async Task<Dictionary<string, NotificationData<BottleneckNotificationDatum>>> GetBottleneckForeCastsAsync()
        {
            return await new NotificationMongoDbContext().GetBottleneckForeCastsAsyc();
        }    
        public Dictionary<string, NotificationData<ProgressionStatusNotificationDatum>> GetWorkflowStatus()
        {
            return new NotificationMongoDbContext().GetWorkflowStatus();
        }
        public async Task<Dictionary<string, NotificationData<ProgressionStatusNotificationDatum>>> GetWorkflowStatusAsync()
        {
            return await new NotificationMongoDbContext().GetWorkflowStatusAsync();
        }      }
}
