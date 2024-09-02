using Authorization.Domain.Emails.Confirmation;
using Authorization.Domain.Emails.MfaCode;
using Authorization.Domain.Emails.ResetPassword;
using Authorization.Domain.Emails.UserLogin;
using Authorization.Domain.Users;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Benraz.Infrastructure.Emails;
using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Authorization.Domain.Emails
{
    /// <summary>
    /// Emails service.
    /// </summary>
    public class EmailsService : IEmailsService
    {
        private readonly IEmailsServiceProvider _emailsServiceProvider;
        private readonly IEmailsNotificationFactory _emailsNotificationFactory;
        private readonly ILogger<EmailsService> _logger;
        private readonly EmailsServiceSettings _settings;
        private readonly UserActionNotificationsServiceSettings _userActionNotificationsServiceSettings;

        /// <summary>
        /// Creates service.
        /// </summary>
        /// <param name="emailsServiceProvider">Emails service provider.</param>
        /// <param name="emailsNotificationFactory">Emails notification factory.</param>
        /// <param name="logger">Logger.</param>
        /// <param name="settings">Settings.</param>
        /// <param name="userActionNotificationsServiceSettings">User action notifications service settings.</param>
        public EmailsService(
            IEmailsServiceProvider emailsServiceProvider,
            IEmailsNotificationFactory emailsNotificationFactory,
            ILogger<EmailsService> logger,
            IOptions<EmailsServiceSettings> settings,
            IOptions<UserActionNotificationsServiceSettings> userActionNotificationsServiceSettings)
        {
            _emailsServiceProvider = emailsServiceProvider;
            _emailsNotificationFactory = emailsNotificationFactory;
            _logger = logger;
            _settings = settings.Value;
            _userActionNotificationsServiceSettings = userActionNotificationsServiceSettings.Value;
        }

        /// <summary>
        /// Sends email address confirmation email.
        /// </summary>
        /// <param name="parameters">Email address confirmation parameters.</param>
        /// <returns>Task.</returns>
        public async Task SendConfirmationEmailAsync(ConfirmationEmailParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            try
            {
                var model = new ConfirmationEmailModel
                {
                    UserFullName = parameters.UserFullName?.Split(' ')?.ToList().FirstOrDefault(),
                    ConfirmEmailLink = parameters.ConfirmEmailLink,
                    CompanyEmail = _settings.CompanyEmail,
                    CompanyName = _settings.CompanyName,
                    CompanyPhone = _settings.CompanyPhone,
                    CompanyLogoUrl = _settings.CompanyLogoUrl
                };

                var body = await _emailsNotificationFactory.CreateNewConfirmationNotificationAsync(model);

                var message = new MailMessage
                {
                    From = new MailAddress(_settings.From, _settings.FromDisplayName),
                    Subject = string.Format("{0} {1}", $"{model.UserFullName},", _settings.ConfirmEmailSubject),
                    Body = body,
                };
                message.To.Add(new MailAddress(parameters.To));

                var emailsService = _emailsServiceProvider.GetService();
                await emailsService.SendEmailAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while sending confirmation email to {parameters.To}.");
                throw;
            }
        }

        /// <summary>
        /// Sends reset password email.
        /// </summary>
        /// <param name="parameters">Reset password email parameters.</param>
        /// <returns>Task.</returns>
        public async Task SendResetPasswordEmailAsync(ResetPasswordEmailParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            try
            {
                var model = new ResetPasswordEmailModel()
                {
                    UserFullName = parameters.UserFullName.Split(' ')?.ToList().FirstOrDefault(),
                    ResetPasswordLink = parameters.ResetPasswordLink,
                    CompanyEmail = _settings.CompanyEmail,
                    CompanyName = _settings.CompanyName,
                    CompanyPhone = _settings.CompanyPhone,
                    CompanyLogoUrl = _settings.CompanyLogoUrl
                };

                var body = await _emailsNotificationFactory.CreateNewResetPasswordNotificationAsync(model);

                var message = new MailMessage
                {
                    From = new MailAddress(_settings.From, _settings.FromDisplayName),
                    Subject = _settings.ResetPasswordSubject,
                    Body = body
                };
                message.To.Add(new MailAddress(parameters.To));

                var emailsService = _emailsServiceProvider.GetService();
                await emailsService.SendEmailAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while sending reset password email to {parameters.To}.");
                throw;
            }
        }

        /// <summary>
        /// Send mfa code email.
        /// </summary>
        /// <param name="parameters">Mfa code email parameters.</param>
        /// <returns>Task.</returns>
        public async Task SendMfaCodeEmailAsync(MfaCodeEmailParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            try
            {
                var model = new MfaCodeEmailModel()
                {
                    UserFullName = parameters.UserFullName.Split(' ')?.ToList().FirstOrDefault(),
                    VerificationCode = parameters.VerificationCode,
                    VerificationCodeCooldown = parameters.VerificationCodeCooldown,
                    CompanyEmail = _settings.CompanyEmail,
                    CompanyName = _settings.CompanyName,
                    CompanyPhone = _settings.CompanyPhone,
                    CompanyLogoUrl = _settings.CompanyLogoUrl,
                    ActionTitle = parameters.ActionTitle,
                    ActionTitleLC = parameters.ActionTitle.ToLower()
                };
                
                var body = await _emailsNotificationFactory.CreateMfaCodeNotificationAsync(model);

                var message = new MailMessage
                {
                    From = new MailAddress(_settings.From, _settings.FromDisplayName),
                    Subject = string.Format(_settings.MfaCodeSubject, parameters.ActionTitle.ToLower()), 
                    Body = body
                };
                message.To.Add(new MailAddress(parameters.To));

                var emailsService = _emailsServiceProvider.GetService();
                await emailsService.SendEmailAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while sending mfa code email to {parameters.To}.");
                throw;
            }
        }

        /// <summary>
        /// Sends user login email.
        /// </summary>
        /// <param name="viewModel">User model.</param>
        /// <returns>Task.</returns>
        public async Task SendUserLoginEmailAsync(User viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            try
            {
                var model = new UserLoginEmailModel
                {
                    UserName = viewModel.UserName,
                    FullName = viewModel.FullName,
                    CreateTimeUtc = DateTime.UtcNow
                };

                var body = await _emailsNotificationFactory.CreateNewUserLoginNotificationAsync(model);

                var message = new MailMessage
                {
                    From = new MailAddress(_settings.From, _settings.FromDisplayName),
                    Subject = _userActionNotificationsServiceSettings.UserLoginEmailSubject,
                    Body = body,
                };

                if (!string.IsNullOrEmpty(_userActionNotificationsServiceSettings.ReceiversEmails))
                {
                    var receiversList = _userActionNotificationsServiceSettings.ReceiversEmails.Split(';');

                    if (receiversList.Length > 0)
                    {
                        foreach (var receiver in receiversList)
                        {
                            message.To.Add(new MailAddress(receiver));
                        }
                    }
                }

                var emailsService = _emailsServiceProvider.GetService();
                await emailsService.SendEmailAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while sending user login email to {_userActionNotificationsServiceSettings.ReceiversEmails}.");
                throw;
            }
        }
    }
}