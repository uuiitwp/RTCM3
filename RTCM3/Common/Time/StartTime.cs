namespace RTCM3.Common.Time
{
    public static class StartTime
    {
        public static readonly DateTime GPS_START_TIME = new(1980, 1, 6);
        public static readonly DateTime GALILEO_START_TIME = new(1999, 8, 22);
        public static readonly DateTime BEIDOU_START_TIME = new(2006, 1, 1);

        public static DateTime GetStartTime(GNSSSystem sys)
        {
            return sys switch
            {
                GNSSSystem.GPS => GPS_START_TIME,
                GNSSSystem.GALILEO => GALILEO_START_TIME,
                GNSSSystem.BEIDOU => BEIDOU_START_TIME,
                GNSSSystem.SBAS => GPS_START_TIME,
                GNSSSystem.QZSS => GPS_START_TIME,
                _ => throw new UnsupportedGNSSSystem(sys),
            };
        }
    }
}
