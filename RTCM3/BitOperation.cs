namespace RTCM3
{
    public static class BitOperation
    {
        public static uint GetBitsUint(ReadOnlySpan<byte> buff, int pos, int length)
        {
            uint result = 0;
            if (length > 32 || buff.Length * 8 < pos + length)
            {
                throw new IndexOutOfRangeException();
            }
            for (int i = pos; i < pos + length; i++)
            {
                result = (result << 1) + (((uint)buff[i / 8] >> (7 - (i % 8))) & 1u);
            }
            return result;
        }

        public static long GetBitsIntS(ReadOnlySpan<byte> buff, int pos, int length)
        {
            long value = GetBitsUint(buff, pos + 1, length - 1);
            return GetBitsUint(buff, pos, 1) == 1u ? -value : value;
        }

        public static void SetBitsUint(ref Memory<byte> memory, int pos, int length, uint data)
        {
            Span<byte> buff = memory.Span;
            uint mask = 1u << (length - 1);
            if (length > 32 || buff.Length * 8 < pos + length)
            {
                throw new IndexOutOfRangeException();
            }
            for (int i = pos; i < pos + length; i++, mask >>= 1)
            {
                if ((data & mask) > 0)
                {
                    buff[i / 8] |= (byte)(1u << (7 - (i % 8)));
                }
                else
                {
                    buff[i / 8] &= (byte)~(1u << (7 - (i % 8)));
                }
            }
        }
        public static int GetBitsInt(ReadOnlySpan<byte> buff, int pos, int length)
        {
            uint result = GetBitsUint(buff, pos, length);
            return length <= 0 || 32 <= length || 0 == (result & (1u << (length - 1))) ? (int)result : (int)(result | (~0u << length));
        }
        public static void SetBitsInt(ref Memory<byte> buff, int pos, int length, int data)
        {
            if (data < 0)
            {
                data |= 1 << (length - 1);
            }
            else
            {
                data &= ~(1 << (length - 1));
            }
            SetBitsUint(ref buff, pos, length, (uint)data);
        }
        public static ulong GetBitsUlong(ReadOnlySpan<byte> buff, int pos, int length)
        {
            ulong result = 0;
            if (length > 64 || buff.Length * 8 < pos + length)
            {
                throw new IndexOutOfRangeException();
            }
            for (int i = pos; i < pos + length; i++)
            {
                result = (result << 1) + (((ulong)buff[i / 8] >> (7 - (i % 8))) & 1ul);
            }
            return result;
        }
        public static void SetBitsUlong(ref Memory<byte> memory, int pos, int length, ulong data)
        {
            ulong mask = 1ul << (length - 1);
            Span<byte> buff = memory.Span;
            if (length > 64 || buff.Length * 8 < pos + length)
            {
                throw new IndexOutOfRangeException();
            }
            for (int i = pos; i < pos + length; i++, mask >>= 1)
            {
                if ((data & mask) > 0)
                {
                    buff[i / 8] |= (byte)(1u << (7 - (i % 8)));
                }
                else
                {
                    buff[i / 8] &= (byte)~(1u << (7 - (i % 8)));
                }
            }
        }
        public static long GetBitsLong(ReadOnlySpan<byte> buff, int pos, int length)
        {
            ulong result = GetBitsUlong(buff, pos, length);
            return length <= 0 || 64 <= length || 0 == (result & (1ul << (length - 1))) ? (long)result : (long)(result | (~0ul << length));
        }
        public static void SetBitsLong(ref Memory<byte> buff, int pos, int length, long data)
        {
            if (data < 0)
            {
                data |= 1L << (length - 1);
            }
            else
            {
                data &= ~(1L << (length - 1));
            }
            SetBitsUlong(ref buff, pos, length, (ulong)data);
        }
    }
}
