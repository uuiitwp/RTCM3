﻿namespace RTCM3.Common
{
    public static class Physics
    {
        /// <summary>
        /// Speed of light /m
        /// </summary>
        public const double CLIGHT = 299792458;
        public const double RANGE_MS = CLIGHT * 0.001;
        public const double km = 1000;
        /// <summary>
        /// A tick is equal to 100 nanosecond /s
        /// </summary>
        public const double Tick = 1e-7;
        public const double NanoSecond = 1e-9;
        public const int Minute = 60;
        public const int Hour = 60 * Minute;
        public const int Day = 24 * Hour;
        public const int Week = 7 * Day;
    }
}
