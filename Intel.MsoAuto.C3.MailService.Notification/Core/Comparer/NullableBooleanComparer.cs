namespace Intel.MsoAuto.C3.MailService.Notification.Core {
    public class NullableBooleanComparer : IComparer<bool?> {
        public int Compare(bool? x, bool? y)
        {
            return Convert.ToByte(x) ^ Convert.ToByte(y);
        }
    }
}
