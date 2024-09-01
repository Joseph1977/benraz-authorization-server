namespace Authorization.Infrastructure.Jwt
{
    /// <summary>
    /// Auth parameters view model.
    /// </summary>
    public class AuthParameters
    {
        /// <summary>
        /// Key set.
        /// </summary>
        public string KeySet { get; set; }

        /// <summary>
        /// Issuer.
        /// </summary>
        public string Issuer { get; set; }
    }
}


