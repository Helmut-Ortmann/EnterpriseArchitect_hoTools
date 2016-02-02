using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace hoTools.Find
{
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [Guid("65FC3076-CD16-4114-B752-FEED0918858A")]
    public interface IFindAndReplaceGUI
    {
        string getName();
           
    }
  
}
