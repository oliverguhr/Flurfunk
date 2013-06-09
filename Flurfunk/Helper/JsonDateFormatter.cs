using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;
namespace Flurfunk.Helper
{
    public static class JsonDateFormatter
    {        
        private static readonly DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        public static DateTime FromJsonDateTime(this string jsDateTime)
        {
            // remove the last 2 and the first 6 chars to get the ms
            // /Date(1370718359560)/
            string ms = jsDateTime.Remove(jsDateTime.Length - 2, 2).Remove(0, 6);

            DateTime dt = unixEpoch.AddMilliseconds(Convert.ToDouble(ms));
            //todo: set time to utc
            return DateTime.SpecifyKind(dt, DateTimeKind.Utc).ToLocalTime();            
        }
    }
}