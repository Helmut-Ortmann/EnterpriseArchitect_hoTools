using System.Runtime.InteropServices;

namespace HybridScriptingDll
{
    /// <summary>
    /// The Interface for all registered properties and methods available in the calling script environment
    /// </summary>
    [ComVisible(true)]
    [Guid("7E476E92-F3DC-4827-B528-F46ACB58E254")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IHybridScriptingDll
    {
        void PrintPackage(EA.Package package);
        bool PrintModel();
        /// <summary>
        /// ProcessId to connect to repository
        /// </summary>
        string ProcessId { get; set; }
        EA.Repository Repository { get;  }
    }
}


