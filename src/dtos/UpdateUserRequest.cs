namespace userService.src.dtos
{
    /// <summary>
    /// Request DTO for updating an existing user.
    /// </summary>
    public class UpdateUserRequest
    {
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}