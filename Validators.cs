using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace client_Ari
{
    internal static class Validators
    {
        /// <summary>
        /// IP address verification function
        /// </summary>
        /// <param name="ipstring">xxx.yyy.zzz.eee</param>
        /// <returns>True/False if ip successfully validate</returns>
        public static bool IsValidIPAddress(string ipstring)
        {
            if (String.IsNullOrWhiteSpace(ipstring))
                return false;
            string[] splitValues = ipstring.Split('.');
            if (splitValues.Length != 4)
                return false;    
            byte tempForParsing;
            return splitValues.All(r => byte.TryParse(r, out tempForParsing));
        }
        /// <summary>
        /// Port authentication function
        /// </summary>
        /// <param name="portstring"></param>
        /// <returns>True/False if port successfully validate</returns>
        public static bool IsValidPortAddress(string portstring)
        {
            Regex regex = new Regex(@"^((6553[0-5])|(655[0-2][0-9])|(65[0-4][0-9]{2})|(6[0-4][0-9]{3})|([1-5][0-9]{4})|([0-5]{0,5})|([0][0-9]{1,4})|([0-9]{1,4}))$");
            return regex.IsMatch(portstring);
        }
        /// <summary>
        /// Mail verification function
        /// </summary>
        /// <param name="emailstring"></param>
        /// <returns>True/False if email successfully validate</returns>
        public static bool IsValidEmailAddress(string emailstring)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(emailstring);
        }
    }
}
