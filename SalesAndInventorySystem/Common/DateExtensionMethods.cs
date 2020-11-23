using System;
using System.Collections.Generic;
using System.Text;

namespace SalesAndInventorySystem.Common
{
    public static class DateExtensionMethods
    {
        public static bool IsInBetween(this DateTime date, DateTime from, DateTime to)
        {
            return date >= from && date <= to;
        }
    }
}
