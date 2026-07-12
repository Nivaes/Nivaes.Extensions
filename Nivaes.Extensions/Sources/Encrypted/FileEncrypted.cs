namespace Nivaes;

public static class FileEncrypted
{
    private static byte[] defaultSalt = [ 0x08, 0xFA, 0x73, 0x1C, 0xB5, 0x9E, 0x44, 0x27,];

    public static async Task WriteAllTextAsync(string path, string message, string key,
        byte[]? salt = null, int iterations = 1000, CancellationToken cancellationToken = default)
    {
        salt ??= defaultSalt;

        var mesageEncrypted = await message.EncryptAsync(key, salt, iterations);
        await File.WriteAllTextAsync(path, mesageEncrypted, cancellationToken);
    }

    public static async Task<string> ReadAllTextAsync(string path, string key,
        byte[]? salt = null, int iterations = 1000, CancellationToken cancellationToken = default)
    {
        salt ??= defaultSalt;

        var messageEncrypted = await File.ReadAllTextAsync(path, cancellationToken);
        return await messageEncrypted.DecryptAsync(key, salt, iterations, cancellationToken);
    }
}
