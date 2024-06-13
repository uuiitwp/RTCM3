using RTCM3.Common;

namespace RTCM3.RTCM3Message
{
    public class RTCM3_1012 : RTCM3_1009to1012
    {
        public uint[] prn;
        public uint[] code1;
        public uint[] pr1;
        public uint[] fcn;
        public uint[] ppr1;
        public uint[] lock1;
        public uint[] amb;
        public uint[] cnr1;
        public uint[] code2;
        public uint[] pr21;
        public uint[] ppr2;
        public uint[] lock2;
        public uint[] cnr2;


        public RTCM3_1012(ReadOnlySpan<byte> databody) : base(databody)
        {
            prn = new uint[GLONASSSatNumber];
            code1 = new uint[GLONASSSatNumber];
            pr1 = new uint[GLONASSSatNumber];
            fcn = new uint[GLONASSSatNumber];
            ppr1 = new uint[GLONASSSatNumber];
            lock1 = new uint[GLONASSSatNumber];
            amb = new uint[GLONASSSatNumber];
            cnr1 = new uint[GLONASSSatNumber];
            code2 = new uint[GLONASSSatNumber];
            pr21 = new uint[GLONASSSatNumber];
            ppr2 = new uint[GLONASSSatNumber];
            lock2 = new uint[GLONASSSatNumber];
            cnr2 = new uint[GLONASSSatNumber];
            int length;
            for (int j = 0; j < GLONASSSatNumber; j++)
            {
                prn[j] = BitOperation.GetBitsUint(databody, i, length = 6);
                i += length;
                code1[j] = BitOperation.GetBitsUint(databody, i, length = 1);
                i += length;
                fcn[j] = BitOperation.GetBitsUint(databody, i, length = 5);
                i += length;
                pr1[j] = BitOperation.GetBitsUint(databody, i, length = 25);
                i += length;
                ppr1[j] = BitOperation.GetBitsUint(databody, i, length = 20);
                i += length;
                lock1[j] = BitOperation.GetBitsUint(databody, i, length = 7);
                i += length;
                amb[j] = BitOperation.GetBitsUint(databody, i, length = 7);
                i += length;
                cnr1[j] = BitOperation.GetBitsUint(databody, i, length = 8);
                i += length;
                code2[j] = BitOperation.GetBitsUint(databody, i, length = 2);
                i += length;
                pr21[j] = BitOperation.GetBitsUint(databody, i, length = 14);
                i += length;
                ppr2[j] = BitOperation.GetBitsUint(databody, i, length = 20);
                i += length;
                lock2[j] = BitOperation.GetBitsUint(databody, i, length = 7);
                i += length;
                cnr2[j] = BitOperation.GetBitsUint(databody, i, length = 8);
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
