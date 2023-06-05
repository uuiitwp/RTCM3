using RTCM3.Common;
using System.Collections;

namespace RTCM3.RTCM3Message
{
    public abstract class RTCM3_MSM : RTCM3Base
    {
        public uint StationID;
        public uint GNSStime;
        public uint Sync;
        public uint IODS;
        public uint Reserved;
        public uint ClockSync;
        public uint ClockExt;
        public uint Smooth;
        public uint TintS;
        public ulong GNSSSatMask;
        public uint GNSSSigMask;
        protected int i;
        public uint SatNumber;
        public uint SigNumber;
        public uint NCell;
        public BitArray Cell;

        private static readonly uint[] GPSMessages = new uint[] { 1071u, 1072u, 1073u, 1074u, 1075u, 1076u, 1077u, };
        private static readonly uint[] GLONASSMessages = new uint[] { 1081u, 1082u, 1083u, 1084u, 1085u, 1086u, 1087u, };
        private static readonly uint[] GALILEOMessages = new uint[] { 1091u, 1092u, 1093u, 1094u, 1095u, 1096u, 1097u, };
#pragma warning disable IDE0052 
        private static readonly uint[] SBASMessages = new uint[] { 1101u, 1102u, 1103u, 1104u, 1105u, 1106u, 1107u, };
#pragma warning restore IDE0052 
        private static readonly uint[] QZSSMessages = new uint[] { 1111u, 1112u, 1113u, 1114u, 1115u, 1116u, 1117u, };
        private static readonly uint[] BDSMessages = new uint[] { 1121u, 1122u, 1123u, 1124u, 1125u, 1126u, 1127u, };
        public RTCM3_MSM(ReadOnlySpan<byte> databody)
        {
            int length;
            MessageType = BitOperation.GetBitsUint(databody, i, length = 12);
            i += length;
            StationID = BitOperation.GetBitsUint(databody, i, length = 12);
            i += length;
            GNSStime = BitOperation.GetBitsUint(databody, i, length = 30);
            i += length;
            Sync = BitOperation.GetBitsUint(databody, i, length = 1);
            i += length;
            IODS = BitOperation.GetBitsUint(databody, i, length = 3);
            i += length;
            Reserved = BitOperation.GetBitsUint(databody, i, length = 7);
            i += length;
            ClockSync = BitOperation.GetBitsUint(databody, i, length = 2);
            i += length;
            ClockExt = BitOperation.GetBitsUint(databody, i, length = 2);
            i += length;
            Smooth = BitOperation.GetBitsUint(databody, i, length = 1);
            i += length;
            TintS = BitOperation.GetBitsUint(databody, i, length = 3);
            i += length;
            GNSSSatMask = BitOperation.GetBitsUlong(databody, i, length = 64);
            i += length;
            GNSSSigMask = BitOperation.GetBitsUint(databody, i, length = 32);
            i += length;
            for (int j = 0; j < 64; j++)
            {
                ulong mask = ((1ul << j) & GNSSSatMask) >> j;
                SatNumber += (uint)mask;
            }
            for (int j = 0; j < 32; j++)
            {
                uint mask = ((1u << j) & GNSSSigMask) >> j;
                SigNumber += mask;
            }

            Cell = new BitArray((int)(SatNumber * SigNumber));
            for (int j = 0; j < Cell.Length; j++)
            {
                bool temp = BitOperation.GetBitsUint(databody, i, length = 1) > 0;
                if (temp)
                {
                    NCell++;
                    Cell.Set(j, temp);
                }
                i += length;
            }
        }

        public abstract Observation[] GetObservations();

        protected int[] GetMaskedSatIds()
        {
            int[] satIds = new int[SatNumber];
            for (int i = 0, index = 0; i < 64; i++)
            {
                if (((1ul << i) & GNSSSatMask) > 0)
                {
                    satIds[SatNumber - ++index] = 64 - i;
                }
            }
            return satIds;
        }

        protected int[] GetMaskedSigIds()
        {
            int[] sigIds = new int[SigNumber];
            for (int i = 0, index = 0; i < 32; i++)
            {
                if (((1u << i) & GNSSSigMask) > 0)
                {
                    sigIds[SigNumber - ++index] = 32 - i;
                }
            }
            return sigIds;
        }

        protected GNSSSystem GetGNSSSystem()
        {
            if (GPSMessages.Contains(MessageType))
            {
                return GNSSSystem.GPS;
            }
            else if (GLONASSMessages.Contains(MessageType))
            {
                return GNSSSystem.GLONASS;
            }
            else if (GALILEOMessages.Contains(MessageType))
            {
                return GNSSSystem.GALILEO;
            }
            else if (QZSSMessages.Contains(MessageType))
            {
                return GNSSSystem.QZSS;
            }
            else if (BDSMessages.Contains(MessageType))
            {
                return GNSSSystem.BEIDOU;
            }
            else
            {
                return GNSSSystem.None;
            }
        }

        protected void EncodeMSMHeader(ref Memory<byte> bytes)
        {
            int i = 24;
            int length;
            BitOperation.SetBitsUint(ref bytes, i, length = 12, MessageType);
            i += length;
            BitOperation.SetBitsUint(ref bytes, i, length = 12, StationID);
            i += length;
            BitOperation.SetBitsUint(ref bytes, i, length = 30, GNSStime);
            i += length;
            BitOperation.SetBitsUint(ref bytes, i, length = 1, Sync);
            i += length;
            BitOperation.SetBitsUint(ref bytes, i, length = 3, IODS);
            i += length;
            BitOperation.SetBitsUint(ref bytes, i, length = 7, Reserved);
            i += length;
            BitOperation.SetBitsUint(ref bytes, i, length = 2, ClockSync);
            i += length;
            BitOperation.SetBitsUint(ref bytes, i, length = 2, ClockExt);
            i += length;
            BitOperation.SetBitsUint(ref bytes, i, length = 1, Smooth);
            i += length;
            BitOperation.SetBitsUint(ref bytes, i, length = 3, TintS);
            i += length;
            BitOperation.SetBitsUlong(ref bytes, i, length = 64, GNSSSatMask);
            i += length;
            BitOperation.SetBitsUint(ref bytes, i, length = 32, GNSSSigMask);
            i += length;

            for (int j = 0; j < Cell.Length; j++)
            {
                bool temp = Cell.Get(j);
                if (temp)
                {
                    BitOperation.SetBitsUint(ref bytes, i, length = 1, 1);
                }
                i += length;
            }
        }
    }
}
