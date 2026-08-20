using System.Security.Cryptography;
using System.Text;

namespace Nivaes;

public static class EncryptHelper
{
    private static byte[] defaultSalt = [ 0x7A, 0xC3, 0x15, 0xE8, 0x91, 0x4F, 0x2B, 0xD6];

    /// <summary>
    /// Encrypt a string.
    /// </summary>
    /// <param name="plainText">String to be encrypted</param>
    /// <param name="password">Password</param>
    /// <param name="salt">Salt</param>
    /// <param name="iterations">Number of iterations</param>
    /// <param name="cancellationToken">Cancelation Token</param>
    public static async ValueTask<string?> EncryptAsync(this string plainText, string password,
        byte[]? salt = null, int iterations = 1000, CancellationToken cancellationToken = default)
    {
        salt ??= defaultSalt;

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

        var bytesEncrypted = await EncryptAsync(bytesToBeEncrypted, passwordBytes, salt, iterations).ConfigureAwait(true);

        return Convert.ToBase64String(bytesEncrypted);
    }

    /// <summary>
    /// Decrypt a string.
    /// </summary>
    /// <param name="encryptedText">String to be decrypted</param>
    /// <param name="password">Password used during encryption</param>
    /// <param name="salt">Salt</param>
    /// <param name="iterations">Number of iterations</param>
    /// <param name="cancellationToken">Cancelation Token</param>
    /// <exception cref="FormatException"></exception>
    public static async ValueTask<string> DecryptAsync(this string encryptedText, string password,
        byte[]? salt = null, int iterations = 1000, CancellationToken cancellationToken = default)
    {
        salt ??= defaultSalt;

        password ??= string.Empty;

        // Get the bytes of the string
        var bytesToBeDecrypted = Convert.FromBase64String(encryptedText);
        var passwordBytes = Encoding.UTF8.GetBytes(password);

        using var sha256 = SHA256.Create();
        passwordBytes = sha256.ComputeHash(passwordBytes);

        var bytesDecrypted = await DecryptAsync(bytesToBeDecrypted, passwordBytes, salt, iterations, cancellationToken).ConfigureAwait(true);

        return Encoding.UTF8.GetString(bytesDecrypted);
    }

    private static async ValueTask<byte[]> EncryptAsync(byte[] bytesToBeEncrypted, byte[] passwordBytes,
        byte[] salt, int iterations, CancellationToken cancellationToken = default)
    {
        byte[] encryptedBytes;

        await using (MemoryStream ms = new MemoryStream())
        {
            using (var aes = Aes.Create())
            {
                var key = Rfc2898DeriveBytes.Pbkdf2(passwordBytes, salt, iterations,
                    HashAlgorithmName.SHA256, (aes.KeySize + aes.BlockSize) / 8);

                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Key = key[..(aes.KeySize / 8)];
                aes.IV = key[(aes.KeySize / 8)..];

                await using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    await cs.WriteAsync(bytesToBeEncrypted, cancellationToken).ConfigureAwait(true);
                }

                encryptedBytes = ms.ToArray();
            }
        }

        return encryptedBytes;
    }

    private static async ValueTask<byte[]> DecryptAsync(byte[] bytesToBeDecrypted, byte[] passwordBytes, byte[] salt, int iterations, CancellationToken cancellationToken = default)
    {
        byte[] decryptedBytes;

        await using (MemoryStream ms = new MemoryStream())
        {
            using (var aes = Aes.Create())
            {
                var key = Rfc2898DeriveBytes.Pbkdf2(passwordBytes, salt, iterations, HashAlgorithmName.SHA256, (aes.KeySize + aes.BlockSize) / 8);

                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Key = key[..(aes.KeySize / 8)];
                aes.IV = key[(aes.KeySize / 8)..];

                await using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    await cs.WriteAsync(bytesToBeDecrypted, cancellationToken).ConfigureAwait(true);
                }

                decryptedBytes = ms.ToArray();
            }
        }

        return decryptedBytes;
    }
}
