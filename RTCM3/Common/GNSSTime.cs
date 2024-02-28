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

    public struct GNSSTime : IComparable<GNSSTime>
    {
        /// <summary>
        /// see https://learn.microsoft.com/en-us/dotnet/api/system.datetime
        /// </summary>
        public DateTime DateTime;
        /// <summary>
        /// nano second under 100
        /// </summary>
        public double NanoSecond;

        /// <summary>
        /// UTC
        /// </summary>
        public static DateTime? ReferenceTime { get; set; }

        internal GNSSTime(DateTime dateTime)
        {
            DateTime = dateTime;
            NanoSecond = 0;
        }

        public readonly void GetWeekAndTow(GNSSSystem sys, out int week, out double tow)
        {
            DateTime start = StartTime.GetStartTime(sys);
            double dt = (DateTime - start).TotalSeconds;
            week = (int)(dt / Physics.Week);
            tow = dt % Physics.Week + NanoSecond * Physics.NanoSecond;
        }

        public override readonly string ToString()
        {
            return $"{DateTime:yyyy/MM/dd HH:mm:ss.fffffff} +NanoSecond={NanoSecond} LeapSecond={GetLeapSecond()}";
        }

        public static GNSSTime FromWeekAndTow(GNSSSystem sys, int week, double tow)
        {
            int intsec = (int)tow;
            double tick = (tow - intsec) / Physics.Tick;
            long longtick = (long)tick;
            DateTime start = StartTime.GetStartTime(sys);
            intsec = sys == GNSSSystem.BEIDOU ? intsec + 14 : intsec;
            DateTime dateTime = start.AddDays(week * 7).AddSeconds(intsec).AddTicks(longtick);
            return new GNSSTime() { DateTime = dateTime, NanoSecond = (tick - longtick) * 100 };
        }

        public static GNSSTime FromGNSSEpochTime(GNSSSystem sys, uint GNSSEpochTime)
        {
            DateTime rt = ReferenceTime ?? DateTime.UtcNow;
            if (sys != GNSSSystem.GLONASS)
            {
                GNSSTime gt = rt.ToGNSSTime();
                double tow = GNSSEpochTime / 1000.0;
                gt.GetWeekAndTow(sys, out int _week, out double _tow);
                if (tow < _tow - Physics.Week / 2)
                {
                    tow += Physics.Week;
                }
                else if (tow > _tow + Physics.Week / 2)
                {
                    tow -= Physics.Week;
                }
                return FromWeekAndTow(sys, _week, tow);
            }
            else
            {
                rt = rt.AddHours(3); //utc(su)
                GNSSTime gt = rt.ToGNSSTime();
                uint dow = GNSSEpochTime << 2 >> 29;
                double tod = (GNSSEpochTime << 5 >> 5) / 1000.0;
                gt.GetWeekAndTow(GNSSSystem.GPS, out int _week, out double _tow);
                double tow = dow * Physics.Day + tod;
                if (tow < _tow - Physics.Week / 2)
                {
                    tow += Physics.Week;
                }
                else if (tow > _tow + Physics.Week / 2)
                {
                    tow -= Physics.Week;
                }
                GNSSTime t = FromWeekAndTow(GNSSSystem.GPS, _week, tow);
                t -= 3 * Physics.Hour;
                t += t.GetLeapSecond();
                return t;
            }

        }

        public readonly int CompareTo(GNSSTime obj)
        {
            int result = DateTime.CompareTo(obj.DateTime);
            if (result == 0)
            {
                return NanoSecond.CompareTo(obj.NanoSecond);
            }
            return result;
        }

        public static bool operator <(GNSSTime left, GNSSTime right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(GNSSTime left, GNSSTime right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(GNSSTime left, GNSSTime right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(GNSSTime left, GNSSTime right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static GNSSTime operator +(GNSSTime left, int right)
        {
            return new GNSSTime() { DateTime = left.DateTime.AddSeconds(right), NanoSecond = left.NanoSecond };
        }
        public static GNSSTime operator -(GNSSTime left, int right)
        {
            return left + (-right);
        }

        public readonly int GetLeapSecond()
        {
            List<Tuple<DateTime, int>> leapseconds = LeapSecond.LeapSecondData;
            int result = 0;
            foreach (Tuple<DateTime, int> item in leapseconds)
            {
                if (DateTime.AddSeconds(-item.Item2) >= item.Item1)
                {
                    result = item.Item2;
                }
                else
                {
                    break;
                }
            }
            return result;
        }
    }

    public static class LeapSecond
    {
        private static readonly List<Tuple<DateTime, int>> data = new()
        {
            { new Tuple<DateTime,int>(new DateTime(1981,7,1) ,01) },
            { new Tuple<DateTime,int>(new DateTime(1982,7,1) ,02) },
            { new Tuple<DateTime,int>(new DateTime(1983,7,1) ,03) },
            { new Tuple<DateTime,int>(new DateTime(1985,7,1) ,04) },
            { new Tuple<DateTime,int>(new DateTime(1988,1,1) ,05) },
            { new Tuple<DateTime,int>(new DateTime(1990,1,1) ,06) },
            { new Tuple<DateTime,int>(new DateTime(1991,1,1) ,07) },
            { new Tuple<DateTime,int>(new DateTime(1992,7,1) ,08) },
            { new Tuple<DateTime,int>(new DateTime(1993,7,1) ,09) },
            { new Tuple<DateTime,int>(new DateTime(1994,7,1) ,10) },
            { new Tuple<DateTime,int>(new DateTime(1996,1,1) ,11) },
            { new Tuple<DateTime,int>(new DateTime(1997,7,1) ,12) },
            { new Tuple<DateTime,int>(new DateTime(1999,1,1) ,13) },
            { new Tuple<DateTime,int>(new DateTime(2006,1,1) ,14) },
            { new Tuple<DateTime,int>(new DateTime(2009,1,1) ,15) },
            { new Tuple<DateTime,int>(new DateTime(2012,7,1) ,16) },
            { new Tuple<DateTime,int>(new DateTime(2015,7,1) ,17) },
            { new Tuple<DateTime,int>(new DateTime(2017,1,1) ,18) },
        };
        public static List<Tuple<DateTime, int>> LeapSecondData => data;
        public static int GetLeapSecond(this DateTime dateTime)
        {
            int result = 0;
            foreach (Tuple<DateTime, int> item in data)
            {
                if (dateTime >= item.Item1)
                {
                    result = item.Item2;
                }
                else
                {
                    break;
                }
            }
            return result;
        }

        public static GNSSTime ToGNSSTime(this DateTime dateTime)
        {
            int ls = dateTime.GetLeapSecond();
            return new GNSSTime(dateTime.AddSeconds(ls));
        }
    }
}
