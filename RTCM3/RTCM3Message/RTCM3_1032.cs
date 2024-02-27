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

        private const int DataBitsLength = 156;

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

        public override int Encode(ref Span<byte> bytes)
        {
            int result = GetEncodeBytesLength();
            bytes[..result].Clear();
            int i = 24;
            int length;
            BitOperation.SetBitsUint(ref bytes, i, length = 12, MessageType);
            i += length;
            BitOperation.SetBitsUint(ref bytes, i, length = 12, StationID);
            i += length;
            BitOperation.SetBitsUint(ref bytes, i, length = 12, ReferenceStationID);
            i += length;
            BitOperation.SetBitsUint(ref bytes, i, length = 6, ITRF);
            i += length;
            BitOperation.SetBitsLong(ref bytes, i, length = 38, RoundToLong(ReferenceStationX / 0.0001));
            i += length;
            BitOperation.SetBitsLong(ref bytes, i, length = 38, RoundToLong(ReferenceStationY / 0.0001));
            i += length;
            BitOperation.SetBitsLong(ref bytes, i, length = 38, RoundToLong(ReferenceStationZ / 0.0001));
            i += length;
            EncodeRTCM3(ref bytes, i - 24);
            return result;
        }
        public override int GetEncodeBytesLength()
        {
            return (RTCM3HeaderBitsLength + CRC24QBitsLength + DataBitsLength + 4) / 8;
        }
    }
}
