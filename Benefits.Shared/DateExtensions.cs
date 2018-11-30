using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benefits.Shared
{
    public static class DateExtensions
    {
        public static DateTime FirstDayOfMonth(this DateTime d)
        {
            return new DateTime(d.Year, d.Month, 1);
        }

        public static DateTime FirstDayOfNextMonth(this DateTime d)
        {
            return new DateTime(d.Year, d.Month, 1).AddMonths(1);
        }
    }
}