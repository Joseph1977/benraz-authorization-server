using Benraz.Infrastructure.Common.EntityBase;

namespace Authorization.Domain.Settings
{
    /// <summary>
    /// Settings entry.
    /// </summary>
    public class SettingsEntry : IEntity<string>
    {
        /// <summary>
        /// Settings entry key.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Settings entry value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Settings description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Creates settings entry.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="value">Value.</param>
        /// <param name="description">Description.</param>
        public SettingsEntry(string id, string value, string description = null)
            : this()
        {
            Id = id;
            Value = value;
            Description = description;
        }

        /// <summary>
        /// Creates settings entry.
        /// </summary>
        public SettingsEntry()
        {
        }
    }
}


