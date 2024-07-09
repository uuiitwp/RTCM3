using RTCM3.Common;
using System;
using System.Buffers;
using System.Runtime.InteropServices;

namespace RTCM3.RTCM3Message
{
    public abstract class RTCM3Base
    {
        protected const uint Preamble = 0xd3;
        protected const int RTCM3HeaderBitsLength = 24;
        protected const int CRC24QBitsLength = 24;


        public uint MessageType;
        protected static uint GetBodyBytesLength(int bodyBitsLength)
        {
            uint bodyBytesLength = (uint)(bodyBitsLength / 8);
            if (bodyBitsLength % 8 > 0)
            {
                bodyBytesLength++;
            }
            return bodyBytesLength;
        }
        public static void EncodeRTCM3(ref Span<byte> result, int bodyBitsLength)
        {
            uint bodyBytesLength = GetBodyBytesLength(bodyBitsLength);
            BitOperation.SetBitsUint(ref result, 0, 8, Preamble);
            BitOperation.SetBitsUint(ref result, 14, 10, bodyBytesLength);
            var s = result[..(int)(bodyBytesLength + 3)];
            BitOperation.SetBitsUint(ref result, RTCM3HeaderBitsLength + ((int)bodyBytesLength * 8), CRC24QBitsLength, (uint)CRC24Q.Get(s));

        }
        public abstract int Encode(ref Span<byte> bytes);

        public abstract int GetEncodeBytesLength();

        protected static long RoundToLong(double value)
        {
            return (long)(value + (value > 0 ? 0.5 : -0.5));
        }
        protected static int RoundToInt(double value)
        {
            return (int)(value + (value > 0 ? 0.5 : -0.5));
        }
    }
}
