using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace UiPath.Samples.SecureStores.SqlPasswordStore
{
    public static class CryptoHelper
    {
        private const int Iterations = 2;
        private const int KeySize = 256;
        private const string Hash = "SHA1";
        private const string PassPhrase = "bug0jlvizywo2npz";
        private const string Salt = "q4dbh99wzv01lv2e";
        private const string Vector = "w8o6n5waipe5m84i";

        public static string Encrypt(string input)
        {
            byte[] vectorBytes = Encoding.ASCII.GetBytes(Vector);
            byte[] saltBytes = Encoding.ASCII.GetBytes(Salt);
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            byte[] encrypted;
            using (var cipher = new AesManaged())
            using (var passwordBytes = new PasswordDeriveBytes(PassPhrase, saltBytes, Hash, Iterations))
            {
                byte[] keyBytes = passwordBytes.GetBytes(KeySize / 8);
                cipher.Mode = CipherMode.CBC;

                using (var encryptor = cipher.CreateEncryptor(keyBytes, vectorBytes))
                using (var ms = new MemoryStream())
                using (var writer = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    writer.Write(inputBytes, 0, inputBytes.Length);
                    writer.FlushFinalBlock();
                    encrypted = ms.ToArray();
                }

                cipher.Clear();
            }

            return Convert.ToBase64String(encrypted);
        }

        public static string Decrypt(string input)
        {
            byte[] vectorBytes = Encoding.ASCII.GetBytes(Vector);
            byte[] saltBytes = Encoding.ASCII.GetBytes(Salt);
            byte[] inputBytes = Convert.FromBase64String(input);

            byte[] decrypted;
            int decryptedByteCount = 0;
            using (var cipher = new AesManaged())
            using (var passwordBytes = new PasswordDeriveBytes(PassPhrase, saltBytes, Hash, Iterations))
            {
                byte[] keyBytes = passwordBytes.GetBytes(KeySize / 8);
                cipher.Mode = CipherMode.CBC;

                using (var decryptor = cipher.CreateDecryptor(keyBytes, vectorBytes))
                using (var ms = new MemoryStream(inputBytes))
                using (CryptoStream reader = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    decrypted = new byte[inputBytes.Length];
                    decryptedByteCount = reader.Read(decrypted, 0, decrypted.Length);
                }

                cipher.Clear();
            }

            return Encoding.UTF8.GetString(decrypted, 0, decryptedByteCount);
        }
    }
}
