namespace Intel.MsoAuto.C3.MailService.Notification.Entities {
    public class NotificationData<U> : List<U> {
        private Dictionary<string, EmailSendTo<U>> _emailSentToMap = new Dictionary<string, EmailSendTo<U>>();
        public NotificationData()
        {
            NotificationTemplateName = $"{typeof(U).Name.Replace("Datum", "Template")}";
        }
        public string NotificationTemplateName { get; set; }
        public Dictionary<string, EmailSendTo<U>> EmailSendTo {
            get
            {
                PopulateMap();
                return _emailSentToMap;
            }
            set
            {
                _emailSentToMap = value;
            }
        }
        public dynamic? GetMetadataValue(KeyValuePair<string, string> keyVal, string lookupField)
        {
            dynamic? result = null;
            dynamic? item = null;
            foreach (var o in this)
            {
                if (null != item)
                    break;
                if (null != o)
                {
                    foreach (var p in o.GetType().GetProperties())
                    {
                        if (p.Name == keyVal.Key && p.GetValue(o) == keyVal.Value)
                        {
                            item = o;
                            break;
                        }
                    }
                }
            }
            if (null != item)
                foreach (var p in item.GetType().GetProperties())
                {
                    if (p.Name == lookupField)
                    {
                        result = p.GetValue(item);
                        break;
                    }
                }
            return result;
        }
        public bool Lookup(dynamic? datum)
        {
            bool found = false;
            if (null != datum)
            {
                foreach(dynamic? item in this)
                {
                    if (item !=  null)
                    {
                        found |= item.CompareTo(datum) == 0;
                        if (found)
                            break;
                    }
                }
            }
            return found;
        }
        private void PopulateMap()
        {
            if (this.Count > 0 && _emailSentToMap.Count == 0)
            {
                foreach (var item in this)
                {
                    var datum = item as NotificationDatum;
                    if (null != datum)
                    {
                        if (!_emailSentToMap.Keys.Contains(datum.Email))
                            _emailSentToMap.Add(datum.Email, new EmailSendTo<U>());
                        var emailSendTo = _emailSentToMap[datum.Email] as EmailSendTo<U>;
                        emailSendTo.SendTo = datum.Email;
                        emailSendTo.UserName = datum.UserName;
                        emailSendTo.Idsid = datum.Idsid;
                        emailSendTo.Content.Add(item);
                    }
                }
            }
        }
    }
}
