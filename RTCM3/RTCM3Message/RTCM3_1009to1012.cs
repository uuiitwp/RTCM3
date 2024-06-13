using RTCM3.Common;

namespace RTCM3.RTCM3Message
{
    public abstract class RTCM3_1009to1012 : RTCM3Base, IObservations
    {
        public uint StationID;
        public uint GLONASStime;
        public uint Sync;
        public uint GLONASSSatNumber;
        public uint SmoothIndicator;
        public uint SmoothInterval;
        protected int i;

        public RTCM3_1009to1012(ReadOnlySpan<byte> databody)
        {
            int length;
            MessageType = BitOperation.GetBitsUint(databody, i, length = 12);
            i += length;
            StationID = BitOperation.GetBitsUint(databody, i, length = 12);
            i += length;
            GLONASStime = BitOperation.GetBitsUint(databody, i, length = 27);
            i += length;
            Sync = BitOperation.GetBitsUint(databody, i, length = 1);
            i += length;
            GLONASSSatNumber = BitOperation.GetBitsUint(databody, i, length = 5);
            i += length;
            SmoothIndicator = BitOperation.GetBitsUint(databody, i, length = 1);
            i += length;
            SmoothInterval = BitOperation.GetBitsUint(databody, i, length = 3);
            i += length;
        }

        public abstract Observation[] GetObservations();
        protected static GNSSSystem GetGNSSSystem() => GNSSSystem.GLONASS;
    }
}
