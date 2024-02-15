using Microsoft.Extensions.Configuration;

namespace Intel.MsoAuto.C3.MailService.Notification.Core {
    static public class Settings {
        private static IConfiguration _configuration => new ConfigurationBuilder()
                                                            .SetBasePath(AppContext.BaseDirectory)
                                                            .AddJsonFile(Constants.APP_SETTINGS_JSON,
                                                            optional: false,
                                                            reloadOnChange: true).Build();
        public static IConfiguration Configuration { get { return _configuration; } }
        public static IConfigurationSection EnvConfig => _configuration.GetSection($"{Constants.CONFIGURATIONS}:{Constants.APP_SETTINGS}:{_configuration.GetSection(Constants.ENVIRONMENT).Value}");
        public static readonly string EmailExcludeConfigurationPath = $"{Constants.CONFIGURATIONS}:{Constants.APP_SETTINGS}:{Constants.EMAIL_TYPE_EXCLUSIVE}";
    }
}
