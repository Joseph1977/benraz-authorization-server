namespace Authorization.WebApi.Services
{
    /// <summary>
    /// Masking data service.
    /// </summary>
    public interface IMaskingDataService
    {
        /// <summary>
        /// Hides secret.
        /// </summary>
        /// <param name="secret">Secret.</param>
        /// <returns>Masked secret.</returns>
        string HideSecret(string secret);

        /// <summary>
        /// Hides secret.
        /// </summary>
        /// <param name="secret">Secret.</param>
        /// <param name="keepChars">Keep chars.</param>
        /// <param name="placeholders">Placeholders.</param>
        /// <returns>Masked secret.</returns>
        string HideSecret(string secret, int keepChars, string placeholders);
    }
}


