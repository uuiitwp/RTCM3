namespace RTCM3.Common
{
    public class UnsupportedGNSSSystem(GNSSSystem GNSSSystem) : Exception($"{GNSSSystem} is not supported")
    {
    }

    [Flags]
    public enum GNSSSystem
    {
        None = 0b0000_0000, // 0
        GPS = 0b0000_0001, // 1
        SBAS = 0b0000_0010, // 2
        GLONASS = 0b0000_0100, // 4
        GALILEO = 0b0000_1000, // 8
        BEIDOU = 0b0001_0000, // 16
        QZSS = 0b0010_0000, // 32
        IRNSS = 0b0100_0000, //64
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
                GNSSSystem.SBAS => 'S',
                _ => throw new UnsupportedGNSSSystem(GNSSSystem),
            };
        }
    }
}
