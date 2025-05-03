using System;
using System.Linq;
using System.Security.Cryptography;

namespace User.Utils
{
    public static class PasswordResetToken
    {
        public static string GeneratePasswordResetToken(User.Models.User user)
        {
            var rng = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            string token = new string(Enumerable.Range(0, 6)
                .Select(_ => chars[rng.Next(chars.Length)])
                .ToArray());

            return token;
        }
    }
}