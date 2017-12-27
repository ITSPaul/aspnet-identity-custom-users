using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetIdentity.WebApi
{
    public static class Extensions
    {
        public static int ToEpoch(this DateTime? date)
        {
            if (date == null) return int.MinValue;
            DateTime epoch = new DateTime(1970, 1, 1);
            TimeSpan epochTimeSpan = date.Value - epoch;
            return (int)epochTimeSpan.TotalSeconds;
        }

        public static DateTime FromEpocToDateTime(int epocSeconds)
        {
            return new DateTime(1970, 1, 1).AddSeconds(epocSeconds);
        }
    }
}
