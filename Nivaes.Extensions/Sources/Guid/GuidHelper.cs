using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Nivaes
{
    public static class GuidHelper
    {
        public static Guid Combine(params ReadOnlySpan<Guid> guids)
        {
            ulong lo = 0;
            ulong hi = 0;

            foreach (ref readonly var guid in guids)
            {
                ReadOnlySpan<byte> bytes = MemoryMarshal.AsBytes(
                    MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in guid), 1));

                lo ^= MemoryMarshal.Read<ulong>(bytes);
                hi ^= MemoryMarshal.Read<ulong>(bytes[8..]);
            }

            Span<byte> result = stackalloc byte[16];
            MemoryMarshal.Write(result, in lo);
            MemoryMarshal.Write(result[8..], in hi);

            return new Guid(result);
        }
    }
}
