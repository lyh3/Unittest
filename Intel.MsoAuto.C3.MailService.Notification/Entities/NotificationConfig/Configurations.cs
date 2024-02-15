using Newtonsoft.Json;

namespace Intel.MsoAuto.C3.MailService.Notification.Core {
    public class Configurations {
        public Configurations(string? environment = null, bool? sendEmail = null)
        { 
            if (environment != null)
                Environment = environment;
            if (sendEmail.HasValue)
                SendEmail = sendEmail.Value;
        }
        [JsonIgnore]
        public string ? Environment { get; set; } = Constants.DEV;
        [JsonIgnore]
        public bool? SendEmail { get; set; } = true;
        public AppSettings? AppSettings { get; set; } = new AppSettings();
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
