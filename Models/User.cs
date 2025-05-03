using System;
using System.Collections.Generic;  // Para List<T> e outras coleções

namespace User.Models
{
    public class User 
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        // Fields to reset password
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiration { get; set; }

        // Fields to time
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
   
        // Field to Goal 
        public double WeeklyGoal { get; set; } = 50; // Default Km 

        public List<Run> Runs { get; set; } = new List<Run>();
    }
}
