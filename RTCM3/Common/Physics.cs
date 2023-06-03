namespace RTCM3.Common
{
    public static class Physics
    {
        public const double CLIGHT = 299792458.0;
        public const double RANGE_MS = CLIGHT * 0.001;
        public const double km = 1000;
        public const double SecondToTick = 1e7; // A tick is equal to 100 nanosecond
        public const double TickToSecond = 1e-7;
    }
}
