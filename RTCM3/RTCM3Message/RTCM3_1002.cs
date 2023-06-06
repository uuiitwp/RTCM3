using RTCM3.Common;

namespace RTCM3.RTCM3Message
{
    public class RTCM3_1002 : RTCM3_1001to1004
    {
        public uint[] prn;
        public uint[] code1;
        public uint[] pr1;
        public uint[] ppr1;
        public uint[] lock1;
        public uint[] amb1;
        public uint[] cnr1;

        public RTCM3_1002(ReadOnlySpan<byte> databody) : base(databody)
        {
            prn = new uint[GPSSatNumber];
            code1 = new uint[GPSSatNumber];
            pr1 = new uint[GPSSatNumber];
            ppr1 = new uint[GPSSatNumber];
            lock1 = new uint[GPSSatNumber];
            amb1 = new uint[GPSSatNumber];
            cnr1 = new uint[GPSSatNumber];
            int length;
            for (int j = 0; j < GPSSatNumber; j++)
            {
                prn[j] = BitOperation.GetBitsUint(databody, i, length = 6);
                i += length;
                code1[j] = BitOperation.GetBitsUint(databody, i, length = 1);
                i += length;
                pr1[j] = BitOperation.GetBitsUint(databody, i, length = 24);
                i += length;
                ppr1[j] = BitOperation.GetBitsUint(databody, i, length = 20);
                i += length;
                lock1[j] = BitOperation.GetBitsUint(databody, i, length = 7);
                i += length;
                amb1[j] = BitOperation.GetBitsUint(databody, i, length = 8);
                i += length;
                cnr1[j] = BitOperation.GetBitsUint(databody, i, length = 8);
                i += length;
            }
        }
        public override int Encode(ref Span<byte> bytes)
        {
            throw new NotImplementedException();
        }

        public override int GetEncodeBytesLength()
        {
            throw new NotImplementedException();
        }

    }
}
