using Intel.MsoAuto.C3.MailService.Notification.Entities;

namespace Intel.MsoAuto.C3.MailService.Notification.Services {
    public interface INotificationMongoDbSevice {
        NotificationArDetails GetArAssigned();
        Task<NotificationArDetails> GetArAssignedAsync();
        Dictionary<string, NotificationData<BottleneckNotificationDatum>> GetBottleneckForeCasts();
        Task<Dictionary<string, NotificationData<BottleneckNotificationDatum>>> GetBottleneckForeCastsAsync();
    }
}