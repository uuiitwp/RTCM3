using RTCM3.Common;

namespace RTCM3.RTCM3Message
{
    public class RTCM3_4068 : RTCM3Base
    {
        public uint QXMessageType;
        public uint MessageLength;
        public uint MessageHeaderLength;
        public uint GNSStime;
        public uint StationID;
        public uint Sync;
        public ulong GNSSSatMask;
        public uint SatNumber;
        public ulong IonoMask;
        public ulong TropMask;
        public uint SatIonoNumber;
        public uint SatTropNumber;
        public double[] IonoResidual;
        public double[] TropResidual;
        public int[] SatArry;
        public int[] SatIonoArry;
        public int[] SatTropArry;

        public RTCM3_4068(ReadOnlySpan<byte> databody)
        {
            int i = 0;
            int length;
            MessageType = BitOperation.GetBitsUint(databody, i, length = 12);
            i += length;
            QXMessageType = BitOperation.GetBitsUint(databody, i, length = 12);
            i += length;
            MessageLength = BitOperation.GetBitsUint(databody, i, length = 10);
            i += length;
            MessageHeaderLength = BitOperation.GetBitsUint(databody, i, length = 10);
            i += length;
            GNSStime = BitOperation.GetBitsUint(databody, i, length = 30);
            i += length;
            StationID = BitOperation.GetBitsUint(databody, i, length = 12);
            i += length;
            Sync = BitOperation.GetBitsUint(databody, i, length = 1);
            i += length;
            GNSSSatMask = BitOperation.GetBitsUlong(databody, i, length = 64);
            i += length;
            for (int j = 0; j < 64; j++)
            {
                ulong mask = ((1ul << j) & GNSSSatMask) >> j;
                SatNumber += (uint)mask;
            }
            SatArry = new int[SatNumber];
            for (int j = 0, k = 0; j < 64; j++)
            {
                ulong mask = ((1ul << j) & GNSSSatMask) >> j;
                if (mask == 1u)
                {
                    SatArry[k] = 64 - j;
                    k++;
                }
            }
            IonoMask = BitOperation.GetBitsUlong(databody, i, length = (int)SatNumber);
            i += length;
            TropMask = BitOperation.GetBitsUlong(databody, i, (int)SatNumber);
            for (int j = 0; j < SatNumber; j++)
            {
                ulong mask = ((1ul << j) & IonoMask) >> j;
                SatIonoNumber += (uint)mask;
            }
            IonoResidual = new double[SatIonoNumber];
            SatIonoArry = new int[SatIonoNumber];

            for (int j = 0; j < SatNumber; j++)
            {
                ulong mask = ((1ul << j) & TropMask) >> j;
                SatTropNumber += (uint)mask;
            }
            TropResidual = new double[SatTropNumber];
            SatTropArry = new int[SatTropNumber];

            for (int j = 0, k = 0; j < SatIonoNumber; j++)
            {
                ulong mask = ((1ul << j) & IonoMask) >> j;
                if (mask == 1u)
                {
                    SatIonoArry[k] = SatArry[j];
                    k++;
                }
            }
            for (int j = 0, k = 0; j < SatTropNumber; j++)
            {
                ulong mask = ((1ul << j) & TropMask) >> j;
                if (mask == 1u)
                {
                    SatTropArry[k] = SatArry[j];
                    k++;
                }
            }

            i = (int)MessageHeaderLength * 8;

            for (int j = 0; j < SatIonoNumber; j++)
            {
                uint temp = BitOperation.GetBitsUint(databody, i, length = 11);
                IonoResidual[j] = GetResidual(temp);
                i += length;
            }
            for (int j = 0; j < SatTropNumber; j++)
            {
                uint temp = BitOperation.GetBitsUint(databody, i, length = 11);
                TropResidual[j] = GetResidual(temp);
                i += length;
            }
        }

        private static double GetResidual(uint value)
        {
            return value switch
            {
                >= 0 and <= 999 => value * 0.0001,
                >= 1000 and <= 1899 => 0.1 + ((value - 1000) * 0.001),
                >= 1900 and <= 1999 => 1 + ((value - 1900) * 0.01),
                >= 2000 and <= 2046 => 2 + ((value - 2000) * 0.1),
                _ => 9999.99,
            };
        }

        public GNSSSystem GetSystem()
        {
            return QXMessageType switch
            {
                101u => GNSSSystem.GPS,
                102u => GNSSSystem.GLONASS,
                103u => GNSSSystem.GALILEO,
                104u => GNSSSystem.QZSS,
                105u => GNSSSystem.BEIDOU,
                _ => GNSSSystem.None,
            };
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
