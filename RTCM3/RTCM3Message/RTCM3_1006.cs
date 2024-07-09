using RTCM3.Common;
using System.Buffers;

namespace RTCM3.RTCM3Message
{
    public class RTCM3_1006 : RTCM3Base
    {
        public uint StationID;
        public uint ITRF;
        public uint GPS;
        public uint Glonass;
        public uint Galileo;
        public uint ReferenceStation;
        public double X;
        public uint OSC;
        public uint Beidou;
        public double Y;
        public uint QuartCycle;
        public double Z;
        public double ARPHeight;

        private const int DataBitsLength = 168;

        public RTCM3_1006(ReadOnlySequence<byte> databody)
        {
            int i = 0;
            int length;
            MessageType = BitOperation.GetBitsUint(databody, i, length = 12);
            i += length;
            StationID = BitOperation.GetBitsUint(databody, i, length = 12);
            i += length;
            ITRF = BitOperation.GetBitsUint(databody, i, length = 6);
            i += length;
            GPS = BitOperation.GetBitsUint(databody, i, length = 1);
            i += length;
            Glonass = BitOperation.GetBitsUint(databody, i, length = 1);
            i += length;
            Galileo = BitOperation.GetBitsUint(databody, i, length = 1);
            i += length;
            ReferenceStation = BitOperation.GetBitsUint(databody, i, length = 1);
            i += length;
            X = BitOperation.GetBitsLong(databody, i, length = 38) * 0.0001;
            i += length;
            OSC = BitOperation.GetBitsUint(databody, i, length = 1);
            i += length;
            Beidou = BitOperation.GetBitsUint(databody, i, length = 1);
            i += length;
            Y = BitOperation.GetBitsLong(databody, i, length = 38) * 0.0001;
            i += length;
            QuartCycle = BitOperation.GetBitsUint(databody, i, length = 2);
            i += length;
            Z = BitOperation.GetBitsLong(databody, i, length = 38) * 0.0001;
            i += length;
            ARPHeight = BitOperation.GetBitsUint(databody, i, 16) * 0.0001;
        }
        public RTCM3_1006()
        {
            MessageType = 1006;
        }

        public override int GetEncodeBytesLength()
        {
            return (RTCM3HeaderBitsLength + CRC24QBitsLength + DataBitsLength) / 8;
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
            BitOperation.SetBitsUint(ref bytes, i, length = 6, ITRF);
            i += length;
            BitOperation.SetBitsUint(ref bytes, i, length = 1, GPS);
            i += length;
            BitOperation.SetBitsUint(ref bytes, i, length = 1, Glonass);
            i += length;
            BitOperation.SetBitsUint(ref bytes, i, length = 1, Galileo);
            i += length;
            BitOperation.SetBitsUint(ref bytes, i, length = 1, ReferenceStation);
            i += length;
            BitOperation.SetBitsLong(ref bytes, i, length = 38, RoundToLong(X / 0.0001));
            i += length;
            BitOperation.SetBitsUint(ref bytes, i, length = 1, OSC);
            i += length;
            BitOperation.SetBitsUint(ref bytes, i, length = 1, Beidou);
            i += length;
            BitOperation.SetBitsLong(ref bytes, i, length = 38, RoundToLong(Y / 0.0001));
            i += length;
            BitOperation.SetBitsUint(ref bytes, i, length = 2, QuartCycle);
            i += length;
            BitOperation.SetBitsLong(ref bytes, i, length = 38, RoundToLong(Z / 0.0001));
            i += length;
            BitOperation.SetBitsUint(ref bytes, i, length = 16, (uint)RoundToLong(ARPHeight / 0.0001));
            i += length;
            EncodeRTCM3(ref bytes, i - 24);
            return result;
        }

    }
}
