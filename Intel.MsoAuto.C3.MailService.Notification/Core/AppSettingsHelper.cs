using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Intel.MsoAuto.C3.MailService.Notification.Core {
    public static class AppSettingsHelper {
        /// <summary>
        /// To merge email notification configuration section into appsettings.json dynamically at runtime
        /// without modifying the json configure file in client code.
        /// </summary>
        /// <param name="configurations"></param>
        public static void MergeNotificationSettings(this Configurations? configurations)
        {
            try
            {
                string? filePath = Path.Combine(AppContext.BaseDirectory, Constants.APP_SETTINGS_JSON);
                string? jsonSource = File.ReadAllText(filePath);
                string? jsonDestination = string.Empty;
                string? emailExcludSettings = Settings.Configuration.GetSection(Settings.EmailExcludeConfigurationPath).Value;
                if (!string.IsNullOrEmpty(jsonSource))
                {
                    JObject sourceObj = JObject.Parse(jsonSource);
                    if (sourceObj != null)
                    {
                        if (configurations != null)
                        {
                            string? env = configurations.Environment;
                            jsonDestination = Constants.ENVIRONMENT.UpdateProperty(jsonSource, env);
                            jsonDestination = Constants.CONFIGURATIONS.UpdateProperty(jsonDestination, configurations.ToString());
                            File.WriteAllText(filePath, jsonDestination);
                            if (configurations.Environment != null)
                                Constants.ENVIRONMENT.UpsertAppSetting(configurations.Environment);
                            if (configurations.Environment != null)
                                Constants.ENVIRONMENT.UpdateOrInsertAppSetting(configurations.Environment);
                        }
                    }
                    Settings.EmailExcludeConfigurationPath.UpsertAppSetting(emailExcludSettings);
                }
            }
            catch (Exception ex)
            {
                Shared.Functions.LogError(ex);
            }
        }
    }
}
