using System;
using Rhino;

namespace $PluginName$
{
    /// <summary>
    /// Every RhinoCommon .rhp assembly must have one and only one PlugIn-derived
    /// class. DO NOT create instances of this class yourself. It is the
    /// responsibility of Rhino to create an instance of this class.
    /// To complete plug-in information, see also the PlugInDescription
    /// attributes in AssemblyInfo.cs.
    /// </summary>
    public class $PluginName$Plugin : Rhino.PlugIns.PlugIn
    {
        public $PluginName$Plugin()
        {
            Instance = this;
        }

        /// <summary>Gets the only instance of the $PluginName$ plug-in.</summary>
        public static $PluginName$Plugin Instance { get; private set; }

        // You can override methods here to change the plug-in behavior on
        // loading and shut down, add options pages to the Rhino _Option command
        // and maintain plug-in wide options in a document.
    }
}
