namespace Intel.MsoAuto.C3.MailService.Notification.Core {
    public class DateTimeStringComparer : IComparer<String> {
        public int Compare(string? x, string? y)
        {
            if (!(string.IsNullOrEmpty(x) || string.IsNullOrEmpty(y)))
            {
                if (DateTime.TryParse(x, out DateTime dateX) && DateTime.TryParse(y, out DateTime dateY))
                    return dateX.CompareTo(dateY);
            }
            return 0;
        }
    }
}
