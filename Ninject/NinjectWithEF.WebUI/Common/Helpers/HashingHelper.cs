using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace NinjectWithEF.WebUI.Common.Helpers
{
    public class HashingHelper
    {
        public static readonly string _hashQuerySeparator = "&h=";

        public static readonly string _hashKey = "C8DE2ABD";

        public static string CreateTamperProofQueryString(string basicQueryString)
        {
            //return string.Concat(basicQueryString, _hashQuerySeparator, ComputeHash(basicQueryString));
            return ComputeHash(basicQueryString);
        }

        /// <summary>
        /// This method uses the querysting value and the _hasKey to compute the Hash using HMACSHA1 Algorithm
        /// </summary>
        /// <param name="basicQueryString"></param>
        /// <returns></returns>
        public static string ComputeHash(string basicQueryString)
        {
            // add some randomness to the hashing using either the client ip or user agent or the session information 
            // to differentiate one request from another
            // this example uses SessionID which is unique for each session
            HttpSessionState httpSession = HttpContext.Current.Session;
            basicQueryString += httpSession.SessionID;
            httpSession["HashIndex"] = 7692;

            byte[] textBytes = Encoding.UTF8.GetBytes(basicQueryString);

            HMACSHA1 hashAlgorithm = new HMACSHA1(Conversions.HexToByteArray(_hashKey));

            byte[] hash = hashAlgorithm.ComputeHash(textBytes);

            return Conversions.ByteArrayToHex(hash);
        }

        public static void ValidateQueryString()
        {
            HttpRequest request = HttpContext.Current.Request;

            if (request.QueryString.Count == 0)
            {
                return;
            }

            string queryString = request.Url.Query.TrimStart(new char[] { '?' });

            string submittedHash = request.QueryString["h"];

            if (submittedHash == null)
            {
                throw new ApplicationException("Querystring validation hash missing!");
            }

            int hashPos = queryString.IndexOf(_hashQuerySeparator);

            queryString = queryString.Substring(0, hashPos);

            if (submittedHash != ComputeHash(queryString))
            {
                throw new ApplicationException("Querystring hash value mismatch");
            }
        }

        
    }
}