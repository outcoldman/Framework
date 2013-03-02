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

    /// <summary>
    /// Extensions methods for <see cref="HttpContent"/>.
    /// </summary>
    public static class HttpContentExtensions
    {
        /// <summary>
        /// Read content as a disctionary of {key}={value}.
        /// </summary>
        /// <param name="this">
        /// The http content.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="@this"/> is null.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// If <paramref name="@this"/> is not "text/plain" content.
        /// </exception>
        public static async Task<Dictionary<string, string>> ReadAsDictionaryAsync(this HttpContent @this)
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this");
            }

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

        /// <summary>
        /// Read content as a Json object <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">
        /// The type which will be used to parse Json string.
        /// </typeparam>
        /// <param name="this">
        /// The http content.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="@this"/> is null.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// If <paramref name="@this"/> is not "text/plain" or "application/json" content.
        /// </exception>
        public static async Task<TResult> ReadAsJsonObject<TResult>(this HttpContent @this)
        {
            if (!@this.IsPlainText() && !@this.IsJson())
            {
                throw new NotSupportedException("ReadAsJsonObject supports only text/plain or application/json content.");
            }

            return JsonConvert.DeserializeObject<TResult>(await @this.ReadAsStringAsync());
        }

        /// <summary>
        /// Verify if <paramref name="@this"/> is a "text/plain".
        /// </summary>
        /// <param name="this">
        /// The http content.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="@this"/> is null.
        /// </exception>
        public static bool IsPlainText(this HttpContent @this)
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this");
            }

            return IsContentType(@this, "text/plain");
        }

        /// <summary>
        /// Verify if <paramref name="@this"/> is a "text/html".
        /// </summary>
        /// <param name="this">
        /// The http content.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="@this"/> is null.
        /// </exception>
        public static bool IsHtmlText(this HttpContent @this)
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this");
            }

            return IsContentType(@this, "text/html");
        }

        /// <summary>
        /// Verify if <paramref name="@this"/> is a "application/json".
        /// </summary>
        /// <param name="this">
        /// The http content.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="@this"/> is null.
        /// </exception>
        public static bool IsJson(this HttpContent @this)
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this");
            }

            return IsContentType(@this, "application/json");
        }

        /// <summary>
        /// Verify if <paramref name="@this"/> is a "application/x-www-form-urlencoded".
        /// </summary>
        /// <param name="this">
        /// The http content.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="@this"/> is null.
        /// </exception>
        public static bool IsFormUrlEncoded(this HttpContent @this)
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this");
            }

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