using RTCM3.Common;
using System.Buffers;

namespace RTCM3.RTCM3Message
{
    public class RTCM3_1230 : RTCM3Base
    {
        public uint StationID;
        public uint cpdSYNC;
        public uint Reserved;
        public uint FDAMMask;
        public int l1ca;
        public int l1p;
        public int l2ca;
        public int l2p;
        public uint N;
        public RTCM3_1230(ReadOnlySequence<byte> databody)
        {
            int i = 0;
            int length;
            MessageType = BitOperation.GetBitsUint(databody, i, length = 12);
            i += length;
            StationID = BitOperation.GetBitsUint(databody, i, length = 12);
            i += length;
            cpdSYNC = BitOperation.GetBitsUint(databody, i, length = 1);
            i += length;
            Reserved = BitOperation.GetBitsUint(databody, i, length = 3);
            i += length;
            FDAMMask = BitOperation.GetBitsUint(databody, i, length = 4);
            i += length;
            if ((FDAMMask & 0b1000u) > 0)
            {
                l1ca = BitOperation.GetBitsInt(databody, i, length = 16);
                i += length;
                N++;
            }
            if ((FDAMMask & 0b0100u) > 0)
            {
                l1p = BitOperation.GetBitsInt(databody, i, length = 16);
                i += length;
                N++;
            }
            if ((FDAMMask & 0b0010u) > 0)
            {
                l2ca = BitOperation.GetBitsInt(databody, i, length = 16);
                i += length;
                N++;
            }
            if ((FDAMMask & 0b0001u) > 0)
            {
                l2p = BitOperation.GetBitsInt(databody, i, 16);
                N++;
            }
        }

        public RTCM3_1230()
        {
            MessageType = 1230;
        }


        public override int GetEncodeBytesLength()
        {
            uint bitsLength = RTCM3HeaderBitsLength + CRC24QBitsLength + 32 + (16 * N);
            uint bytesLength = bitsLength / 8;
            return (int)bytesLength;
        }

        public override int Encode(ref Span<byte> bytes)
        {

            int result = GetEncodeBytesLength();
            bytes[..result].Clear();
            int i = 24;
            int length;
            BitOperation.SetBitsUint(ref bytes, i, length = 12, MessageType);
            i += length;
            BitOperation.SetBitsUint(ref bytes, i, length = 12, StationID);
            i += length;
            BitOperation.SetBitsUint(ref bytes, i, length = 1, cpdSYNC);
            i += length;
            BitOperation.SetBitsUint(ref bytes, i, length = 3, Reserved);
            i += length;
            BitOperation.SetBitsUint(ref bytes, i, length = 4, FDAMMask);
            i += length;
            if ((FDAMMask & 0b1000u) > 0)
            {
                BitOperation.SetBitsInt(ref bytes, i, length = 16, l1ca);
                i += length;
            }
            if ((FDAMMask & 0b0100u) > 0)
            {
                BitOperation.SetBitsInt(ref bytes, i, length = 16, l1p);
                i += length;
            }
            if ((FDAMMask & 0b0010u) > 0)
            {
                BitOperation.SetBitsInt(ref bytes, i, length = 16, l2ca);
                i += length;
            }
            if ((FDAMMask & 0b0001u) > 0)
            {
                BitOperation.SetBitsInt(ref bytes, i, length = 16, l2p);
                i += length;
            }
            EncodeRTCM3(ref bytes, i - 24);
            return result;
        }
    }
}
