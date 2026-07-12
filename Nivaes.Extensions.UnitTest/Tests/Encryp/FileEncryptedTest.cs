using Shouldly;
using Xunit;

namespace Nivaes.Extensions.UnitTest.Encryp;

[Trait("TestType", "Unit")]
public class FileEncryptedTest
{
    [Fact]
    public async Task EncryptFile1()
    {
        const string filenameGenerator = "FileEncryptedTest.txt";
        var originMessage = "kjkdkdif";
        // Set your salt here, change it to meet your flavor:
        // The salt bytes must be at least 8 bytes.
        var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

        await FileEncrypted.EncryptedWriteAllTextAsync(filenameGenerator, originMessage, "pass", saltBytes, 1000, TestContext.Current.CancellationToken).ConfigureAwait(true);

        var decryptMessage = await FileEncrypted.EncryptedReadAllTextAsync(filenameGenerator, "pass", saltBytes, 1000, TestContext.Current.CancellationToken).ConfigureAwait(true);

        decryptMessage.ShouldBe(originMessage);
    }
}
