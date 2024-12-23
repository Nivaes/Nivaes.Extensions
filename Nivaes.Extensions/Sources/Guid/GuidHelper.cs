namespace Nivaes
{
    using System;

    public static class GuidHelper
    {
        public static Guid Combine(params Guid[] guids)
        {
            ArgumentNullException.ThrowIfNull(guids);

            byte[] buffer = new byte[16];

            foreach (var guid in guids)
            {
                var guidBuffer = guid.ToByteArray();
                for (int i = 0; i < 16; i++)
                {
                    buffer[i] = (byte)((buffer[i] + guidBuffer[i]) % 256);
                }
            }

            return new Guid(buffer);
        }
    }
}
