using Intel.MsoAuto.Shared.Security;
using System.Text.Json;

namespace Intel.MsoAuto.C3.PITT.Api.Core
{

    public class EncryptedAzureAD
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string TenantId { get; set; }
    }

    public class EncryptedMetadata
    {
        public string ConnectionString { get; set; }
        public EncryptedAzureAD AzureAD { get; set; }

    }

    public class CustomConfigurationSource : IConfigurationSource
    {
        private readonly string key;

        public CustomConfigurationSource(string key)
        {
            this.key = key;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new CustomConfigurationProvider(key);
        }
    }

    public class CustomConfigurationProvider : Microsoft.Extensions.Configuration.ConfigurationProvider
    {
        private readonly string key;

        public CustomConfigurationProvider(string key)
        {
            this.key = key;
        }

        public override void Load()
        {
            if (File.Exists(@"encrypted.json"))
            {

                Cryptographer.CryptographyType ct = Cryptographer.CryptographyType.CBC;
                string text = File.ReadAllText(@"encrypted.json");

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                EncryptedMetadata? content = JsonSerializer.Deserialize<EncryptedMetadata>(text, options);
                if (content != null)
                {
                    Data = new Dictionary<string, string>
                    {
                        {"Configurations:PittDatabase:ConnectionString",  content.ConnectionString.Decrypt(key, ct) },
                        {"AzureAd:ClientId",  content.AzureAD.ClientId.Decrypt(key, ct) },
                        {"AzureAd:ClientSecret", content.AzureAD.ClientSecret.Decrypt(key, ct) },
                        {"AzureAd:TenantId", content.AzureAD.TenantId.Decrypt(key, ct) }
                    };

                };
            }

        }
    }


    public static class CustomConfigurationExtensions
    {
        public static IConfigurationBuilder AddEncryptedConfiguration(this IConfigurationBuilder builder, string key)
        {
            return builder.Add(new CustomConfigurationSource(key));
        }
    }

}