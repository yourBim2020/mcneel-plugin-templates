using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace $PluginName$
{
    /// <summary>
    /// Provides assembly information for the Grasshopper plugin.
    /// </summary>
    public class $PluginName$Info : GH_AssemblyInfo
    {
        /// <summary>
        /// Gets the plugin name displayed in Grasshopper.
        /// </summary>
        public override string Name => "$PluginName$";

        /// <summary>
        /// Gets the plugin icon (24x24 pixels) displayed in the Grasshopper ribbon.
        /// </summary>
        public override Bitmap? Icon => null;
        // To provide a custom icon, embed a 24x24 bitmap in Resources and return:
        // => Properties.Resources.YourPluginIcon;

        /// <summary>
        /// Gets the plugin description.
        /// </summary>
        public override string Description => "$PluginDescription$";

        /// <summary>
        /// Gets the unique identifier for this plugin.
        /// </summary>
        public override Guid Id => new Guid("$PluginGuid$");

        /// <summary>
        /// Gets the author name.
        /// </summary>
        public override string AuthorName => "$AuthorName$";

        /// <summary>
        /// Gets the author contact information.
        /// </summary>
        public override string AuthorContact => "$AuthorEmail$";

        /// <summary>
        /// Gets the plugin version.
        /// </summary>
        public override string Version => "$Version$";
    }
}
