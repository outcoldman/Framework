// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using OutcoldSolutions.Web;

    using Windows.Security.Cryptography;
    using Windows.Security.Cryptography.Core;
    using Windows.Storage.Streams;

    /// <summary>
    /// The logger web extensions.
    /// </summary>
    public static class LoggerWebExtensions
    {
        /// <summary>
        /// Log request.
        /// </summary>
        /// <param name="this">
        /// The this.
        /// </param>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="requestUrl">
        /// The request url.
        /// </param>
        /// <param name="cookieCollection">
        /// The cookie collection.
        /// </param>
        /// <param name="formData">
        /// The form data.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="@this"/> is null.
        /// </exception>
        public static void LogRequest(
            this ILogger @this,
            HttpMethod method,
            string requestUrl,
            IEnumerable<Cookie> cookieCollection = null,
            IDictionary<string, string> formData = null)
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this");
            }

            if (@this.IsDebugEnabled)
            {
                var log = new StringBuilder();

                log.AppendLine();
                log.AppendFormat("{0} REQUEST: {1}.", method, requestUrl);
                log.AppendLine();

                if (formData != null)
                {
                    log.AppendLine("    FORMDATA: ");

                    foreach (var argument in formData)
                    {
                        log.AppendFormat("        {0}={1}", argument.Key, argument.Value);
                        log.AppendLine();
                    }
                }

                LogCookies(log, cookieCollection);

                log.AppendLine();

                @this.Debug(log.ToString());
            }
        }

        /// <summary>
        /// The log cookies.
        /// </summary>
        /// <param name="this">
        /// The this.
        /// </param>
        /// <param name="cookieCollection">
        /// The cookie collection.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="@this"/> is null.
        /// </exception>
        public static void LogCookies(this ILogger @this, IEnumerable<Cookie> cookieCollection)
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this");
            }

            if (@this.IsDebugEnabled)
            {
                if (cookieCollection != null)
                {
                    var log = new StringBuilder();
                    LogCookies(log, cookieCollection);

                    @this.Debug(log.ToString());
                }
            }
        }

        /// <summary>
        /// The log response async.
        /// </summary>
        /// <param name="this">
        /// The this.
        /// </param>
        /// <param name="requestUrl">
        /// The request url.
        /// </param>
        /// <param name="responseMessage">
        /// The response message.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="@this"/> or <paramref name="responseMessage"/> are null.
        /// </exception>
        public static async Task LogResponseAsync(
            this ILogger @this,
            string requestUrl,
            HttpResponseMessage responseMessage)
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this");
            }

            if (responseMessage == null)
            {
                throw new ArgumentNullException("responseMessage");
            }

            if (@this.IsDebugEnabled)
            {
                var log = new StringBuilder();

                log.AppendLine();
                log.AppendFormat("RESPONSE FROM '{0}' COMPLETED, STATUS CODE: {1}.", responseMessage.RequestMessage.RequestUri, responseMessage.StatusCode);
                log.AppendLine();
                if (!string.IsNullOrEmpty(responseMessage.ReasonPhrase))
                {
                    log.AppendFormat("  REASON PHRASE '{0}'", responseMessage.ReasonPhrase);
                    log.AppendLine();
                }

                log.AppendFormat("  ORIGINAL URI: {0}.", requestUrl);
                log.AppendLine();

                log.AppendLine("    RESPONSE HEADERS: ");
                LogHeaders(log, responseMessage.Headers);

                await LogContentAsync(log, responseMessage.Content);

                log.AppendLine();

                @this.Debug(log.ToString());
            }
        }

        private static async Task LogContentAsync(StringBuilder log, HttpContent httpContent)
        {
            if (httpContent != null)
            {
                log.AppendLine("    CONTENT HEADERS: ");

                LogHeaders(log, httpContent.Headers);

                if (httpContent.IsPlainText()
                    || httpContent.IsHtmlText()
                    || httpContent.IsJson()
                    || httpContent.IsFormUrlEncoded())
                {
                    var content = await httpContent.ReadAsStringAsync();

                    log.AppendFormat("    CONTENT:{0}{1}", Environment.NewLine, content.Substring(0, Math.Min(4096, content.Length)));
                    log.AppendLine();
                    log.AppendFormat("    ENDCONTENT.");
                    log.AppendLine();
                }
            }
            else
            {
                log.AppendLine("    CONTENT IS NULL.");
            }
        }

        private static void LogHeaders(StringBuilder log, HttpHeaders headers)
        {
            if (headers != null)
            {
                foreach (var httpResponseHeader in headers)
                {
                    log.AppendFormat("        {0}={1}", httpResponseHeader.Key, string.Join("&&&", httpResponseHeader.Value));
                    log.AppendLine();
                }
            }
        }

        private static void LogCookies(StringBuilder log, IEnumerable<Cookie> cookieCollection)
        {
            if (cookieCollection != null)
            {
                var cookies = cookieCollection.ToList();

                StringBuilder readlCookiesOutput = null;
                IDebugConsole debugConsole = null;
                if (ApplicationBase.Container != null && ApplicationBase.Container.IsRegistered<IDebugConsole>())
                {
                    debugConsole = ApplicationBase.Container.Resolve<IDebugConsole>();

                    readlCookiesOutput = new StringBuilder();
                    readlCookiesOutput.AppendFormat("    REAL COOKIES({0}):", cookies.Count);
                    readlCookiesOutput.AppendLine();
                }
                
                log.AppendFormat("    COOKIES({0}):", cookies.Count);
                log.AppendLine();

                if (readlCookiesOutput != null)
                {
                    readlCookiesOutput.Append(log);
                }

                foreach (Cookie cookieLog in cookies)
                {
                    if (readlCookiesOutput != null)
                    {
                        readlCookiesOutput.AppendFormat("        {0}={1}, Expires={2}", cookieLog.Name, cookieLog.Value, cookieLog.Expires);
                        readlCookiesOutput.AppendLine();
                    }

                    HashAlgorithmProvider hashProvider = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
                    IBuffer hash =
                        hashProvider.HashData(
                            CryptographicBuffer.ConvertStringToBinary(cookieLog.Value, BinaryStringEncoding.Utf8));
                    string hashValue = CryptographicBuffer.EncodeToBase64String(hash);

                    log.AppendFormat("        {0}={{MD5_VALUE_HASH}}{1}, Expires={2}", cookieLog.Name, hashValue, cookieLog.Expires);
                    log.AppendLine();
                }

                if (debugConsole != null)
                {
                    debugConsole.WriteLine(readlCookiesOutput.ToString());
                }
            }
        }
    }
}