using System.IO;
using Umbraco.Core.Models;

namespace Diplo.MediaDownload
{
    /// <summary>
    /// Service to zip Media files
    /// </summary>
    public interface IZipService
    {
        /// <summary>
        /// Creates a zip file from the target media by generating it within a memory stream
        /// </summary>
        /// <param name="media">The root Umbraco media</param>
        /// <param name="memoryStream">The memory stream to write the zip to</param>
        /// <param name="nested">Whether to include nested folders</param>
        void CreateZipStream(IMedia media, MemoryStream memoryStream, bool nested);
    }
}