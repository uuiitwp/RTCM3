namespace RTCM3.Common
{
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
