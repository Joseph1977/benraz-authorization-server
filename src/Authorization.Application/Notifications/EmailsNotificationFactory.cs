using Authorization.Domain.Emails;
using Authorization.Domain.Emails.Confirmation;
using Authorization.Domain.Emails.MfaCode;
using Authorization.Domain.Emails.ResetPassword;
using Authorization.Domain.Emails.UserLogin;
using RazorLight;
using System.Threading.Tasks;

namespace Authorization.Application.Notifications
{
    /// <summary>
    /// Emails notification factory.
    /// </summary>
    public class EmailsNotificationFactory : IEmailsNotificationFactory
    {
        /// <summary>
        /// Creates new confirmation notification.
        /// </summary>
        /// <param name="model">Confirmation email notification model.</param>
        /// <returns>Email body.</returns>
        public async Task<string> CreateNewConfirmationNotificationAsync(ConfirmationEmailModel model)
        {
            var engine = CreateEngine();
            string content = await engine.CompileRenderStringAsync(
                "Confirmation", EmailTemplates.Confirmation, model);

            return content;
        }

        /// <summary>
        /// Creates new reset password notification.
        /// </summary>
        /// <param name="model">Reset password email notification model.</param>
        /// <returns>Email body.</returns>
        public async Task<string> CreateNewResetPasswordNotificationAsync(ResetPasswordEmailModel model)
        {
            var engine = CreateEngine();
            string content = await engine.CompileRenderStringAsync(
                "ResetPassword", EmailTemplates.ResetPassword, model);
             
            return content;
        }

        /// <summary>
        /// Creates new mfa code notification.
        /// </summary>
        /// <param name="model">Mfa code email notification model.</param>
        /// <returns>Email body.</returns>
        public async Task<string> CreateMfaCodeNotificationAsync(MfaCodeEmailModel model)
        {
            var engine = CreateEngine();
            string content = await engine.CompileRenderStringAsync(
                "MfaCode", EmailTemplates.MfaCode, model);

            return content;
        }

        /// <summary>
        /// Creates new user login notification.
        /// </summary>
        /// <param name="model">User login email model.</param>
        /// <returns>Email body.</returns>
        public async Task<string> CreateNewUserLoginNotificationAsync(UserLoginEmailModel model)
        {
            var engine = CreateEngine();
            string content = await engine.CompileRenderStringAsync(
                "UserLogin", EmailTemplates.UserLogin, model);

            return content;
        }

        private IRazorLightEngine CreateEngine()
        {
            var engine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(typeof(EmailTemplates))
                .UseMemoryCachingProvider()
                .Build();

            return engine;
        }
    }
}