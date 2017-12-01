using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BRPLUSA.Core.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Compares two strings and returns true if the 
        /// FIRST string is before the SECOND string alphabetically
        /// </summary>
        /// <param name="sa">base string</param>
        /// <param name="sb">string to compare</param>
        /// <returns></returns>
        public static bool IsLessThan(this string sa, string sb)
        {
            var lessThan = string.CompareOrdinal(sa.ToLower(), sb.ToLower()) < 0;

            return lessThan;
        }

        public static bool IsMoreThan(this string sa, string sb)
        {
            return !IsLessThan(sa, sb);
        }
    }
}
