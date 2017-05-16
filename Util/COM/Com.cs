using System.Reflection;

namespace hoTools.Utils.COM
{
    /// <summary>
    /// Utilities to dynamically communicate with COM via interop.dll
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
