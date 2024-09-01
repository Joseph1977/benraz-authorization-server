using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Benraz.Infrastructure.Gateways;
using Benraz.Infrastructure.Gateways.BenrazServices;
using Benraz.Infrastructure.Gateways.BenrazServices.Messages;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Authorization.Domain.Emails
{
    /// <summary>
    /// Emails service.
    /// </summary>
    public class EmailsService : IEmailsService
    {
        private readonly IBenrazServicesGateway _BenrazServicesGateway;
        private readonly ILogger<EmailsService> _logger;
        private readonly EmailsServiceSettings _settings;

        /// <summary>
        /// Creates service.
        /// </summary>
        /// <param name="BenrazServicesGateway">Benraz services gateway.</param>
        /// <param name="logger">Logger.</param>
        /// <param name="settings">Settings.</param>
        public EmailsService(
            IBenrazServicesGateway BenrazServicesGateway,
            ILogger<EmailsService> logger,
            IOptions<EmailsServiceSettings> settings)
        {
            _BenrazServicesGateway = BenrazServicesGateway;
            _logger = logger;
            _settings = settings.Value;
        }

        /// <summary>
        /// Sends email address confirmation email.
        /// </summary>
        /// <param name="model">Email address confirmation email.</param>
        /// <returns>Task.</returns>
        public async Task SendConfirmationEmailAsync(ConfirmationEmailModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var request = CreateEmailRequest(
                model.To, _settings.ConfirmEmailSubject, _settings.ConfirmEmailTemplateId);

            request.EmailParams = new Dictionary<string, string>();
            request.EmailParams.Add("-RootBannerUrl-", _settings.CompanyLogoUrl);
            request.EmailParams.Add("-corporateName-", _settings.CompanyName);
            request.EmailParams.Add("-userName-", model.UserFullName);
            request.EmailParams.Add("-ActionLink-", model.ConfirmEmailLink);
            request.EmailParams.Add("-corporateEmail-", _settings.CompanyEmail);
            request.EmailParams.Add("-corporateNumber-", _settings.CompanyPhone);

            await SendEmailRequestAsync(request);
        }

        /// <summary>
        /// Sends reset password email.
        /// </summary>
        /// <param name="model">Reset password email model.</param>
        /// <returns>Task.</returns>
        public async Task SendResetPasswordEmailAsync(ResetPasswordEmailModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var request = CreateEmailRequest(
                model.To, _settings.ResetPasswordSubject, _settings.ResetPasswordTemplateId);

            request.EmailParams = new Dictionary<string, string>();
            request.EmailParams.Add("-RootBannerUrl-", _settings.CompanyLogoUrl);
            request.EmailParams.Add("-corporateName-", _settings.CompanyName);
            request.EmailParams.Add("-userName-", model.UserFullName);
            request.EmailParams.Add("-ActionLink-", model.ResetPasswordLink);
            request.EmailParams.Add("-corporateEmail-", _settings.CompanyEmail);
            request.EmailParams.Add("-corporateNumber-", _settings.CompanyPhone);

            await SendEmailRequestAsync(request);
        }

        private EmailRequest CreateEmailRequest(string to, string subject, string templateId)
        {
            var request = new EmailRequest
            {
                Username = _settings.Username,
                Password = _settings.Password,
                BasicInfo = new EmailBasicInfo
                {
                    From = _settings.From,
                    DisplayName = _settings.FromDisplayName,
                    To = to,
                    Subject = subject,
                    TemplateId = templateId,
                    SkipOptOutCheck = true
                },
            };

            return request;
        }

        private async Task SendEmailRequestAsync(EmailV2Request request)
        {
            try
            {
                var response = await _BenrazServicesGateway.SendAsync(request);
                if (response.HttpStatusCode != (int)HttpStatusCode.OK)
                {
                    throw new InvalidOperationException(CreateResponseErrorMessage(response));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending email.");
                throw;
            }
        }

        private string CreateResponseErrorMessage(HttpResponseBase response)
        {
            var errorMessage =
                $"Unable to send email - response does not indicate success. Status code: {response.HttpStatusCode}.";

            if (!string.IsNullOrEmpty(response.HttpContentString))
            {
                errorMessage += $" Message: {response.HttpContentString}.";
            }

            return errorMessage;
        }
    }
}


