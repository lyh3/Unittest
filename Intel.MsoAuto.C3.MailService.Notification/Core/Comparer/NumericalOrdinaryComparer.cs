using System.Text.RegularExpressions;

namespace Intel.MsoAuto.C3.MailService.Notification.Core {
    public class NumericalOrdinaryComparer : IComparer<String> {
        public int Compare(string? x, string? y)
        {
            const string PATTERN = "[0-9]";
            if (!(string.IsNullOrEmpty(x) || string.IsNullOrEmpty(y))
                && Regex.Match(x, PATTERN).Success && Regex.Match(y, PATTERN).Success)
            {
                return Convert.ToInt32(x).CompareTo(Convert.ToInt32(y));
            }
            return 0;
        }
    }
}
