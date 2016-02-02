using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace hoTools.EaServices
{
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [Guid("607875FF-1217-41AC-A91A-C6080D51C227")]
    public interface IAddinControl
    {
        string getName();
    }
}
