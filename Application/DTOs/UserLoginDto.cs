namespace Application.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) used for user login requests.
    /// </summary>
    public class UserLoginDto
    {
        /// <summary>
        /// Username of the user attempting to log in.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Plain-text password provided by the user.
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
