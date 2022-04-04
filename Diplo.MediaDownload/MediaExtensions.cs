using Umbraco.Core;
using Umbraco.Core.Models;

namespace Diplo.MediaDownload
{
    public static class MediaExtensions
    {
        /// <summary>
        /// Determins if a media item is a folder (or not)
        /// </summary>
        /// <param name="media">The media</param>
        /// <returns>True if it's a folder; otherwise false</returns>
        public static bool IsFolder(this IMedia media)
        {
            return media.ContentType.Alias.InvariantEquals("folder");
        }
    }
}
