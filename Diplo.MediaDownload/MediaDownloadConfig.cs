namespace Diplo.MediaDownload
{
    /// <summary>
    /// Configuration
    /// </summary>
    public class MediaDownloadConfig : IMediaDownloadConfig
    {
        /// <summary>
        /// Sets the default compression level - by default this is low to increase speed (plus most media won't compress much)
        /// </summary>
        public int CompressionLevel => 1;

        /// <summary>
        /// Sets the size of the buffer (in bytes) used when generating zip file
        /// </summary>
        public int BufferSizeBytes => 4096;
    }
}
