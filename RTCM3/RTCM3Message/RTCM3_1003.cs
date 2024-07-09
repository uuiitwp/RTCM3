using RTCM3.Common;
using System.Buffers;

namespace RTCM3.RTCM3Message
{
    public class RTCM3_1003 : RTCM3_1001to1004
    {
        public uint[] prn;
        public uint[] code1;
        public uint[] pr1;
        public uint[] ppr1;
        public uint[] lock1;
        public uint[] code2;
        public uint[] pr21;
        public uint[] ppr21;
        public uint[] lock2;


        public RTCM3_1003(ReadOnlySequence<byte> databody) : base(databody)
        {
            prn = new uint[GPSSatNumber];
            code1 = new uint[GPSSatNumber];
            pr1 = new uint[GPSSatNumber];
            ppr1 = new uint[GPSSatNumber];
            lock1 = new uint[GPSSatNumber];
            code2 = new uint[GPSSatNumber];
            pr21 = new uint[GPSSatNumber];
            ppr21 = new uint[GPSSatNumber];
            lock2 = new uint[GPSSatNumber];
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
                code2[j] = BitOperation.GetBitsUint(databody, i, length = 2);
                i += length;
                pr21[j] = BitOperation.GetBitsUint(databody, i, length = 14);
                i += length;
                ppr21[j] = BitOperation.GetBitsUint(databody, i, length = 20);
                i += length;
                lock2[j] = BitOperation.GetBitsUint(databody, i, length = 7);
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
        public override Observation[] GetObservations()
        {
            throw new NotImplementedException();
        }

    }
}
