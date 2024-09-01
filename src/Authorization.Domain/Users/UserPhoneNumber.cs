using System.Linq;

namespace Authorization.Domain.Users
{
    /// <summary>
    /// User phone number.
    /// </summary>
    public static class UserPhoneNumber
    {
        /// <summary>
        /// Normalizes phone number removing all non-digit characters.
        /// </summary>
        /// <param name="phoneNumber">Phone number.</param>
        /// <returns>Normalized phone number.</returns>
        public static string NormalizePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return phoneNumber;
            }

            var normalizedPhoneNumber = new string(phoneNumber.Where(char.IsDigit).ToArray());
            return normalizedPhoneNumber;
        }
    }
}


