using RTCM3.Common;
using RTCM3.RTCM3Message;
using System.Buffers;

namespace RTCM3
{
    public class RTCM3
    {
        private const uint Preamble = 0xd3;
        private const uint Reserved = 0;
        private readonly uint DataBytesLength;
        public readonly uint MessageType;
        public readonly RTCM3Base? Databody;
        public RTCM3(ReadOnlySequence<byte> bytes, bool checkCRC24Q = true, bool decode = true)
        {
            if (bytes.Length < 6)
            {
                throw new InvalidCastException($"invalid Length:{bytes.Length}");
            }
            uint preamble = BitOperation.GetBitsUint(bytes, 0, 8);
            if (preamble != Preamble)
            {
                throw new InvalidCastException($"invalid preamble:{preamble:X2}");
            }
            uint reserved = BitOperation.GetBitsUint(bytes, 8, 6);
            if (reserved != Reserved)
            {
                throw new InvalidCastException($"invalid reserved:{reserved:X2}");
            }
            DataBytesLength = BitOperation.GetBitsUint(bytes, 14, 10);
            if (bytes.Length < DataBytesLength + 6)
            {
                throw new InvalidCastException($"invalid data bytes length:{DataBytesLength}");
            }
            if (checkCRC24Q && (!ValidCRC24Q(bytes)))
            {
                throw new InvalidCastException("invalid CRC24Q");
            }
            MessageType = BitOperation.GetBitsUint(bytes, 24, 12);
            if (decode)
            {
                ReadOnlySequence<byte> body = bytes.Slice(3, (int)(3 + DataBytesLength));
                Databody = MessageType switch
                {
                    1001 => new RTCM3_1001(body),
                    1002 => new RTCM3_1002(body),
                    1003 => new RTCM3_1003(body),
                    1004 => new RTCM3_1004(body),
                    1005 => new RTCM3_1005(body),
                    1006 => new RTCM3_1006(body),
                    1007 => new RTCM3_1007(body),
                    1008 => new RTCM3_1008(body),
                    1012 => new RTCM3_1012(body),
                    1019 => new RTCM3_1019(body),
                    1020 => new RTCM3_1020(body),
                    1032 => new RTCM3_1032(body),
                    1033 => new RTCM3_1033(body),
                    1042 => new RTCM3_1042(body),
                    1044 => new RTCM3_1044(body),
                    1074 => new RTCM3_MSM4(body),
                    1075 => new RTCM3_MSM5(body),
                    1076 => new RTCM3_MSM6(body),
                    1084 => new RTCM3_MSM4(body),
                    1085 => new RTCM3_MSM5(body),
                    1086 => new RTCM3_MSM6(body),
                    1094 => new RTCM3_MSM4(body),
                    1095 => new RTCM3_MSM5(body),
                    1096 => new RTCM3_MSM6(body),
                    1114 => new RTCM3_MSM4(body),
                    1115 => new RTCM3_MSM5(body),
                    1116 => new RTCM3_MSM6(body),
                    1124 => new RTCM3_MSM4(body),
                    1125 => new RTCM3_MSM5(body),
                    1126 => new RTCM3_MSM6(body),
                    1230 => new RTCM3_1230(body),
                    4068 => new RTCM3_4068(body),
                    _ => default,
                };
            }
        }

        private bool ValidCRC24Q(ReadOnlySequence<byte> bytes)
        {
            int crcposition = (int)(DataBytesLength + 3);
            return CRC24Q.Get(bytes.Slice(0, crcposition)) == BitOperation.GetBitsUint(bytes.Slice(crcposition), 0, 24);
        }
        public static RTCM3? Filter(ref ReadOnlySequence<byte> buffer)
        {
            RTCM3? result = null;
            SequenceReader<byte> reader = new(buffer);
            if (reader.TryAdvanceTo((byte)Preamble, false))
            {
                long Consumed = reader.Consumed;
                buffer = buffer.Slice(Consumed);
                if (reader.Remaining < 3)
                {
                    buffer = buffer.Slice(buffer.Length);
                    return null;
                }

                if (BitOperation.GetBitsUint(buffer, 8, 6) == Reserved)
                {
                    uint len = BitOperation.GetBitsUint(buffer, 14, 10) + 6;
                    if (reader.Remaining < len)
                    {
                        buffer = buffer.Slice(buffer.Length);
                        return null;
                    }
                    try
                    {
                        result = new RTCM3(buffer);
                        buffer = buffer.Slice(len, 0);
                    }
                    catch
                    {
                        result = null;
                        buffer = buffer.Slice(1, 0);
                    }
                }
                else
                {
                    buffer = buffer.Slice(3, 0);
                }
            }
            else
            {
                buffer = buffer.Slice(buffer.Length, 0);
            }
            return result;
        }
    }
}
