namespace Diplo.MediaDownload
{
    /// <summary>
    /// Configuration for the Media Download services
    /// </summary>
    public interface IMediaDownloadConfig
    {
        /// <summary>
        /// Gets the size of the buffer used when generating a zip file
        /// </summary>
        int BufferSizeBytes { get; }

        /// <summary>
        /// Gets the compression level used when generating a zip file (higher the more compression; lower faster)
        /// </summary>
        int CompressionLevel { get; }
    }
}