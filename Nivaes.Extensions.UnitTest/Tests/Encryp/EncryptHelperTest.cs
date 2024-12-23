namespace Nivaes.UnitTest
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Xunit;
    using Nivaes.DataTestGenerator.Xunit;


    [Trait("TestType", "Unit")]
    public class EncryptHelperTest
    {
        [Fact]
        public async Task EncryptSuccess1()
        {
            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            var originMessage = "kjkdkdif";
            var encriptedMessage = await EncryptHelper.Encrypt(originMessage, "pass", saltBytes, 1000).ConfigureAwait(true);
            encriptedMessage.Should().NotBeNull();

            var decryptMessage = await EncryptHelper.Decrypt(encriptedMessage!, "pass", saltBytes, 1000).ConfigureAwait(true);

            decryptMessage.Should().Be(originMessage);
        }

        [Theory]
        [InlineData("Hola mundo", "pass")]
        [InlineData("alsdhfahsdfj akdfjas dfjalñks kl", "dkdf38834$$·33")]
        [InlineData("", "")]
        [InlineData("", "kdk33.55%")]
        [InlineData("jaklsdfjkdjasñf aaksjdf ñlakjfñ kajñf kañlfj aklsfj aksdfjaslñie", "1")]
        [InlineData("kajsdñfklas dfaksjf ñadfj ñdfjsie", "138382929.30293+ç")]
        public async Task EncryptSuccess2(string message, string pass)
        {
            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            var encriptedMessage = await EncryptHelper.Encrypt(message, pass, saltBytes, 1000).ConfigureAwait(true);
            encriptedMessage.Should().NotBeNull();

            var decryptMessage = await EncryptHelper.Decrypt(encriptedMessage!, pass, saltBytes, 1000).ConfigureAwait(true);

            decryptMessage.Should().Be(message);
        }

        [Theory]
        [GenerateStringInlineData(DataNumber = 3, MinSize = 35, MaxSize = 2000)]
        public async Task EncryptSuccess3(string message)
        {
            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            string pass = "938!·";
            var encriptedMessage = await EncryptHelper.Encrypt(message, pass, saltBytes, 1000).ConfigureAwait(true);
            encriptedMessage.Should().NotBeNull();

            var decryptMessage = await EncryptHelper.Decrypt(encriptedMessage!, pass, saltBytes, 1000).ConfigureAwait(true);

            decryptMessage.Should().Be(message);
        }
    }
}
