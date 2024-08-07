﻿using RTCM3.Common;
using System.Buffers;
using System.Text;

namespace RTCM3.RTCM3Message
{
    public class RTCM3_1033 : RTCM3Base
    {
        public uint StationID;
        public uint AntDescriptorCounter;
        public string AntDescriptor;
        public uint AntSetupID;
        public uint AntSerialNumberCounter;
        public string AntSerialNumber;
        public uint ReceiverDescriptorCounter;
        public string ReceiverDescriptor;
        public uint ReceiverFirmwareVersionCounter;
        public string ReceiverFirmwareVersion;
        public uint ReceiverSerialNumberCounter;
        public string ReceiverSerialNumber;
        public RTCM3_1033(ReadOnlySequence<byte> databody)
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
            AntDescriptor = Encoding.ASCII.GetString(databody.Slice(antDescriptorPosition, AntDescriptorCounter));
            i += (int)AntDescriptorCounter * 8;

            AntSetupID = BitOperation.GetBitsUint(databody, i, length = 8);
            i += length;

            AntSerialNumberCounter = BitOperation.GetBitsUint(databody, i, length = 8);
            i += length;
            int antSerialNumberPosition = i / 8;
            AntSerialNumber = Encoding.ASCII.GetString(databody.Slice(antSerialNumberPosition, AntSerialNumberCounter));
            i += (int)AntSerialNumberCounter * 8;

            ReceiverDescriptorCounter = BitOperation.GetBitsUint(databody, i, length = 8);
            i += length;
            int receiverDescriptorPosition = i / 8;
            ReceiverDescriptor = Encoding.ASCII.GetString(databody.Slice(receiverDescriptorPosition, ReceiverDescriptorCounter));
            i += (int)ReceiverDescriptorCounter * 8;

            ReceiverFirmwareVersionCounter = BitOperation.GetBitsUint(databody, i, length = 8);
            i += length;
            int receiverFirmwareVersionPosition = i / 8;
            ReceiverFirmwareVersion = Encoding.ASCII.GetString(databody.Slice(receiverFirmwareVersionPosition, ReceiverFirmwareVersionCounter));
            i += (int)ReceiverFirmwareVersionCounter * 8;

            ReceiverSerialNumberCounter = BitOperation.GetBitsUint(databody, i, length = 8);
            i += length;
            int receiverSerialNumberPosition = i / 8;
            ReceiverSerialNumber = Encoding.ASCII.GetString(databody.Slice(receiverSerialNumberPosition, ReceiverSerialNumberCounter));
        }

        public RTCM3_1033()
        {
            MessageType = 1033;
            AntDescriptor = string.Empty;
            AntSerialNumber = string.Empty;
            ReceiverDescriptor = string.Empty;
            ReceiverFirmwareVersion = string.Empty;
            ReceiverSerialNumber = string.Empty;
        }

        public override int GetEncodeBytesLength()
        {
            uint bitsLength = 72 + RTCM3HeaderBitsLength + CRC24QBitsLength + (8 * (AntDescriptorCounter + AntSerialNumberCounter + ReceiverDescriptorCounter + ReceiverFirmwareVersionCounter + ReceiverSerialNumberCounter));
            uint bytesLength = bitsLength / 8;
            return (int)bytesLength;
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
            int antDescriptorPosition = i / 8;
            Encoding.ASCII.GetBytes(AntDescriptor).CopyTo(bytes[antDescriptorPosition..(int)(antDescriptorPosition + AntDescriptorCounter)]);
            i += (int)AntDescriptorCounter * 8;
            BitOperation.SetBitsUint(ref bytes, i, length = 8, AntSetupID);
            i += length;
            BitOperation.SetBitsUint(ref bytes, i, length = 8, AntSerialNumberCounter);
            i += length;
            int antSerialNumberPosition = i / 8;
            Encoding.ASCII.GetBytes(AntSerialNumber).CopyTo(bytes[antSerialNumberPosition..(int)(antSerialNumberPosition + AntSerialNumberCounter)]);
            i += (int)AntSerialNumberCounter * 8;
            BitOperation.SetBitsUint(ref bytes, i, length = 8, ReceiverDescriptorCounter);
            i += length;
            int receiverDescriptorPosition = i / 8;
            Encoding.ASCII.GetBytes(ReceiverDescriptor).CopyTo(bytes[receiverDescriptorPosition..(int)(receiverDescriptorPosition + ReceiverDescriptorCounter)]);
            i += (int)ReceiverDescriptorCounter * 8;

            BitOperation.SetBitsUint(ref bytes, i, length = 8, ReceiverFirmwareVersionCounter);
            i += length;
            int receiverFirmwareVersionPosition = i / 8;
            Encoding.ASCII.GetBytes(ReceiverFirmwareVersion).CopyTo(bytes[receiverFirmwareVersionPosition..(int)(receiverFirmwareVersionPosition + ReceiverFirmwareVersionCounter)]);
            i += (int)ReceiverFirmwareVersionCounter * 8;
            BitOperation.SetBitsUint(ref bytes, i, length = 8, ReceiverSerialNumberCounter);
            i += length;
            int receiverSerialNumberPosition = i / 8;
            Encoding.ASCII.GetBytes(ReceiverSerialNumber).CopyTo(bytes[receiverSerialNumberPosition..(int)(receiverSerialNumberPosition + ReceiverSerialNumberCounter)]);
            i += (int)ReceiverSerialNumberCounter * 8;
            EncodeRTCM3(ref bytes, i - 24);
            return result;
        }
    }
}
