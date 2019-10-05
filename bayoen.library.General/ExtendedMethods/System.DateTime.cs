using System;
using System.Globalization;

namespace bayoen.library.General.ExtendedMethods
{
    public static partial class ExtendedMethods
    {
        public static string ToSimpleString(this DateTime datetime)
        {
            if (datetime.Date == DateTime.Today)
            {
                return datetime.ToString("tt hh:mm:ss", CultureInfo.CreateSpecificCulture("en-US"));
            }
            else
            {
                return datetime.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en-US"));
            }
        }
    }
}
