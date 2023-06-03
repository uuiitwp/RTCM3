namespace RTCM3.Common
{
    [Flags]
    public enum GNSSSystem
    {
        None = 0b0000_0000,
        GPS = 0b0000_0001,
        SBAS = 0b0000_0010,
        GLONASS = 0b0000_0100,
        GALILEO = 0b0000_1000,
        BEIDOU = 0b0001_0000,
        QZSS = 0b0010_0000,
        IRNSS = 0b0100_0000,
    }
    public static class GNSSSystemMethod
    {
        public static char GetGNSSSystemChar(GNSSSystem GNSSSystem)
        {
            return GNSSSystem switch
            {
                GNSSSystem.GPS => 'G',
                GNSSSystem.GLONASS => 'R',
                GNSSSystem.GALILEO => 'E',
                GNSSSystem.BEIDOU => 'C',
                GNSSSystem.QZSS => 'J',
                GNSSSystem.IRNSS => 'I',
                _ => throw new InvalidCastException($"{GNSSSystem} is not supported"),
            };
        }
    }
}
