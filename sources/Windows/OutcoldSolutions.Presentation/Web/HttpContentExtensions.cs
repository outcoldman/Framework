// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Web
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    internal static class HttpContentExtensions
    {
        internal static async Task<Dictionary<string, string>> ReadAsDictionaryAsync(this HttpContent @this)
        {
            if (!@this.IsPlainText())
            {
                throw new NotSupportedException("ReadAsDictionaryAsync supports only text/plain content.");
            }

            var responseValues = new Dictionary<string, string>();

            using (var streamReader = new StringReader(await @this.ReadAsStringAsync()))
            {
                string responseLine = null;
                while ((responseLine = await streamReader.ReadLineAsync()) != null)
                {
                    var firstEqual = responseLine.IndexOf("=", StringComparison.OrdinalIgnoreCase);
                    if (firstEqual > 0)
                    {
                        string name = responseLine.Substring(0, firstEqual);
                        string value = responseLine.Substring(firstEqual + 1, responseLine.Length - (firstEqual + 1));
                        responseValues.Add(name, WebUtility.UrlDecode(value));
                    }
                    else
                    {
                        responseValues.Add(responseLine, string.Empty);
                    }
                }
            }

            return responseValues;
        }

        internal static async Task<TResult> ReadAsJsonObject<TResult>(this HttpContent @this)
        {
            if (!@this.IsPlainText() && !@this.IsJson())
            {
                throw new NotSupportedException("ReadAsJsonObject supports only text/plain or application/json content.");
            }

            return JsonConvert.DeserializeObject<TResult>(await @this.ReadAsStringAsync());
        }

        internal static bool IsPlainText(this HttpContent @this)
        {
            return IsContentType(@this, "text/plain");
        }

        internal static bool IsHtmlText(this HttpContent @this)
        {
            return IsContentType(@this, "text/html");
        }

        internal static bool IsJson(this HttpContent @this)
        {
            return IsContentType(@this, "application/json");
        }

        internal static bool IsFormUrlEncoded(this HttpContent @this)
        {
            return IsContentType(@this, "application/x-www-form-urlencoded");
        }

        private static bool IsContentType(HttpContent @this, string contentType)
        {
            if (@this == null)
            {
                return false;
            }

            if (@this.Headers.ContentType != null)
            {
                return string.Equals(@this.Headers.ContentType.MediaType, contentType, StringComparison.OrdinalIgnoreCase);
            }

            return @this.Headers.Contains("Content-Type") && @this.Headers.GetValues("Content-Type").Any(x => x.IndexOf(contentType, StringComparison.OrdinalIgnoreCase) == 0);
        }
    }
}