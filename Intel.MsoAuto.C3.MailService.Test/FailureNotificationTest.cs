using Intel.MsoAuto.C3.MailService.Notification;
using Intel.MsoAuto.C3.MailService.Notification.Core;

namespace Intel.MsoAuto.C3.MailService.Test {
    [SetCulture("en-US")]
    public class FailureNotificationTest {
        const string TEST_ERROR_MESSAGE = "PITT DailyTask: Main, Caught Exception!";
        const string TEST_ERROR_SOURCE = "ExceptionMailNotificationTest";
        static readonly Exception ex = new Exception(TEST_ERROR_MESSAGE);
        static readonly string exclusiveEmailType = Constants.FAILURE_NOTIFICATION;
        static readonly string emailTypeFilter = $"{Constants.CONFIGURATIONS}:{Constants.APP_SETTINGS}:{Constants.EMAIL_TYPE_EXCLUSIVE}";
        static readonly string defaultEnv = Constants.DEV;
        [Test] 
        public void RequiredAppSettingsValueValidation() 
        {
            const string propertyName = "Foo";
            const string propertyValue = "Bar";
            
            string c3CommonConnectionString = Settings.EnvConfig.GetRequiredAppSettingsValueValidation(Constants.C3_COMMON_CONNECTION_STRING);
            Assert.IsNotNull(c3CommonConnectionString);

            Constants.ENVIRONMENT.UpdateOrInsertAppSetting(Constants.INT);
            string env = Settings.Configuration.GetRequiredAppSettingsValueValidation(Constants.ENVIRONMENT);
            Assert.IsNotNull(env);
            Assert.That(env, Is.EqualTo(Constants.INT));

            propertyName.UpdateOrInsertAppSetting(propertyValue);
            string result = Settings.Configuration.GetRequiredAppSettingsValueValidation(propertyName);
            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo(propertyValue));
        }
        //[Ignore("Comment out to do the E2E test when needed.")]
        [Test, Order(0)]
        public void ExceptionMailNotificationWithEmailSendingTest()
        {
            Assert.IsTrue(InvokeTest(ex, TEST_ERROR_SOURCE));
        }
        /// <summary>
        /// This test case provids an example how to turn off the error notification email on client code
        /// by specify the paramter [sendEmail] to false.
        /// </summary>
        [Test]
        public void ExceptionMailNotificationNoEmailWithoutSendingTest()
        {
            Assert.IsFalse(ex.ExceptionEmailNotification(TEST_ERROR_SOURCE, new Configurations(environment:Constants.DEV, sendEmail:false)));
        }
        /// <summary>
        /// This test case demonstrate an option to skip email sending by set the configuration filter dynamically at runtime
        /// </summary>
        [Test]
        public void ExceptionMailNotificationSetSkipEmailOnFlyTest()
        {
            emailTypeFilter.UpsertAppSetting(exclusiveEmailType);
            Assert.That(Settings.Configuration.GetSection(emailTypeFilter).Value, Is.EqualTo(exclusiveEmailType));
            Assert.IsFalse(InvokeTest(ex, TEST_ERROR_SOURCE));
        }
        [Test]
        public void DefaultValuesOfConfigurationsTest()
        {
            Configurations config = new Configurations();
            Assert.IsTrue(config.SendEmail);
            Assert.That(config.Environment, Is.EqualTo(Constants.DEV));
        }
        [Test]
        public void ConfigurationsConstructorSetEnvironmentWithoutSendingEmailTest()
        {
            string env = Constants.PROD;
            Configurations config = new Configurations(environment: env, sendEmail:false);
            Assert.IsFalse(config.SendEmail);
            // --- verify the configuration settings of the "Environment" = default settings bfore invoking the notification 
            Assert.That(Settings.Configuration.GetSection(Constants.ENVIRONMENT).Value, Is.EqualTo(defaultEnv));
            Assert.IsFalse(ex.ExceptionEmailNotification(TEST_ERROR_SOURCE, config));
            // --- verify the configuration settings of the "Environment" = env after invoked the notification
            Assert.That(Settings.Configuration.GetSection(Constants.ENVIRONMENT).Value, Is.EqualTo(env));
        }
        private static Func<Exception, string, bool> InvokeTest = (x, s) => x.ExceptionEmailNotification(s, new Configurations(environment: defaultEnv));
    }
}
