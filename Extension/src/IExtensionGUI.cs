using System;
using System.Runtime.InteropServices;

namespace hoTools.Extensions
{
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [Guid("7D019C3D-5E7A-4BC4-B412-0B013F7DCEF2")]
    public interface IExtensionGui
    {
        string GetName();
           
    }
  
}
