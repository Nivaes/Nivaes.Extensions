﻿namespace Nivaes
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    public static class EncryptHelper
    {
        /// <summary>
        /// Encrypt a string.
        /// </summary>
        /// <param name="plainText">String to be encrypted</param>
        /// <param name="password">Password</param>
        public static async ValueTask<string?> Encrypt(string plainText, string password, byte[] salt, int iterations)
        {
            if (plainText == null)
            {
                return null;
            }

            password ??= string.Empty;

            // Get the bytes of the string
            var bytesToBeEncrypted = Encoding.UTF8.GetBytes(plainText);
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            using var sha256 = SHA256.Create();
            passwordBytes = sha256.ComputeHash(passwordBytes);

            var bytesEncrypted = await Encrypt(bytesToBeEncrypted, passwordBytes, salt, iterations).ConfigureAwait(true);

            return Convert.ToBase64String(bytesEncrypted);
        }

        /// <summary>
        /// Decrypt a string.
        /// </summary>
        /// <param name="encryptedText">String to be decrypted</param>
        /// <param name="password">Password used during encryption</param>
        /// <exception cref="FormatException"></exception>
        public static async ValueTask<string?> Decrypt(string encryptedText, string password, byte[] salt, int iterations)
        {
            if (encryptedText == null)
            {
                return null;
            }

            password ??= string.Empty;

            // Get the bytes of the string
            var bytesToBeDecrypted = Convert.FromBase64String(encryptedText);
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            using var sha256 = SHA256.Create();
            passwordBytes = sha256.ComputeHash(passwordBytes);

            var bytesDecrypted = await Decrypt(bytesToBeDecrypted, passwordBytes, salt, iterations).ConfigureAwait(true);

            return Encoding.UTF8.GetString(bytesDecrypted);
        }

        private static async ValueTask<byte[]> Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes, byte[] salt, int iterations)
        {
            byte[] encryptedBytes;

            using (MemoryStream ms = new MemoryStream())
            {
                using (var aes = Aes.Create())
                {
                    using var key = new Rfc2898DeriveBytes(passwordBytes, salt, iterations, HashAlgorithmName.SHA256);

                    aes.KeySize = 256;
                    aes.BlockSize = 128;
                    aes.Key = key.GetBytes(aes.KeySize / 8);
                    aes.IV = key.GetBytes(aes.BlockSize / 8);

                    aes.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        await cs.WriteAsync(bytesToBeEncrypted).ConfigureAwait(true);
                        cs.Close();
                    }

                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        private static async ValueTask<byte[]> Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes, byte[] salt, int iterations)
        {
            byte[] decryptedBytes;

            using (MemoryStream ms = new MemoryStream())
            {
                using (var aes = Aes.Create())
                {
                    using var key = new Rfc2898DeriveBytes(passwordBytes, salt, iterations, HashAlgorithmName.SHA256);

                    aes.KeySize = 256;
                    aes.BlockSize = 128;
                    aes.Key = key.GetBytes(aes.KeySize / 8);
                    aes.IV = key.GetBytes(aes.BlockSize / 8);
                    aes.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        await cs.WriteAsync(bytesToBeDecrypted).ConfigureAwait(true);
                        cs.Close();
                    }

                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }
    }
}
