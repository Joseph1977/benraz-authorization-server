using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Authorization.Domain.SsoServices
{
    /// <summary>
    /// SSO state.
    /// </summary>
    public class SsoState
    {
        private const string APPLICATION_ID_KEY = "applicationId";
        private const string RETURN_URL_KEY = "returnUrl";

        /// <summary>
        /// Application identifier.
        /// </summary>
        public Guid ApplicationId { get; set; }

        /// <summary>
        /// Return URL.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Converts SSO state object to string.
        /// </summary>
        /// <returns>SSO state string.</returns>
        public override string ToString()
        {
            var values = new Dictionary<string, string>();
            values.Add(APPLICATION_ID_KEY, ApplicationId.ToString());
            values.Add(RETURN_URL_KEY, ReturnUrl);

            var valuePairs = new List<string>();
            foreach(var value in values)
            {
                if (!string.IsNullOrEmpty(value.Value))
                {
                    valuePairs.Add($"{value.Key}={value.Value}");
                }
            }

            var state = string.Join("&", valuePairs);
            return WebUtility.UrlEncode(state);
        }

        /// <summary>
        /// Creates SSO state from string.
        /// </summary>
        /// <param name="state">SSO state string.</param>
        /// <returns>SSO state.</returns>
        public static SsoState FromString(string state)
        {
            if (string.IsNullOrEmpty(state))
            {
                throw new ArgumentNullException(nameof(state));
            }

            var decodedState = WebUtility.UrlDecode(state);
            var valuePairs = decodedState.Split('&');

            var values = new Dictionary<string, string>();
            foreach(var valuePair in valuePairs)
            {
                var valuePairParts = valuePair.Split('=').ToArray();
                if (valuePairParts.Length != 2)
                {
                    continue;
                }

                values.Add(valuePairParts[0], valuePairParts[1]);
            }

            var ssoState = new SsoState
            {
                ApplicationId = ExtractApplicationId(values),
                ReturnUrl = ExtractReturnUrl(values)
            };
            return ssoState;
        }

        private static Guid ExtractApplicationId(IDictionary<string, string> values)
        {
            values.TryGetValue(APPLICATION_ID_KEY, out var applicationIdString);
            Guid.TryParse(applicationIdString, out var applicationId);
            return applicationId;
        }

        private static string ExtractReturnUrl(IDictionary<string, string> values)
        {
            values.TryGetValue(RETURN_URL_KEY, out var returnUrl);
            return returnUrl;
        }
    }
}


