namespace Intel.MsoAuto.C3.MailService.Notification.Entities
{
    public interface IEmailTemplate
    {
        string EmailSubject { get; set; }
        int EmailTypeId { get; set; }
        bool IsActive { get; set; }
        string LanguageCode { get; set; }
        string TemplateDataFormat { get; set; }
        string TemplateName { get; set; }
        string TemplateText { get; set; }
    }
}