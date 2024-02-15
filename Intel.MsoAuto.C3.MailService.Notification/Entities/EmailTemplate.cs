namespace Intel.MsoAuto.C3.MailService.Notification.Entities
{
    public class EmailTemplate : IEmailTemplate
    {
        public int EmailTypeId { get; set; }
        public string TemplateName { get; set; } = string.Empty;
        public string TemplateText { get; set; } = string.Empty;
        public string TemplateDataFormat { get; set; } = string.Empty;
        public string LanguageCode { get; set; } = string.Empty;
        public string EmailSubject { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
