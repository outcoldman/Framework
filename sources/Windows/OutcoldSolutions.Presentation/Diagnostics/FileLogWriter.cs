// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Presentation.Diagnostics
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using Windows.Storage;

    /// <summary>
    /// The file log writer.
    /// </summary>
    public class FileLogWriter : ILogWriter, IDisposable
    {
        private readonly object locker = new object();

        private StreamWriter writer;

        /// <inheritdoc />
        public bool IsEnabled
        {
            get
            {
                return true;
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            lock (this.locker)
            {
                if (this.writer != null)
                {
                    this.ClearStream();
                }
            }
        }

        /// <inheritdoc />
        public void Log(DateTime dateTime, string level, string context, string message, params object[] parameters)
        {
            lock (this.locker)
            {
                if (this.writer == null)
                {
                    this.EnableLogging();
                }

                if (this.writer != null)
                {
                    if (parameters.Length == 0)
                    {
                        this.writer.WriteLine(
                            "{0:o}: {1}::: {2} --- {3}", dateTime, level, context, message);
                    }
                    else
                    {
                        this.writer.WriteLine(
                            "{0:o}: {1}::: {2} --- {3}", dateTime, level, context, string.Format(message, parameters));
                    }

                    this.writer.Flush();
                }
            }
        }

        private void EnableLogging()
        {
            var enableLoggingAsync = this.EnableLoggingAsync();
            TaskEx.WaitAllSafe(enableLoggingAsync);

            if (enableLoggingAsync.IsCompleted && !enableLoggingAsync.IsFaulted)
            {
                lock (this.locker)
                {
                    this.writer = enableLoggingAsync.Result;   
                }
            }
        }

        private void ClearStream()
        {
            lock (this.locker)
            {
                this.writer.Flush();
                this.writer.Dispose();
                this.writer = null;
            }
        }

        private async Task<StreamWriter> EnableLoggingAsync()
        {
            var storageFile =
                await ApplicationData.Current.LocalFolder.CreateFileAsync(string.Format("{0:yyyy-MM-dd-HH-mm-ss-ffff}.log", DateTime.Now), CreationCollisionOption.OpenIfExists).AsTask();

            var stream = await storageFile.OpenAsync(FileAccessMode.ReadWrite).AsTask();

            return new StreamWriter(stream.AsStream());
        }
    }
}
