using RTCM3.Common;

namespace RTCM3.RTCM3Message
{
    public class RTCM3_1019 : RTCM3Base
    {
        public uint prn;
        public uint week;
        public uint ura;
        public uint code2;
        public double idot;
        public uint iode;
        public double toc;
        public double af2;
        public double af1;
        public double af0;
        public uint iodc;
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
        public double tgd;
        public uint svh;
        public uint l2pflag;
        public uint fit;



        public RTCM3_1019(ReadOnlySpan<byte> databody)
        {
            int i = 0;
            int length;
            MessageType = BitOperation.GetBitsUint(databody, i, length = 12);
            i += length;
            prn = BitOperation.GetBitsUint(databody, i, length = 6);
            i += length;
            week = BitOperation.GetBitsUint(databody, i, length = 10);
            i += length;
            ura = BitOperation.GetBitsUint(databody, i, length = 4);
            i += length;
            code2 = BitOperation.GetBitsUint(databody, i, length = 2);
            i += length;
            idot = BitOperation.GetBitsInt(databody, i, length = 14) * Common.Math.pow2_m43 * Common.Math.PI;
            i += length;
            iode = BitOperation.GetBitsUint(databody, i, length = 8);
            i += length;
            toc = BitOperation.GetBitsUint(databody, i, length = 16) * Common.Math.pow2_p4;
            i += length;
            af2 = BitOperation.GetBitsInt(databody, i, length = 8) * Common.Math.pow2_m55;
            i += length;
            af1 = BitOperation.GetBitsInt(databody, i, length = 16) * Common.Math.pow2_m43;
            i += length;
            af0 = BitOperation.GetBitsInt(databody, i, length = 22) * Common.Math.pow2_m31;
            i += length;
            iodc = BitOperation.GetBitsUint(databody, i, length = 10);
            i += length;
            crs = BitOperation.GetBitsInt(databody, i, length = 16) * Common.Math.pow2_m5;
            i += length;
            deltan = BitOperation.GetBitsInt(databody, i, length = 16) * Common.Math.pow2_m43 * Common.Math.PI;
            i += length;
            M0 = BitOperation.GetBitsUint(databody, i, length = 32) * Common.Math.pow2_m31 * Common.Math.PI;
            i += length;
            cuc = BitOperation.GetBitsInt(databody, i, length = 16) * Common.Math.pow2_m29;
            i += length;
            e = BitOperation.GetBitsUint(databody, i, length = 32) * Common.Math.pow2_m33;
            i += length;
            cus = BitOperation.GetBitsInt(databody, i, length = 16) * Common.Math.pow2_m29;
            i += length;
            sqrta = BitOperation.GetBitsUint(databody, i, length = 32) * Common.Math.pow2_m19;
            i += length;
            toe = BitOperation.GetBitsUint(databody, i, length = 16) * Common.Math.pow2_p4;
            i += length;
            cic = BitOperation.GetBitsInt(databody, i, length = 16) * Common.Math.pow2_m29;
            i += length;
            omg0 = BitOperation.GetBitsInt(databody, i, length = 32) * Common.Math.pow2_m31 * Common.Math.PI;
            i += length;
            cis = BitOperation.GetBitsInt(databody, i, length = 16) * Common.Math.pow2_m29;
            i += length;
            i0 = BitOperation.GetBitsInt(databody, i, length = 32) * Common.Math.pow2_m31 * Common.Math.PI;
            i += length;
            crc = BitOperation.GetBitsInt(databody, i, length = 16) * Common.Math.pow2_m5;
            i += length;
            omg = BitOperation.GetBitsInt(databody, i, length = 32) * Common.Math.pow2_m31 * Common.Math.PI;
            i += length;
            omgd = BitOperation.GetBitsInt(databody, i, length = 24) * Common.Math.pow2_m43 * Common.Math.PI;
            i += length;
            tgd = BitOperation.GetBitsInt(databody, i, length = 8) * Common.Math.pow2_m31;
            i += length;
            svh = BitOperation.GetBitsUint(databody, i, length = 6);
            i += length;
            l2pflag = BitOperation.GetBitsUint(databody, i, length = 1);
            i += length;
            fit = BitOperation.GetBitsUint(databody, i, 1);
        }

        public RTCM3_1019()
        {
            MessageType = 1019;
        }

        public override void Encode(ref Span<byte> bytes)
        {
            throw new NotImplementedException();
        }

        public override int GetEncodeBytesLength()
        {
            throw new NotImplementedException();
        }
    }
}
