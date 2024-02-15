using Apache.NMS.ActiveMQ;
using Apache.NMS;
using Intel.MsoAuto.C3.MailService.Notification.Entities;
using Microsoft.Extensions.Configuration;
using AAM_Dotnet;
using Intel.MsoAuto.C3.MailService.Notification.Core;
using Newtonsoft.Json;

namespace Intel.MsoAuto.C3.MailService.Notification {
    public class ActiveMessageQueue<T> : IDisposable {
        protected IConnection _connection;
        protected ISession _session;
        protected IMessageProducer _producer;
        protected IMessageConsumer _consumer;
        public ActiveMessageQueue()
        {
            try
            {
                string? host1 = _config.GetRequiredAppSettingsValueValidation(Constants.AMQ_HOST_1);
                string? host2 = _config.GetRequiredAppSettingsValueValidation(Constants.AMQ_HOST_2);
                string? port = _config.GetRequiredAppSettingsValueValidation(Constants.AMQ_PORT);
                string? username = _config.GetRequiredAppSettingsValueValidation(Constants.AMQ_USER_NAME);
                string? password = RetrievePassWordFromPamSafe(_config);
                string? connectString = string.Format(Constants.AMQ_CONNECTION_STRING_FORMAT, host1, host2, port);
                Uri connecturi = new Uri(connectString);
                ConnectionFactory connectionFactory = new ConnectionFactory(connecturi);
                _connection = connectionFactory.CreateConnection(username, password);
                _session = _connection.CreateSession(AcknowledgementMode.AutoAcknowledge);
                _connection.Start();
                _session = _connection.CreateSession(AcknowledgementMode.AutoAcknowledge);
                IDestination destination = this._session.GetQueue(_config[Constants.AMQ_TOPIC_NAME]);
                _producer = this._session.CreateProducer(destination);
                _producer.DeliveryMode = MsgDeliveryMode.NonPersistent;
                _consumer = _session.CreateConsumer(destination);
            }
            catch (Exception ex)
            {
                Shared.Functions.LogMyError(ex, this);
                throw;
            }
        }
        public ISession Session => _session;
        public IMessageConsumer Consumer => _consumer;
        private IConfigurationSection _config => Settings.EnvConfig;
        public void Dispose()
        {
            try
            {
                if (null != _producer)
                    _producer.Close();
                if (null != _consumer)
                    _consumer.Close();
                if (null != _connection)
                    _connection.Close();
            }
            catch (Exception ex) 
            { 
                Shared.Functions.LogMyError(ex, this);
                throw;
            }
        }
        public void SendMessageToQueue(string json, MailNotification<T> notification)
        {
            try
            {
                if (null != _session)
                {
                    EmailNotificationRequest<NotificationData<T>>? request = JsonConvert.DeserializeObject<EmailNotificationRequest<NotificationData<T>>>(json);
                    if (request != null)
                    {
                        string? linkWebUrl = _config.GetRequiredAppSettingsValueValidation(Constants.LINK_WEBSITE_URL);
                        if (linkWebUrl != null)
                            request.LinkWebsiteUrl = linkWebUrl;
                        else
                            throw new Exception($"The configuration [{Constants.LINK_WEBSITE_URL}] is required.");
                        IEmailNotificationRequest<NotificationData<T>> req = notification.SaveNotificationTracking(request, string.Empty, hasSent: null);
                        if ( req != null && req.NotificationData.Count > 0 )
                        {
                            ITextMessage message = CreateMessage(JsonConvert.SerializeObject(req));
                            _producer.Send(message);
                        }
                    }
                    else
                        throw new ArgumentException($"Failed to DeserializeObject with input json data {json}");
                }
            }
            catch (Exception ex)
            {
                Shared.Functions.LogMyError(ex, this);
                throw;
            }
        }
        protected ITextMessage CreateMessage(string json)
        {
            return _session.CreateTextMessage(json);
        }
        private string RetrievePassWordFromPamSafe(IConfigurationSection config)
        {
            string pwd = string.Empty;
            try
            {
                AAMConfiguration aamConfig = new AAMConfiguration {
                    appID = config[Constants.APP_ID],
                    safeName = config[Constants.SAFE_NAME],
                    certificateThumbprint = config[Constants.CERTIFICATE_THUMBPRINT],
                };
                AAMConsumer.ConfigureAAMConsumer(aamConfig);
                pwd = AAMConsumer.GetCredentials(config[Constants.AMQ_USER_NAME]);
            }
            catch (Exception ex)
            {
                Shared.Functions.LogMyError(ex, this);
                throw;
            }
            return pwd;
        }
    }
}
