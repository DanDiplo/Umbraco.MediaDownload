using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;
using System.Web.Hosting;
using Umbraco.Core;
using Umbraco.Core.IO;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Diplo.MediaDownload
{
    /// <summary>
    /// Service used to create zip files from Umbraco media
    /// </summary>
    public class ZipService : IZipService
    {
        private readonly IMediaService mediaService;
        private readonly ILogger logger;
        private readonly IMediaDownloadConfig config;
        private readonly byte[] buffer;
        private readonly IMediaFileSystem _media;

        public ZipService(IMediaService mediaService, ILogger logger, IMediaDownloadConfig config, IMediaFileSystem media)
        {
            this.mediaService = mediaService ?? throw new ArgumentNullException(nameof(mediaService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            this._media = media ?? throw new ArgumentNullException(nameof(media)); ;
            this.buffer = new byte[config.BufferSizeBytes];
        }

        /// <summary>
        /// Creates a zip output stream that is copied to the memory stream
        /// </summary>
        /// <param name="media">The media. If this is null then assumes all media at root.</param>
        /// <param name="memoryStream">The memory stream to create the zip stream in</param>
        /// <param name="nested">Whether to include nested folders</param>
        /// <remarks>See https://github.com/icsharpcode/SharpZipLib </remarks>
        public void CreateZipStream(IMedia media, MemoryStream memoryStream, bool nested)
        {
            using (ZipOutputStream zipStream = new ZipOutputStream(memoryStream))
            {
                string folderPath = string.Empty;
                zipStream.SetLevel(config.CompressionLevel);

                if (media == null)
                {
                    foreach (var item in this.mediaService.GetRootMedia())
                    {
                        AddMedia(zipStream, item, folderPath, nested);
                    }
                }
                else
                {
                    AddMedia(zipStream, media, folderPath, nested);
                }

                StreamUtils.Copy(memoryStream, zipStream, this.buffer);
                zipStream.CloseEntry();
                zipStream.IsStreamOwner = false;
            }
        }

        private void AddMedia(ZipOutputStream zipStream, IMedia media, string folderPath, bool nested)
        {
            if (media.IsFolder())
            {
                AddFolderToZip(zipStream, media, folderPath, nested);
            }
            else
            {
                AddFileToZipZip(zipStream, media, folderPath);
            }
        }

        private void AddFolderToZip(ZipOutputStream zipStream, IMedia folder, string folderPath, bool nested)
        {
            var mediaItems = this.mediaService.GetPagedChildren(folder.Id, 0, int.MaxValue, out long _);

            folderPath += ZipEntry.CleanName(folder.Name) + "/";

            foreach (var media in mediaItems)
            {
                if (media.IsFolder())
                {
                    if (nested)
                    {
                        AddFolderToZip(zipStream, media, folderPath, nested);
                    }
                }
                else
                {
                    AddFileToZipZip(zipStream, media, folderPath);
                }
            }
        }

        private void AddFileToZipZip(ZipOutputStream zipStream, IMedia file, string folderPath)
        {
            var filePath = file.GetUrl(Constants.Conventions.Media.File, logger);

            if (_media.FileExists(filePath))
            {
                ZipEntry entry = new ZipEntry(ZipEntry.CleanName(Path.GetFileName(filePath)));
                entry.DateTime = file.CreateDate;
                zipStream.PutNextEntry(entry);

                using (Stream stream = _media.OpenFile(filePath))
                {
                    int sourceBytes;

                    do
                    {
                        sourceBytes = stream.Read(this.buffer, 0, buffer.Length);
                        zipStream.Write(this.buffer, 0, sourceBytes);
                    } while (sourceBytes > 0);
                }
            }
            else
            {
                logger.Warn(this.GetType(), $"Could not map file path to media {file.Name} ({file.Id}) at path {filePath}");
            }
        }
    }
}
