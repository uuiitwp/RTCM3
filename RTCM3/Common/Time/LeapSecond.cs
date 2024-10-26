namespace RTCM3.Common.Time
{
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
