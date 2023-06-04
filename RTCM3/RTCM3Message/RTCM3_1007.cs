using System.Text;
using RTCM3.Common;

namespace RTCM3.RTCM3Message
{
    public class RTCM3_1007 : RTCM3Base
    {
        public uint StationID;
        public uint AntDescriptorCounter;
        public string AntDescriptor;
        public uint AntSetupID;

        public RTCM3_1007(ReadOnlySpan<byte> databody)
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
            AntDescriptor = Encoding.ASCII.GetString(databody[antDescriptorPosition..(int)(antDescriptorPosition + AntDescriptorCounter)]);
            i += (int)AntDescriptorCounter * 8;
            AntSetupID = BitOperation.GetBitsUint(databody, i, 8);
        }

        public RTCM3_1007()
        {
            MessageType = 1007;
            AntDescriptor = string.Empty;
        }

        public override Memory<byte> Encode()
        {
            Memory<byte> result = new(new byte[((40 + RTCM3HeaderBitsLength + CRC24QBitsLength) / 8) + AntDescriptorCounter]);
            int i = 24;
            int length;
            BitOperation.SetBitsUint(ref result, i, length = 12, MessageType);
            i += length;
            BitOperation.SetBitsUint(ref result, i, length = 12, StationID);
            i += length;
            BitOperation.SetBitsUint(ref result, i, length = 8, AntDescriptorCounter);
            i += length;
            int AntDescriptorPosition = i / 8;
            Encoding.ASCII.GetBytes(AntDescriptor).CopyTo(result[AntDescriptorPosition..(int)(AntDescriptorPosition + AntDescriptorCounter)]);
            i += (int)AntDescriptorCounter * 8;
            BitOperation.SetBitsUint(ref result, i, length = 8, AntSetupID);
            i += length;
            EncodeRTCM3(ref result, i - 24);
            return result;
        }
    }
}
