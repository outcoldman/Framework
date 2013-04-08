// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Web
{
    using System;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Threading.Tasks;

    using OutcoldSolutions.Diagnostics;

    /// <summary>
    /// The web service base.
    /// </summary>
    public abstract class WebServiceBase
    {
        /// <summary>
        /// Gets the logger.
        /// </summary>
        protected abstract ILogger Logger { get; }

        /// <summary>
        /// Gets the http client.
        /// </summary>
        protected abstract HttpClient HttpClient { get; }

        /// <summary>
        /// Send request async.
        /// </summary>
        /// <param name="requestMessage">
        /// The request message.
        /// </param>
        /// <param name="completionOption">
        /// The completion option.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        protected async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage requestMessage,
            HttpCompletionOption completionOption = HttpCompletionOption.ResponseHeadersRead)
        {
            Debug.Assert(this.Logger != null, "this.Logger != null");
            Debug.Assert(this.HttpClient != null, "this.Logger != null");

            try
            {
                if (this.Logger.IsDebugEnabled)
                {
                    this.Logger.Debug("Send request ({0}): {1}.", requestMessage.Method, requestMessage.RequestUri);
                }

                HttpResponseMessage responseMessage = await this.HttpClient.SendAsync(requestMessage, completionOption);

                if (this.Logger.IsDebugEnabled)
                {
                    await this.Logger.LogResponseAsync(requestMessage.RequestUri.ToString(), responseMessage);
                }

                return responseMessage;
            }
            catch (Exception exception)
            {
                this.Logger.Error(exception, "Exception while sending request, Method: {0}, Uri: {1}.", requestMessage.Method, requestMessage.RequestUri);
                throw;
            }
        }
    }
}