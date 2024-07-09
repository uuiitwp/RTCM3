using RTCM3.Common;
using System.Buffers;

namespace RTCM3.RTCM3Message
{
    public abstract class RTCM3_1001to1004 : RTCM3Base, IObservations
    {
        public uint StationID;
        public uint GPStime;
        public uint Sync;
        public uint GPSSatNumber;
        public uint SmoothIndicator;
        public uint SmoothInterval;
        protected int i;

        public RTCM3_1001to1004(ReadOnlySequence<byte> databody)
        {
            int length;
            MessageType = BitOperation.GetBitsUint(databody, i, length = 12);
            i += length;
            StationID = BitOperation.GetBitsUint(databody, i, length = 12);
            i += length;
            GPStime = BitOperation.GetBitsUint(databody, i, length = 30);
            i += length;
            Sync = BitOperation.GetBitsUint(databody, i, length = 1);
            i += length;
            GPSSatNumber = BitOperation.GetBitsUint(databody, i, length = 5);
            i += length;
            SmoothIndicator = BitOperation.GetBitsUint(databody, i, length = 1);
            i += length;
            SmoothInterval = BitOperation.GetBitsUint(databody, i, length = 3);
            i += length;
        }

        public abstract Observation[] GetObservations();

        protected static GNSSSystem GetGNSSSystem() => GNSSSystem.GPS;
    }
}
