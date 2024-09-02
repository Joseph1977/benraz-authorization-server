namespace Authorization.WebApi.Services
{
    /// <summary>
    /// Masking data service.
    /// </summary>
    public class MaskingDataService : IMaskingDataService
    {
        /// <summary>
        /// Hides secret.
        /// </summary>
        /// <param name="secret">Secret.</param>
        /// <returns>Masked secret.</returns>
        public string? HideSecret(string? secret)
        {
            return ApplyMasking(secret);
        }

        /// <summary>
        /// Hides secret.
        /// </summary>
        /// <param name="secret">Secret.</param>
        /// <param name="keepChars">Keep chars.</param>
        /// <param name="placeholders">Placeholders.</param>
        /// <returns>Masked secret.</returns>
        public string? HideSecret(string? secret, int keepChars, string? placeholders)
        {
            return ApplyMasking(secret, keepChars, placeholders);
        }

        private string? ApplyMasking(string? secret, int keepChars = 3, string? placeholders = "*****")
        {
            if (string.IsNullOrEmpty(secret))
            {
                return secret;
            }

            if (secret.Length <= keepChars)
            {
                return secret;
            }

            return secret.Substring(0, keepChars) + placeholders;
        }
    }
}


