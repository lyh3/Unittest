namespace Intel.MsoAuto.C3.MailService.Notification.Core {
    public class PropertyListComparable<T> : IComparable<T> {
        public PropertyListComparable(T baseObject, params Func<T, object>[] propertySelectors)
        {
            if (baseObject == null)
                throw new ArgumentNullException(nameof(baseObject));

            BaseObject = baseObject;

            Comparer = new PropertyListComparer<T>(propertySelectors);
        }

        private PropertyListComparer<T> Comparer { get; }
        private T BaseObject { get; }
        public int CompareTo(T? other) => Comparer.Compare(BaseObject, other);
    }
}
