using System.Security.Cryptography;
using System.Text;

namespace User.Utils
{
    public static class PasswordHelper
    {

        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 100_000;

        public static string HashPassword(string password)
        {
            using var rng = RandomNumberGenerator.Create();
            var salt = new byte[SaltSize];
            rng.GetBytes(salt);

            var key = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            var hash = key.GetBytes(KeySize);

            var base64Salt = Convert.ToBase64String(salt);
            var base64Hash = Convert.ToBase64String(hash);

            return $"{Iterations}.{base64Salt}.{base64Hash}";
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            var parts = hashedPassword.Split('.', 3);
            if (parts.Length != 3) return false;

            var iterations = int.Parse(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var hash = Convert.FromBase64String(parts[2]);

            var key = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
            var hashToCompare = key.GetBytes(KeySize);

            return CryptographicOperations.FixedTimeEquals(hash, hashToCompare);
        }
    }
}
