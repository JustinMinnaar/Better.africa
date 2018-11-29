using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benefits.Shared
{
    public static class Clock
    {
        private static DateTime? _Now;

        public static DateTime Now => _Now ?? DateTime.Now;
    }
}