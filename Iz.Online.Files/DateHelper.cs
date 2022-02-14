using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.Files
{
    public class DateHelper
    {
        public static DateTime GetTimeFromString(string dateTime)
        {
            var year = Convert.ToInt32(dateTime.Substring(0, 4));
            var month = Convert.ToInt32(dateTime.Substring(4, 2));
            var day = Convert.ToInt32(dateTime.Substring(6, 2));
            var hour = Convert.ToInt32(dateTime.Substring(8, 2));
            var minute = Convert.ToInt32(dateTime.Substring(10, 2));
            var sec = Convert.ToInt32(dateTime.Substring(12, 2));

            DateTime res = new DateTime(year, month, day, hour, minute,sec);


            return res;
        }
    }
}
