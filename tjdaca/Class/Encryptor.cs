using System.Security.Cryptography;

namespace tjdaca.Class
{
    public class Encryptor
    {
        public static string CreateHash(string password)
        {
            int iterations = 12000;
            int saltSize = 24;
            int hashSize = 24;

            byte[] salt = GenerateSalt(saltSize);
            byte[] hashBytes = GeneratePBKDF2(password, salt, iterations, hashSize);

            string prefix = "sha256"; // You can change this to match the desired hash algorithm
            return $"{prefix}:{iterations}:{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hashBytes)}";
        }

        public static byte[] GenerateSalt(int size)
        {
            byte[] salt = new byte[size];
            using (RNGCryptoServiceProvider rng = new())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public static byte[] GeneratePBKDF2(string password, byte[] salt, int iterations, int hashSize)
        {
            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                return pbkdf2.GetBytes(hashSize);
            }
        }

        public static bool ValidatePassword(string password, string hash)
        {
            string[] parts = hash.Split(':');
            if (parts.Length < 4)
            {
                return false;
            }
            Console.WriteLine(parts);
            string prefix = parts[0];
            int iterations = int.Parse(parts[1]);
            byte[] salt = Convert.FromBase64String(parts[2]);
            byte[] storedHash = Convert.FromBase64String(parts[3]);

            byte[] computedHash = GeneratePBKDF2(password, salt, iterations, storedHash.Length);
            Console.WriteLine("StoredHash : " + Convert.ToBase64String(storedHash));
            Console.WriteLine("computedHash : " + Convert.ToBase64String(computedHash));
            return SlowEquals(storedHash, computedHash);
        }

        static bool NeedsUpgrade(string hash)
        {
            string[] parts = hash.Split(':');
            if (parts.Length < 4)
            {
                return true;
            }

            string algorithm = parts[0];
            int iterations = int.Parse(parts[1]);

            // You can modify the comparison logic to check for the desired algorithm and iteration count
            return !(algorithm == "sha256" && iterations >= 12000);
        }

        static bool SlowEquals(byte[] a, byte[] b)
        {
            int diff = a.Length ^ b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= a[i] ^ b[i];
            }
            return diff == 0;
        }
    }
}
