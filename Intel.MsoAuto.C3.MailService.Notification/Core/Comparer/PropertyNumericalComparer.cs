using System.Text.RegularExpressions;

namespace Intel.MsoAuto.C3.MailService.Notification.Core {
    public class PropertyNumericalComparer : IComparer<object> {
        string PropertyName { get; set; }
        public PropertyNumericalComparer(string propertyName)
        {
            PropertyName = propertyName;
        }
        public int Compare(object? x, object? y)
        {
            string PATTERN =  @"^([+-]?((\d+)|((\d+)(.)?(\d+))))$";
            var pX = x.GetType().GetProperties().FirstOrDefault(x => x.Name == PropertyName);
            var pY = y.GetType().GetProperties().FirstOrDefault(x => x.Name == PropertyName);
            if (!(null == pX || null == pY)
                && x.GetType() == y.GetType())
            {
                dynamic? vX = pX.GetValue(x);
                dynamic? vY = pY.GetValue(y);
                double dx = 0.0, dy = 0.0;
                if (vX != null && vX.GetType() == typeof(string))
                {
                    if (Regex.Match(vX.ToString(), PATTERN).Success && Regex.Match(vY.ToString(), PATTERN).Success)
                    {
                        dx = Convert.ToDouble(vX);
                        dy = Convert.ToDouble(vY);
                    }
                    else
                    {
                        int sX = vX.ToString().Length;
                        int sY = vY.ToString().Length;
                        return sX.CompareTo(sY);
                    }
                }
                else if (vX.GetType() == typeof(double) || vX.GetType() == typeof(decimal) || vX.GetType() == typeof(int))
                {
                    dx = (double)vX;
                    dy = (double)vY;
                }
                return dx.CompareTo(dy);
            }
            return 0;
        }
    }
}
