using System;
using Grasshopper.Kernel;
using Rhino.Geometry;

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
            pManager.AddGeometryParameter("Geometry", "G", "Input geometry", GH_ParamAccess.item);
            pManager.AddNumberParameter("Factor", "F", "Scale factor", GH_ParamAccess.item, 1.0);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        /// <param name="pManager">The output parameter manager.</param>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            // Example output - modify as needed
            pManager.AddGeometryParameter("Result", "R", "Resulting geometry", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The data access object.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Declare variables for input data
            GeometryBase? geometry = null;
            double factor = 1.0;

            // Retrieve input data
            if (!DA.GetData(0, ref geometry)) return;
            if (!DA.GetData(1, ref factor)) return;

            // Validate input
            if (geometry == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Geometry is null");
                return;
            }

            // Process geometry
            var result = geometry.Duplicate();
            var xform = Transform.Scale(Point3d.Origin, factor);
            result.Transform(xform);

            // Set output data
            DA.SetData(0, result);
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
