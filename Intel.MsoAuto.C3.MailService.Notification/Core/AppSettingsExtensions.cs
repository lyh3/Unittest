using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Intel.MsoAuto.C3.MailService.Notification.Core {
    public static class AppSettingsExtensions {
        const int RECURSIVE_LEVEL = 2;
        /// <summary>
        /// Get property values of appsetings.json with an IConfiguration instance
        /// </summary>
        /// <param name="configSection"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetRequiredAppSettingsValueValidation(this IConfiguration configSection, string key)
        {
            return RequiredAppSettingsValueValidation(configSection, key);
        }
        /// <summary>
        ///  Get property values of appsetings.json with an IConfigurationSection instance
        /// </summary>
        /// <param name="configSection"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetRequiredAppSettingsValueValidation(this IConfigurationSection configSection, string key)
        {
            return RequiredAppSettingsValueValidation(configSection, key);
        }
        /// <summary>
        /// Usage : To insert or update a config field value in an appsettings.json dynamically at runtime
        /// Limitations : Support the json path in 1 level deepth.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="val"></param>
        public static void UpdateOrInsertAppSetting(this string propertyName, string? val)
        {
            if (val == null)
                return;
            try
            {
                string? filePath = Path.Combine(AppContext.BaseDirectory, Constants.APP_SETTINGS_JSON);
                string? jsonSource = File.ReadAllText(filePath);
                string? jsonDistination = propertyName.UpdateProperty(jsonSource, val);
                File.WriteAllText(filePath, jsonDistination);
            }
            catch (Exception ex)
            {
                Shared.Functions.LogError(ex);
            }
        }
        /// <summary>
        /// To merge or insert configuration in to appsettings.json
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="source"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        internal static string? UpdateProperty(this string propertyName, string? source, string? val)
        {
            string? results = source;
            if (!string.IsNullOrEmpty(source) || string.IsNullOrEmpty(val))
            {
                JObject? sourceObj = source.TryParseJson();
                if (sourceObj != null)
                {
                    dynamic? jsonContentObj = val.TryParseJson();
                    if (jsonContentObj != null)
                    {
                        sourceObj.Remove(propertyName);
                        sourceObj.Add(new JProperty(propertyName, jsonContentObj));
                    }
                    results = sourceObj.ToString();
                }
            }
            return results;
        }
        /// <summary>
        /// Usage : To insert or update a config field value in an appsettings.json dynamically at runtime
        /// Limitations : Support the json path in 2 level deepth.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sectionPathKey"></param>
        /// <param name="value"></param>
        public static void UpsertAppSetting<T>(this string sectionPathKey, T value)
        {
            try
            {
                string filePath = Path.Combine(AppContext.BaseDirectory, Constants.APP_SETTINGS_JSON);
                string json = File.ReadAllText(filePath);
                dynamic? jsonObj = JsonConvert.DeserializeObject(json);
                if (jsonObj != null)
                {
                    SetValueRecursively(sectionPathKey, jsonObj, value);
                    string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                    File.WriteAllText(filePath, output);
                }
            }
            catch (Exception ex)
            {
                Shared.Functions.LogError(ex);
            }
        }
        private static void SetValueRecursively<T>(string sectionPathKey, dynamic jsonObj, T value)
        {
            if (jsonObj == null)
                return;
            var remainingSections = sectionPathKey.Split(":", RECURSIVE_LEVEL);
            var currentSection = remainingSections[0];
            if (remainingSections.Length > 1)
            {
                var nextSection = remainingSections[1];
                SetValueRecursively(nextSection, jsonObj[currentSection], value);
            }
            else
            {
                jsonObj[currentSection] = value;
            }
        }
        private static dynamic? TryParseJson(this string? json)
        {
            if (json != null && json.StartsWith("{"))
            {
                try
                {
                    return JObject.Parse(json);
                }
                catch (Exception ex) { Shared.Functions.LogError(ex); }
            }
            return json;
        }
        private static Func<dynamic, string, string> RequiredAppSettingsValueValidation = (x, s) => x.GetSection(s).Value ?? throw new Exception($"{s} {Constants.IS_REQUIRED}.");
    }
}
