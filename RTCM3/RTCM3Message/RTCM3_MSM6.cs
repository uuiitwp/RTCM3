using RTCM3.Common;

namespace RTCM3.RTCM3Message
{
    public class RTCM3_MSM6 : RTCM3_MSM46
    {
        public RTCM3_MSM6(ReadOnlySpan<byte> databody) : base(databody)
        {

            for (int j = 0; j < NCell; j++)
            {
                prv[j] = BitOperation.GetBitsInt(databody, i, 20) * Common.Math.pow2_m29 * Common.Physics.RANGE_MS;
                i += 20;
            }

            for (int j = 0; j < NCell; j++)
            {
                cpv[j] = BitOperation.GetBitsInt(databody, i, 24) * Common.Math.pow2_m29 * Common.Physics.RANGE_MS;
                i += 24;
            }

            for (int j = 0; j < NCell; j++)
            {
                plock[j] = BitOperation.GetBitsUint(databody, i, 10);
                i += 10;
            }

            for (int j = 0; j < NCell; j++)
            {
                half[j] = BitOperation.GetBitsUint(databody, i, 1);
                i += 1;
            }

            for (int j = 0; j < NCell; j++)
            {
                cnr[j] = BitOperation.GetBitsUint(databody, i, 10);
                i += 10;
            }
        }


        public override int GetEncodeBytesLength()
        {
            int i = (int)(RTCM3HeaderBitsLength + 169 + Cell.Length + (18 * SatNumber));
            int bitLength = (int)(i + CRC24QBitsLength + (65 * NCell));
            int byteLength = (int)GetBodyBytesLength(bitLength);
            return byteLength;
        }

        public override void Encode(ref Span<byte> bytes)
        {
            int i = (int)(RTCM3HeaderBitsLength + 169 + Cell.Length + (18 * SatNumber));

            EncodeMSMHeader(ref bytes);
            EncodeSatData(ref bytes);
            for (int j = 0; j < NCell; j++)
            {
                BitOperation.SetBitsInt(ref bytes, i, 20, RoundToInt(prv[j] / Common.Math.pow2_m29 / Common.Physics.RANGE_MS));
                i += 20;
            }
            for (int j = 0; j < NCell; j++)
            {
                BitOperation.SetBitsInt(ref bytes, i, 24, RoundToInt(cpv[j] / Common.Math.pow2_m29 / Common.Physics.RANGE_MS));
                i += 24;
            }
            for (int j = 0; j < NCell; j++)
            {
                BitOperation.SetBitsUint(ref bytes, i, 10, plock[j]);
                i += 10;
            }
            for (int j = 0; j < NCell; j++)
            {
                BitOperation.SetBitsUint(ref bytes, i, 1, half[j]);
                i += 1;
            }
            for (int j = 0; j < NCell; j++)
            {
                BitOperation.SetBitsUint(ref bytes, i, 10, cnr[j]);
                i += 10;
            }
            EncodeRTCM3(ref bytes, i - 24);
        }
    }
}
