using Grasshopper2.UI;
using System;

namespace $PluginName$
{
    /// <summary>
    /// Plugin entry point for Grasshopper 2.
    /// Defines plugin metadata and author information.
    /// </summary>
    public sealed class $PluginName$Plugin : Grasshopper2.Framework.Plugin
    {
        public $PluginName$Plugin()
          : base(new Guid("$PluginGuid$"),
                 new Nomen("$PluginName$", "$PluginDescription$"),
                 Version.Parse("$Version$"))
        { }

        public override string Author
        {
            get { return "$AuthorName$"; }
        }
    }
}
