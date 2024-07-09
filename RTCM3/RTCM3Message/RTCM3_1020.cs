using RTCM3.Common;
using System.Buffers;

namespace RTCM3.RTCM3Message
{
    public class RTCM3_1020 : RTCM3Base
    {
        public uint prn;
        public int freq;
        public uint ah;
        public uint ahf;
        public uint p1;
        public uint tk;
        public uint bn;
        public uint p2;
        public uint tb;
        public double vx;
        public double px;
        public double ax;
        public double vy;
        public double py;
        public double ay;
        public double vz;
        public double pz;
        public double az;
        public uint p3;
        public double gamman;
        public uint p;
        public uint ln3;
        public double taun;
        public double dtaun;
        public uint en;
        public uint p4;
        public uint ft;
        public uint nt;
        public uint m;
        public uint ex;
        public uint na;
        public double tauc;
        public uint n4;
        public double taugps;
        public uint ln5;
        public uint reserved;

        public RTCM3_1020(ReadOnlySequence<byte> databody)
        {
            int i = 0;
            int length;
            MessageType = BitOperation.GetBitsUint(databody, i, length = 12);
            i += length;
            prn = BitOperation.GetBitsUint(databody, i, length = 6);
            i += length;
            freq = (int)BitOperation.GetBitsUint(databody, i, length = 5) - 7;
            i += length;
            ah = BitOperation.GetBitsUint(databody, i, length = 1);
            i += length;
            ahf = BitOperation.GetBitsUint(databody, i, length = 1);
            i += length;
            p1 = BitOperation.GetBitsUint(databody, i, length = 2);
            i += length;
            tk = BitOperation.GetBitsUint(databody, i, length = 12);
            i += length;
            bn = BitOperation.GetBitsUint(databody, i, length = 1);
            i += length;
            p2 = BitOperation.GetBitsUint(databody, i, length = 1);
            i += length;
            tb = BitOperation.GetBitsUint(databody, i, length = 7);
            i += length;
            vx = BitOperation.GetBitsIntS(databody, i, length = 24) * Common.Math.pow2_m20 * Physics.km;
            i += length;
            px = BitOperation.GetBitsIntS(databody, i, length = 27) * Common.Math.pow2_m11 * Physics.km;
            i += length;
            ax = BitOperation.GetBitsIntS(databody, i, length = 5) * Common.Math.pow2_m30 * Physics.km;
            i += length;
            vy = BitOperation.GetBitsIntS(databody, i, length = 24) * Common.Math.pow2_m20 * Physics.km;
            i += length;
            py = BitOperation.GetBitsIntS(databody, i, length = 27) * Common.Math.pow2_m11 * Physics.km;
            i += length;
            ay = BitOperation.GetBitsIntS(databody, i, length = 5) * Common.Math.pow2_m30 * Physics.km;
            i += length;
            vz = BitOperation.GetBitsIntS(databody, i, length = 24) * Common.Math.pow2_m20 * Physics.km;
            i += length;
            pz = BitOperation.GetBitsIntS(databody, i, length = 27) * Common.Math.pow2_m11 * Physics.km;
            i += length;
            az = BitOperation.GetBitsIntS(databody, i, length = 5) * Common.Math.pow2_m30 * Physics.km;
            i += length;
            p3 = BitOperation.GetBitsUint(databody, i, length = 1);
            i += length;
            gamman = BitOperation.GetBitsIntS(databody, i, length = 11) * Common.Math.pow2_m40;
            i += length;
            p = BitOperation.GetBitsUint(databody, i, length = 2);
            i += length;
            ln3 = BitOperation.GetBitsUint(databody, i, length = 1);
            i += length;
            taun = BitOperation.GetBitsIntS(databody, i, length = 22) * Common.Math.pow2_m30;
            i += length;
            dtaun = BitOperation.GetBitsIntS(databody, i, length = 5) * Common.Math.pow2_m30;
            i += length;
            en = BitOperation.GetBitsUint(databody, i, length = 5);
            i += length;
            p4 = BitOperation.GetBitsUint(databody, i, length = 1);
            i += length;
            ft = BitOperation.GetBitsUint(databody, i, length = 4);
            i += length;
            nt = BitOperation.GetBitsUint(databody, i, length = 11);
            i += length;
            m = BitOperation.GetBitsUint(databody, i, length = 2);
            i += length;
            ex = BitOperation.GetBitsUint(databody, i, length = 1);
            i += length;
            na = BitOperation.GetBitsUint(databody, i, length = 11);
            i += length;
            tauc = BitOperation.GetBitsIntS(databody, i, length = 32) * Common.Math.pow2_m31;
            i += length;
            n4 = BitOperation.GetBitsUint(databody, i, length = 5);
            i += length;
            taugps = BitOperation.GetBitsIntS(databody, i, length = 22) * Common.Math.pow2_m30;
            i += length;
            ln3 = BitOperation.GetBitsUint(databody, i, length = 1);
            i += length;
            reserved = BitOperation.GetBitsUint(databody, i, 7);
        }

        public RTCM3_1020()
        {
            MessageType = 1020;
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
