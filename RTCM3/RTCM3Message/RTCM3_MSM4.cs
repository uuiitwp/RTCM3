﻿using RTCM3.Common;
using System.Buffers;

namespace RTCM3.RTCM3Message
{
    public class RTCM3_MSM4 : RTCM3_MSM46
    {
        public RTCM3_MSM4(ReadOnlySequence<byte> databody) : base(databody)
        {

            for (int j = 0; j < NCell; j++)
            {
                int temp = BitOperation.GetBitsInt(databody, i, 15);
                prv[j] = temp == 0x4000 ? double.NaN : temp * Common.Math.pow2_m24 * Common.Physics.RANGE_MS;
                i += 15;
            }

            for (int j = 0; j < NCell; j++)
            {
                int temp = BitOperation.GetBitsInt(databody, i, 22);
                cpv[j] = temp == 0x200000 ? double.NaN : temp * Common.Math.pow2_m29 * Common.Physics.RANGE_MS;
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
        }


        public override int GetEncodeBytesLength()
        {
            int i = (int)(RTCM3HeaderBitsLength + 169 + Cell.Length + (18 * SatNumber));
            int bitLength = (int)(i + CRC24QBitsLength + (48 * NCell));
            int byteLength = (int)GetBodyBytesLength(bitLength);
            return byteLength;
        }

        public override int Encode(ref Span<byte> bytes)
        {
            int result = GetEncodeBytesLength();
            bytes[..result].Clear();
            int i = (int)(RTCM3HeaderBitsLength + 169 + Cell.Length + (18 * SatNumber));
            EncodeMSMHeader(ref bytes);
            EncodeSatData(ref bytes);
            for (int j = 0; j < NCell; j++)
            {
                int temp = double.IsNaN(prv[j]) ? 0x4000 : RoundToInt(prv[j] / Common.Math.pow2_m24 / Common.Physics.RANGE_MS);
                BitOperation.SetBitsInt(ref bytes, i, 15, temp);
                i += 15;
            }
            for (int j = 0; j < NCell; j++)
            {
                int temp = double.IsNaN(cpv[j]) ? 0x200000 : RoundToInt(cpv[j] / Common.Math.pow2_m29 / Common.Physics.RANGE_MS);
                BitOperation.SetBitsInt(ref bytes, i, 22, temp);
                i += 22;
            }
            for (int j = 0; j < NCell; j++)
            {
                BitOperation.SetBitsUint(ref bytes, i, 4, plock[j]);
                i += 4;
            }
            for (int j = 0; j < NCell; j++)
            {
                BitOperation.SetBitsUint(ref bytes, i, 1, half[j]);
                i += 1;
            }
            for (int j = 0; j < NCell; j++)
            {
                BitOperation.SetBitsUint(ref bytes, i, 6, cnr[j]);
                i += 6;
            }
            EncodeRTCM3(ref bytes, i - 24);
            return result;
        }
    }
}
