using System.Diagnostics;
using System.Windows.Forms;
using EA;
using hoTools.Utils.BulkChange;


// ReSharper disable once CheckNamespace
namespace hoTools.EaServices
{
    public static partial class EaService
    {

        /// <summary>
        /// Bulk change Element in Package recursive 1 according to Settings.Json, first entry 
        /// </summary>
        [ServiceOperation("{32B75793-B369-4443-B497-61E50BAB359B}", "Bulk change Elements to 1",
            "Select Browser Package (see Settings.Json: 'BulkItems: 1. entry)", isTextRequired: false)]
        public static void BulkElementChangeRecursive1(EA.Repository rep)
        {
            BulkElementChangeRecursiveWrapper(rep, 0);
        }
        /// <summary>
        /// Bulk change Element in Package recursive 1 according to Settings.Json, first entry 
        /// </summary>
        [ServiceOperation("{E30A7EC0-C443-453F-A31E-A62766EAA082}", "Bulk change Elements to 2",
            "Select Browser Package (see Settings.Json: 'BulkItems: 2. entry)", isTextRequired: false)]
        public static void BulkElementChangeRecursive2(EA.Repository rep)
        {
            BulkElementChangeRecursiveWrapper(rep, 1);
        }
        /// <summary>
        /// Bulk change Element in Package recursive 1 according to Settings.Json, first entry 
        /// </summary>
        [ServiceOperation("{6483D0C7-6B47-4844-8E7E-5E1A8CA02463}", "Bulk change Elements to 3",
            "Select Browser Package (see Settings.Json: 'BulkItems: 3. entry)", isTextRequired: false)]
        public static void BulkElementChangeRecursive3(EA.Repository rep)
        {
            BulkElementChangeRecursiveWrapper(rep, 2);
        }
        /// <summary>
        /// Bulk change Element in Package recursive 1 according to Settings.Json, first entry 
        /// </summary>
        [ServiceOperation("{1B693D4E-43DD-4041-B3F4-9A18B3748FD0}", "Bulk change Elements to 4",
            "Select Browser Package (see Settings.Json: 'BulkItems: 4. entry)", isTextRequired: false)]
        public static void BulkElementChangeRecursive4(EA.Repository rep)
        {
            BulkElementChangeRecursiveWrapper(rep, 3);
        }
        /// <summary>
        /// Bulk change Element in Package recursive 1 according to Settings.Json, first entry 
        /// </summary>
        [ServiceOperation("{E9CF4F77-8204-48CC-8CD9-A42B99935D51}", "Bulk change Elements to 5",
            "Select Browser Package (see Settings.Json: 'BulkItems: 5. entry)", isTextRequired: false)]
        public static void BulkElementChangeRecursive5(EA.Repository rep)
        {
            BulkElementChangeRecursiveWrapper(rep, 4);
        }



        /// <summary>
        /// Bulk change Elements 1 according to Settings.Json, first entry 
        /// </summary>
        [ServiceOperation("{9BC37C84-6BC7-4FD8-83B7-B87E176F3302}", "Bulk change Elements to 1",
            "Select Diagram Elements, Browser Package, Browser Elements (see Settings.Json: 'BulkItems: 1. entry)", isTextRequired: false)]
        public static void BulkElementChange1(EA.Repository rep)
        {
            BulkElementChangeWrapper(rep, 0);
        }
        /// <summary>
        /// Bulk change Elements 2 according to Settings.Json, second entry 
        /// </summary>
        [ServiceOperation("{87F4044A-6401-4BF4-8DC5-9739064775A6}", "Bulk change Elements to 1",
            "Select Diagram Elements, Browser Package, Browser Elements (see Settings.Json: 'BulkItems: 1. entry)", isTextRequired: false)]
        public static void BulkElementChange2(EA.Repository rep)
        {
            BulkElementChangeWrapper(rep, 1);
        }
        /// <summary>
        /// Bulk change Elements 4 according to Settings.Json, third entry 
        /// </summary>
        [ServiceOperation("{5AE828EC-2DFD-4842-8EA9-7E60A64A2F45}", "Bulk change Elements to 1",
            "Select Diagram Elements, Browser Package, Browser Elements (see Settings.Json: 'BulkItems: 1. entry)", isTextRequired: false)]
        public static void BulkElementChange3(EA.Repository rep)
        {
            BulkElementChangeWrapper(rep, 2);
        }
        /// <summary>
        /// Bulk change Elements 4 according to Settings.Json, fourth entry 
        /// </summary>
        [ServiceOperation("{5406AEA8-D17F-4B69-800B-715D6AF3B00B}", "Bulk change Elements to 1",
            "Select Diagram Elements, Browser Package, Browser Elements (see Settings.Json: 'BulkItems: 1. entry)", isTextRequired: false)]
        public static void BulkElementChange4(EA.Repository rep)
        {
            BulkElementChangeWrapper(rep, 3);
        }
        /// <summary>
        /// Bulk change Elements 1 according to Settings.Json, first entry 
        /// </summary>
        [ServiceOperation("{4D0947E2-0C4D-41A8-8D85-B1D935EF532B}", "Bulk change Elements to 1",
            "Select Diagram Elements, Browser Package, Browser Elements (see Settings.Json: 'BulkItems: 1. entry)", isTextRequired: false)]
        public static void BulkElementChange5(EA.Repository rep)
        {
            BulkElementChangeWrapper(rep, 4);
        }
        /// <summary>
        /// Wrapper to change Diagram style
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="pos"></param>
        private static void BulkElementChangeWrapper(Repository rep, int pos)
        {
            Debug.Assert(DiagramStyle.BulkElementItems != null, "DiagramStyle.BulkElementItems != null");
            if (DiagramStyle.BulkElementItems == null && DiagramStyle.BulkElementItems.Count <= pos)
            {
                MessageBox.Show($"Element number(rel 1): {pos+1} missing", $"No Bulk Element style in 'Settings.json' found");
                return;
            }

            BulkElementItem bulkElement = DiagramStyle.BulkElementItems[pos];
            BulkItemChange.BulkChange(rep,  bulkElement);
        }
        /// <summary>
        /// Wrapper to change Diagram style
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="pos"></param>
        private static void BulkElementChangeRecursiveWrapper(Repository rep, int pos)
        {
            Debug.Assert(DiagramStyle.BulkElementItems != null, "DiagramStyle.BulkElementItems != null");
            if (DiagramStyle.BulkElementItems == null && DiagramStyle.BulkElementItems.Count <= pos)
            {
                MessageBox.Show($"Element number(rel 1): {pos+1} missing", $"No Bulk Element style in 'Settings.json' found");
                return;
            }

            BulkElementItem bulkElement = DiagramStyle.BulkElementItems[pos];
            BulkItemChange.BulkChangeRecursive(rep,  bulkElement);
        }
    }
 }

