using Grasshopper2.Components;
using Grasshopper2.Data;
using Grasshopper2.UI;
using GrasshopperIO;
using Rhino.Geometry;

namespace $PluginName$
{
    /// <summary>
    /// $ComponentDescription$
    /// </summary>
    [IoId("$ComponentGuid$")]
    public sealed class $PluginName$Component : Component
    {
        /// <summary>
        /// Component metadata: Name, Description, Plugin Tab, Category.
        /// </summary>
        public $PluginName$Component()
        : base(new Nomen("$ComponentName$", "$ComponentDescription$", "$PluginName$", "$Subcategory$")) { }

        /// <summary>
        /// Required deserialization constructor.
        /// </summary>
        public $PluginName$Component(IReader reader) : base(reader) { }

        /// <summary>
        /// Define inputs to the component.
        /// </summary>
        protected override void AddInputs(InputAdder inputs)
        {
            // Example inputs - modify as needed
            inputs.AddGeneric("Input Object", "Obj", "A generic input that accepts anything.");
            inputs.AddNumber("Factor", "F", "A numeric input.").Set(1.0);
        }

        /// <summary>
        /// Define outputs from the component.
        /// </summary>
        protected override void AddOutputs(OutputAdder outputs)
        {
            // Example output - modify as needed
            outputs.AddGeneric("Result", "R", "The result.", Access.Item);
        }

        /// <summary>
        /// Component logic. This is where the work happens.
        /// </summary>
        protected override void Process(IDataAccess access)
        {
            // Get input data
            access.GetItem(0, out object obj);
            access.GetItem(1, out double factor);

            // Process - replace with your logic
            string result = $"Received: {obj}, Factor: {factor}";

            // Set output data
            access.SetItem(0, result);
        }
    }
}
