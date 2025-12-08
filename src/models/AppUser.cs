using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace userService.src.models
{
    /// <summary>
    /// Application user class extending IdentityUser with additional properties.
    /// </summary>
    public class AppUser : IdentityUser<Guid>
    {
        /// <summary>
        /// The user's first name.
        /// </summary>
        public string? Name { get; set; } = string.Empty;
        /// <summary>
        /// The user's last name.
        /// </summary>
        public string? LastName { get; set; } = string.Empty;
        /// <summary>
        /// The user's date of birth.
        /// </summary>
        public DateOnly DateOfBirth { get; set; }
        /// <summary>
        /// The user's address.
        /// </summary>
        public string Address { get; set; } = string.Empty;
        /// <summary>
        /// The user's phone number.
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// The unique identifier for the user (UUID V4).
        /// </summary>
        public override Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Indicates whether the user is active (1 = active, 0 = inactive).
        /// </summary>
        public int IsActive { get; set; } = 1;

        /// <summary>
        /// The date when the user was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The date when the user was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public List<AppUserMember> UserMembers { get; set; } = new List<AppUserMember>();

        /// <summary>
        /// The role assigned to the user.
        /// </summary>
        public string Role { get; set; } = "User";
    }
}