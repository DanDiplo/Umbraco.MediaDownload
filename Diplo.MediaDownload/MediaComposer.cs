using Umbraco.Core;
using Umbraco.Core.Composing;

namespace Diplo.MediaDownload
{
    /// <summary>
    /// Used to register the custom services services with DI
    /// </summary>
    /// <remarks>
    /// See https://our.umbraco.com/Documentation/Reference/using-ioc
    /// </remarks>
    public class LogComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Register<IMediaDownloadConfig, MediaDownloadConfig>(Lifetime.Singleton);
            composition.Register<IZipService, ZipService>();
        }
    }
}
