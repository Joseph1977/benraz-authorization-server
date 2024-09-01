namespace Authorization.Domain.Users
{
    /// <summary>
    /// Register user model.
    /// </summary>
    public class RegisterUserModel
    {
        /// <summary>
        /// User name.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// First name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Phone number.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Send confirmation email, if false, 
        /// server will not send the confirmation email to user.
        /// </summary>
        public bool SendConfirmationEmail { get; set; }
    }
}


