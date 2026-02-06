using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using DB = Autodesk.Revit.DB;

namespace $PluginName$
{
    /// <summary>
    /// $ComponentDescription$
    /// </summary>
    public class $PluginName$Component : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="$PluginName$Component"/> class.
        /// </summary>
        public $PluginName$Component()
            : base(
                "$ComponentName$",           // Component name
                "$ComponentNickname$",       // Nickname
                "$ComponentDescription$",    // Description
                "$Category$",                // Category
                "$Subcategory$")             // Subcategory
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        /// <param name="pManager">The input parameter manager.</param>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            // Example inputs - modify as needed
            pManager.AddBooleanParameter("Run", "R", "Set to true to query Revit", GH_ParamAccess.item, false);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        /// <param name="pManager">The output parameter manager.</param>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            // Example outputs - modify as needed
            pManager.AddTextParameter("Document Title", "T", "Active Revit document title", GH_ParamAccess.item);
            pManager.AddTextParameter("Element Names", "N", "Names of collected elements", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The data access object.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool run = false;
            if (!DA.GetData(0, ref run)) return;
            if (!run) return;

            // Access the active Revit document through Rhino.Inside.Revit
            DB.Document? doc = RhinoInside.Revit.Revit.ActiveDBDocument;
            if (doc == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "No active Revit document");
                return;
            }

            // Output the document title
            DA.SetData(0, doc.Title);

            // Example: Collect wall names from the active document
            using (var collector = new DB.FilteredElementCollector(doc))
            {
                var names = new List<string>();
                foreach (var element in collector.OfClass(typeof(DB.Wall)))
                {
                    names.Add(element.Name);
                }
                DA.SetDataList(1, names);
            }
        }

        /// <summary>
        /// Gets the unique ID for this component.
        /// </summary>
        public override Guid ComponentGuid => new Guid("$ComponentGuid$");

        /// <summary>
        /// Gets the component icon (24x24 pixels).
        /// </summary>
        protected override System.Drawing.Bitmap? Icon => null;
        // To provide a custom icon, embed a 24x24 bitmap in Resources and return:
        // => Properties.Resources.YourIconName;

        /// <summary>
        /// Gets the exposure level in the component tab.
        /// </summary>
        public override GH_Exposure Exposure => GH_Exposure.primary;
    }
}
