namespace RTCM3.Common.Time
{

    public static class Constant
    {
        public static readonly GNSSTime GPS_START_TIME = new() { DateTime = new DateTime(1980, 1, 6) };
        public static readonly GNSSTime GALILEO_START_TIME = new() { DateTime = new DateTime(1999, 8, 22) };
        public static readonly GNSSTime BEIDOU_START_TIME = new() { DateTime = new DateTime(2006, 1, 1) };
    }


    public struct GNSSTime : IComparable<GNSSTime>
    {
        public DateTime DateTime; // see https://learn.microsoft.com/en-us/dotnet/api/system.datetime
        public double NanoSecond; // nano second under 100

        public GNSSTime(int year, int month, int day)
        {
            DateTime = new DateTime(year, month, day);
            NanoSecond = 0;
        }

        public GNSSTime(DateTime dateTime)
        {
            DateTime = dateTime;
            NanoSecond = 0;
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
    }



    public static class LeapSecond
    {
        private static List<Tuple<DateTime, int>> data = new()
        {
            {new Tuple<DateTime,int>(new DateTime(1981,7,1) ,01)},
            {new Tuple<DateTime,int>(new DateTime(1982,7,1) ,02)},
            {new Tuple<DateTime,int>(new DateTime(1983,7,1) ,03)},
            {new Tuple<DateTime,int>(new DateTime(1985,7,1) ,04)},
            {new Tuple<DateTime,int>(new DateTime(1988,1,1) ,05)},
            {new Tuple<DateTime,int>(new DateTime(1990,1,1) ,06)},
            {new Tuple<DateTime,int>(new DateTime(1991,1,1) ,07)},
            {new Tuple<DateTime,int>(new DateTime(1992,7,1) ,08)},
            {new Tuple<DateTime,int>(new DateTime(1993,7,1) ,09)},
            {new Tuple<DateTime,int>(new DateTime(1994,7,1) ,10)},
            {new Tuple<DateTime,int>(new DateTime(1996,1,1) ,11)},
            {new Tuple<DateTime,int>(new DateTime(1997,7,1) ,12)},
            {new Tuple<DateTime,int>(new DateTime(1999,1,1) ,13)},
            {new Tuple<DateTime,int>(new DateTime(2006,1,1) ,14)},
            {new Tuple<DateTime,int>(new DateTime(2009,1,1) ,15)},
            {new Tuple<DateTime,int>(new DateTime(2012,7,1) ,16)},
            {new Tuple<DateTime,int>(new DateTime(2015,7,1) ,17)},
            {new Tuple<DateTime,int>(new DateTime(2017,1,1) ,18)},
        };
        public static void SetLeapSecond(List<Tuple<DateTime, int>> data)
        {
            LeapSecond.data = data;
        }
        public static int GetLeapSecond(this DateTime dateTime)
        {
            DateTime first = data[0].Item1;
            int result = 0;
            if (dateTime < first)
            {
                return result;
            }
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
            return new GNSSTime(dateTime.AddSeconds(dateTime.GetLeapSecond()));
        }
    }
}
