using System.Collections;

namespace Intel.MsoAuto.C3.MailService.Notification.Core {
    public class PropertyListComparer<T> : IComparer<T> {
        public PropertyListComparer(params Func<T, object>[] propertySelectors)
        {
            ComparedProperties = propertySelectors ?? throw new ArgumentNullException(nameof(propertySelectors));
        }

        private Func<T, object>[] ComparedProperties { get; }

        public int Compare(T? x, T? y)
        {
            if (x == null)
            {
                if (y == null)
                    return 0;
                else
                    return -1;
            }

            foreach (var propertySelector in ComparedProperties)
            {
                var xValue = propertySelector(x);
                var yValue = propertySelector(y);

                var result = Comparer.Default.Compare(xValue, yValue);

                if (result != 0)
                    return result;
            }

            return 0;
        }
    }
}

