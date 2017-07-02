using System.Reflection;

namespace hoTools.Utils.COM
{
    /// <summary>
    /// Utilities to dynamically communicate with COM via interop.dll
    /// It get's a connection to an EA instance by their process id. After that you can use th EA API.
    /// It's somehow like the VB Script snippet: Set App = GetObject(,"EA.App")
    /// 
    /// Usually communication from EA via Script is done:
    /// - EA calls your Script (VBScript, JScrip, JavaScript), usually as chosen from a Context Menu
    /// - You call C# *.exe file and pass the EA process id
    /// - C# get's the process id and connects to the correct EA model
    /// - C# does it's work
    /// - C# returns the result via standard output
    /// 
    /// Note: 
    /// - This is a skeleton to show how it might work
    /// - Easier is the off the shelf SPARX DLL 'SparxSystems.Repository.dll':
    /// 
    /// See also: 
    /// https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/HybridScripting
    /// </summary>
    public class Com
    {
        /// <summary>
        /// Set property in COM via interop.dll
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="sProperty"></param>
        /// <param name="oValue"></param>
        public static void SetProperty(object obj, string sProperty, object oValue)
        {
            object[] oParam = new object[1];
            oParam[0] = oValue;
            obj.GetType().InvokeMember(sProperty, BindingFlags.SetProperty, null, obj, oParam);
        }
        /// <summary>
        /// Get property in COM via interop.dll
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="sProperty"></param>
        /// <param name="oValue"></param>
        /// <returns></returns>
        public static object GetProperty(object obj, string sProperty, object oValue)
        {
            object[] oParam = new object[1];
            oParam[0] = oValue;
            return obj.GetType().InvokeMember
                (sProperty, BindingFlags.GetProperty, null, obj, oParam);
        }
        /// <summary>
        /// Get property in COM via interop.dll
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="sProperty"></param>
        /// <param name="oValue1"></param>
        /// <param name="oValue2"></param>
        /// <returns></returns>
        public static object GetProperty(object obj, string sProperty, object oValue1, object oValue2)
        {
            object[] oParam = new object[2];
            oParam[0] = oValue1;
            oParam[1] = oValue2;
            return obj.GetType().InvokeMember
                (sProperty, BindingFlags.GetProperty, null, obj, oParam);
        }
        /// <summary>
        /// Get property in COM via interop.dll
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="sProperty"></param>
        /// <returns></returns>
        public static object GetProperty(object obj, string sProperty)
        {
            return obj.GetType().InvokeMember
                (sProperty, BindingFlags.GetProperty, null, obj, null);
        }
        /// <summary>
        /// Invoke method in COM via interop.dll
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="sProperty"></param>
        /// <param name="oParam"></param>
        /// <returns></returns>
        public static object InvokeMethod(object obj, string sProperty, object[] oParam)
        {
            return obj.GetType().InvokeMember
                (sProperty, BindingFlags.InvokeMethod, null, obj, oParam);
        }
        /// <summary>
        /// Invoke method in COM via interop.dll
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="sProperty"></param>
        /// <param name="oValue"></param>
        /// <returns></returns>
        public static object InvokeMethod(object obj, string sProperty, object oValue)
        {
            object[] oParam = new object[1];
            oParam[0] = oValue;
            return obj.GetType().InvokeMember
                (sProperty, BindingFlags.InvokeMethod, null, obj, oParam);
        }
    }
}
