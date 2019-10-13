using System;
using System.Globalization;

namespace bayoen.library.General.ExtendedMethods
{
    public static partial class ExtendedMethods
    {
        public static string ToCompactString(this DateTime dt)
        {
            if (dt.Date == DateTime.Today)
            {
                return dt.ToString("tt hh:mm:ss", CultureInfo.CreateSpecificCulture("en-US"));
            }
            else
            {
                return dt.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en-US"));
            }
        }
    }
}
