namespace Domain.Entities
{
    /// <summary>
    /// Represents a system user who can access the Resume Scoring API.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Unique identifier for the user.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Username used for authentication.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Hashed password of the user.
        /// </summary>
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// Role of the user (e.g., "User" or "Admin").
        /// </summary>
        public string Role { get; set; } = "User";
    }
}
