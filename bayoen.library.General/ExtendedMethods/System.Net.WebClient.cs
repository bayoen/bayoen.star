using System;
using System.Globalization;
using System.Net;

namespace bayoen.library.General.ExtendedMethods
{
    public static partial class ExtendedMethods
    {
        public static long ContentLength(this WebClient web, string url)
        {
            web.OpenRead(url);
            return Convert.ToInt64(web.ResponseHeaders["Content-Length"]);
        }
    }
}
