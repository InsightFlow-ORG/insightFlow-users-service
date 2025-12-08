using System;

namespace userService.src.dtos
{
    /// <summary>
    /// Response DTO for returning user information.
    /// </summary>
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public string Address { get; set; } = string.Empty;
        public int IsActive { get; set; }
    }
}