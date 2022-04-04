using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace Diplo.MediaDownload
{
    [PluginController("DiploMedia")]
    public class MediaController : UmbracoAuthorizedApiController
    {
        private readonly IZipService zipService;
        private readonly IUmbracoContextFactory umbracoContextFactory;

        public MediaController(IZipService zipService, IUmbracoContextFactory umbracoContextFactory)
        {
            this.zipService = zipService ?? throw new ArgumentNullException(nameof(zipService));
            this.umbracoContextFactory = umbracoContextFactory ?? throw new ArgumentNullException(nameof(umbracoContextFactory));
        }

        /// <summary>
        /// API controller endpoint for generating a zip file from a given Umbraco media identifier
        /// </summary>
        /// <returns>An HTTP response message</returns>
        /// <remarks>
        ///  ~/Umbraco/backoffice/DiploMedia/Media/Download/12345
        /// </remarks>
        [HttpGet]
        [Umbraco.Web.WebApi.UmbracoAuthorize]
        public HttpResponseMessage Download(int id, bool nested = true)
        {
            using (var context = umbracoContextFactory.EnsureUmbracoContext())
            {
                if (!context.UmbracoContext.Security.CurrentUser.AllowedSections.Contains(Constants.Trees.Media))
                {
                    return NotAuthorisedResponse();
                }
            }

            var media = this.Services.MediaService.GetById(id);

            if (media == null && id != -1)
            {
                return MediaNotFoundResponse(id);
            }

            string fileName = ((media?.Name ?? "Media") + ".zip").ToSafeFileName();

            var response = new HttpResponseMessage(HttpStatusCode.OK);

            using (var memoryStream = new MemoryStream())
            {
                zipService.CreateZipStream(media, memoryStream, nested);
                memoryStream.Position = 0;

                response.Content = new ByteArrayContent(memoryStream.ToArray());

                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileName
                };

                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/zip");

                return response;
            }
        }

        private HttpResponseMessage MediaNotFoundResponse(int id)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent($"Could not find a media item with an ID of {id} in Umbraco.")
            };

            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");

            return response;
        }

        private HttpResponseMessage NotAuthorisedResponse()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                Content = new StringContent("You are not authorized to access the Media section in Umbraco")
            };

            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");

            return response;
        }
    }
}
