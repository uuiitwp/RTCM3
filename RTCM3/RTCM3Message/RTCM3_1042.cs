using RTCM3.Common;
using System.Buffers;

namespace RTCM3.RTCM3Message
{
    public class RTCM3_1042 : RTCM3Base
    {
        public uint prn;
        public uint week;
        public uint ura;
        public double idot;
        public uint aode;
        public double toc;
        public double a2;
        public double a1;
        public double a0;
        public uint aodc;
        public double crs;
        public double deltan;
        public double M0;
        public double cuc;
        public double e;
        public double cus;
        public double sqrta;
        public double toe;
        public double cic;
        public double omg0;
        public double cis;
        public double i0;
        public double crc;
        public double omg;
        public double omgd;
        public double tgd1;
        public double tgd2;
        public uint svh;


        public RTCM3_1042(ReadOnlySequence<byte> databody)
        {
            int i = 0;
            int length;
            MessageType = BitOperation.GetBitsUint(databody, i, length = 12);
            i += length;
            prn = BitOperation.GetBitsUint(databody, i, length = 6);
            i += length;
            week = BitOperation.GetBitsUint(databody, i, length = 13);
            i += length;
            ura = BitOperation.GetBitsUint(databody, i, length = 4);
            i += length;
            idot = BitOperation.GetBitsInt(databody, i, length = 14) * Common.Math.pow2_m43 * Common.Math.PI;
            i += length;
            aode = BitOperation.GetBitsUint(databody, i, length = 5);
            i += length;
            toc = BitOperation.GetBitsUint(databody, i, length = 17) * Common.Math.pow2_p3;
            i += length;
            a2 = BitOperation.GetBitsInt(databody, i, length = 11) * Common.Math.pow2_m66;
            i += length;
            a1 = BitOperation.GetBitsInt(databody, i, length = 22) * Common.Math.pow2_m50;
            i += length;
            a0 = BitOperation.GetBitsInt(databody, i, length = 24) * Common.Math.pow2_m33;
            i += length;
            aodc = BitOperation.GetBitsUint(databody, i, length = 5);
            i += length;
            crs = BitOperation.GetBitsInt(databody, i, length = 18) * Common.Math.pow2_m6;
            i += length;
            deltan = BitOperation.GetBitsInt(databody, i, length = 16) * Common.Math.pow2_m43 * Common.Math.PI;
            i += length;
            M0 = BitOperation.GetBitsUint(databody, i, length = 32) * Common.Math.pow2_m31 * Common.Math.PI;
            i += length;
            cuc = BitOperation.GetBitsInt(databody, i, length = 18) * Common.Math.pow2_m31;
            i += length;
            e = BitOperation.GetBitsUint(databody, i, length = 32) * Common.Math.pow2_m33;
            i += length;
            cus = BitOperation.GetBitsInt(databody, i, length = 18) * Common.Math.pow2_m31;
            i += length;
            sqrta = BitOperation.GetBitsUint(databody, i, length = 32) * Common.Math.pow2_m19;
            i += length;
            toe = BitOperation.GetBitsUint(databody, i, length = 17) * Common.Math.pow2_p3;
            i += length;
            cic = BitOperation.GetBitsInt(databody, i, length = 18) * Common.Math.pow2_m31;
            i += length;
            omg0 = BitOperation.GetBitsInt(databody, i, length = 32) * Common.Math.pow2_m31 * Common.Math.PI;
            i += length;
            cis = BitOperation.GetBitsInt(databody, i, length = 18) * Common.Math.pow2_m31;
            i += length;
            i0 = BitOperation.GetBitsInt(databody, i, length = 32) * Common.Math.pow2_m31 * Common.Math.PI;
            i += length;
            crc = BitOperation.GetBitsInt(databody, i, length = 18) * Common.Math.pow2_m6;
            i += length;
            omg = BitOperation.GetBitsInt(databody, i, length = 32) * Common.Math.pow2_m31 * Common.Math.PI;
            i += length;
            omgd = BitOperation.GetBitsInt(databody, i, length = 24) * Common.Math.pow2_m43 * Common.Math.PI;
            i += length;
            tgd1 = BitOperation.GetBitsUint(databody, i, length = 10);
            i += length;
            tgd2 = BitOperation.GetBitsUint(databody, i, length = 10);
            i += length;
            svh = BitOperation.GetBitsUint(databody, i, 1);
        }

        public RTCM3_1042()
        {
            MessageType = 1042;
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
