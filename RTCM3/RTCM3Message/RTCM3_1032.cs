using RTCM3.Common;

namespace RTCM3.RTCM3Message
{
    public class RTCM3_1032 : RTCM3Base
    {
        public uint StationID;
        public uint ReferenceStationID;
        public uint ITRF;
        public double ReferenceStationX;
        public double ReferenceStationY;
        public double ReferenceStationZ;

        public RTCM3_1032(ReadOnlySpan<byte> databody)
        {
            int i = 0;
            int length;
            MessageType = BitOperation.GetBitsUint(databody, i, length = 12);
            i += length;
            StationID = BitOperation.GetBitsUint(databody, i, length = 12);
            i += length;
            ReferenceStationID = BitOperation.GetBitsUint(databody, i, length = 12);
            i += length;
            ITRF = BitOperation.GetBitsUint(databody, i, length = 6);
            i += length;
            ReferenceStationX = BitOperation.GetBitsLong(databody, i, length = 38) * 0.0001;
            i += length;
            ReferenceStationY = BitOperation.GetBitsLong(databody, i, length = 38) * 0.0001;
            i += length;
            ReferenceStationZ = BitOperation.GetBitsLong(databody, i, 38) * 0.0001;
        }

        public RTCM3_1032(uint StationID, uint ReferenceStationID, uint ITRF, double ReferenceStationX, double ReferenceStationY, double ReferenceStationZ)
        {
            MessageType = 1032;
            this.StationID = StationID;
            this.ReferenceStationID = ReferenceStationID;
            this.ITRF = ITRF;
            this.ReferenceStationX = ReferenceStationX;
            this.ReferenceStationY = ReferenceStationY;
            this.ReferenceStationZ = ReferenceStationZ;
        }

        public override Memory<byte> Encode()
        {
            throw new NotImplementedException();
        }
    }
}
