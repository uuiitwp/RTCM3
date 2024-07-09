using RTCM3.Common;
using System.Buffers;
using System.Text;

namespace RTCM3.RTCM3Message
{
    public class RTCM3_1008 : RTCM3Base
    {
        public uint StationID;
        public uint AntDescriptorCounter;
        public string AntDescriptor;
        public uint AntSetupID;
        public uint AntennaSerialNumberCounter;
        public string AntennaSerialNumber;

        public RTCM3_1008(ReadOnlySequence<byte> databody)
        {
            int i = 0;
            int length;
            MessageType = BitOperation.GetBitsUint(databody, i, length = 12);
            i += length;
            StationID = BitOperation.GetBitsUint(databody, i, length = 12);
            i += length;
            AntDescriptorCounter = BitOperation.GetBitsUint(databody, i, length = 8);
            i += length;
            int antDescriptorPosition = i / 8;
            AntDescriptor = Encoding.ASCII.GetString(databody.Slice(antDescriptorPosition,  AntDescriptorCounter));
            i += (int)AntDescriptorCounter * 8;
            AntSetupID = BitOperation.GetBitsUint(databody, i, length = 8);
            i += length;
            AntennaSerialNumberCounter = BitOperation.GetBitsUint(databody, i, 8);
            int antennaSerialNumberPosition = i / 8;
            AntennaSerialNumber = Encoding.ASCII.GetString(databody.Slice(antennaSerialNumberPosition,  AntennaSerialNumberCounter));
        }

        public RTCM3_1008()
        {
            MessageType = 1008;
            AntDescriptor = string.Empty;
            AntennaSerialNumber = string.Empty;

        }

        public override int GetEncodeBytesLength()
        {
            return ((48 + RTCM3HeaderBitsLength + CRC24QBitsLength) / 8) + (int)(AntDescriptorCounter + AntennaSerialNumberCounter);
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
            BitOperation.SetBitsUint(ref bytes, i, length = 8, AntDescriptorCounter);
            i += length;
            int AntDescriptorPosition = i / 8;
            Encoding.ASCII.GetBytes(AntDescriptor).CopyTo(bytes[AntDescriptorPosition..(int)(AntDescriptorPosition + AntDescriptorCounter)]);
            i += (int)AntDescriptorCounter * 8;
            BitOperation.SetBitsUint(ref bytes, i, length = 8, AntSetupID);
            i += length;
            BitOperation.SetBitsUint(ref bytes, i, length = 8, AntennaSerialNumberCounter);
            i += length;
            int antennaSerialNumberPosition = i / 8;
            Encoding.ASCII.GetBytes(AntennaSerialNumber).CopyTo(bytes[antennaSerialNumberPosition..(int)(antennaSerialNumberPosition + AntennaSerialNumberCounter)]);
            i += (int)AntennaSerialNumberCounter * 8;
            EncodeRTCM3(ref bytes, i - 24);
            return result;
        }
    }
}
