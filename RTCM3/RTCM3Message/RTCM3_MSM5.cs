using RTCM3.Common;
using System.Buffers;

namespace RTCM3.RTCM3Message
{
    public class RTCM3_MSM5 : RTCM3_MSM57
    {
        public RTCM3_MSM5(ReadOnlySequence<byte> databody) : base(databody)
        {

            for (int j = 0; j < NCell; j++)
            {
                int temp = BitOperation.GetBitsInt(databody, i, 15);
                prv[j] = temp == 0x4000 ? double.NaN : temp * Common.Math.pow2_m24 * Physics.RANGE_MS;
                i += 15;
            }

            for (int j = 0; j < NCell; j++)
            {
                int temp = BitOperation.GetBitsInt(databody, i, 22);
                cpv[j] = temp == 0x200000 ? double.NaN : temp * Common.Math.pow2_m29 * Physics.RANGE_MS;
                i += 22;
            }

            for (int j = 0; j < NCell; j++)
            {
                plock[j] = BitOperation.GetBitsUint(databody, i, 4);
                i += 4;
            }

            for (int j = 0; j < NCell; j++)
            {
                half[j] = BitOperation.GetBitsUint(databody, i, 1);
                i += 1;
            }

            for (int j = 0; j < NCell; j++)
            {
                cnr[j] = BitOperation.GetBitsUint(databody, i, 6);
                i += 6;
            }

            for (int j = 0; j < NCell; j++)
            {
                FinePhaseRangeRate[j] = BitOperation.GetBitsInt(databody, i, 15) * 0.0001;
                i += 15;
            }
        }


        public override int GetEncodeBytesLength()
        {
            throw new NotImplementedException();
        }

        public override int Encode(ref Span<byte> bytes)
        {
            throw new NotImplementedException();
        }
    }
}
