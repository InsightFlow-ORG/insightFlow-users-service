using System;

namespace userService.src.models
{
    /// <summary>
    /// Represents a member relationship for a user.
    /// </summary>
    public class AppUserMember
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        public Guid UserId { get; set; }
        
        public AppUser User { get; set; }
        
        public string MemberRole { get; set; } = string.Empty;
        
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}