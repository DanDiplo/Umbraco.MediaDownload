using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Trees;

namespace Diplo.MediaDownload
{
    /// <summary>
    /// Extends the media tree by adding the download link to the Actions menu of the Media tree
    /// </summary>
    public class MediaTreeComponent : IComponent
    {
        public void Initialize()
        {
            TreeControllerBase.MenuRendering += TreeControllerBase_MenuRendering;
        }

        public void Terminate()
        {
            TreeControllerBase.MenuRendering -= TreeControllerBase_MenuRendering;
        }

        /// <summary>
        /// Called when the tree controllers are rendering
        /// </summary>
        /// <param name="sender">The base tree controller</param>
        /// <param name="e">The tree rendering event</param>
        private void TreeControllerBase_MenuRendering(TreeControllerBase sender, MenuRenderingEventArgs e)
        {
            if (sender.TreeAlias == Constants.Trees.Media)
            {
                var menuItem = new MenuItem("diploMediaDownload", "Download");
                menuItem.LaunchDialogView("/App_Plugins/DiploMedia/Download.html", "Download Zip");
                menuItem.Icon = "download-alt";

                e.Menu.Items.Add(menuItem);
            }
        }
    }

    /// <summary>
    /// Adds our <see cref="MediaTreeComponent"/> to the Umbraco components so it's discovered
    /// </summary>
    public class MediaTreeComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Components().Append<MediaTreeComponent>();
        }
    }
}
