using System;


namespace websocket
{

    public static class UnixTime
    {
        static DateTime unixEpoch;
        static UnixTime()
        {
            unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        }

        public static UInt32 Now { get { return GetFromDateTime(DateTime.UtcNow); } }
        public static UInt32 GetFromDateTime(DateTime d) { return (UInt32)(d - unixEpoch).TotalSeconds; }
        public static DateTime ConvertToDateTime(double unixtime) { return unixEpoch.AddSeconds(unixtime); }
        //public static DateTime UnixTimestampToDateTime(long unixTime)
        //{
        //    DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        //    long unixTimeStampInTicks = (long)(unixTime * TimeSpan.TicksPerSecond);
        //    var dt = new DateTime(unixStart.Ticks + unixTimeStampInTicks);
        //    var dtt = new DateTime(unixTime, DateTimeKind.Utc);
        //    return dt;
        //}

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

    }
}

