﻿using System;
using System.Runtime.InteropServices;

namespace hoTools.hoSqlGui
{
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [Guid("8AD92777-EF23-4105-85DF-BA99D9C61BC5")]
    public interface IHoSqlGui
    {
        string GetName();
           
    }
  
}
