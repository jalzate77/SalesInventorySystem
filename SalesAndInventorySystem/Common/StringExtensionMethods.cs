using System;
using System.Collections.Generic;
using System.Text;

namespace SalesAndInventorySystem.Common
{
    public static class StringExtensionMethods
    {
        public static string AppendCommaDelimited(this string source, string toAppend)
        {
            return source + (!string.IsNullOrEmpty(source) ? ", " : "") + toAppend;
        }

        public static string AppendDelimited(this string source, string toAppend, string delimiter, bool addSpaceBeforeDelimiter = false, bool addSpaceAfterDelimiter = false)
        {

            return source + (!string.IsNullOrEmpty(source) ?  (addSpaceBeforeDelimiter ? " " : "") + delimiter + (addSpaceAfterDelimiter ? " " : "") : "") + toAppend;
        }
    }
}
