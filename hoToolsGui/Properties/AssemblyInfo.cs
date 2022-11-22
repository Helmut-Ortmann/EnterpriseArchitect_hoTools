using System.Resources;
using System.Reflection;
using System.Runtime.InteropServices;
using hoTools;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("hoToolsGui")]
[assembly: AssemblyDescription("hoToolsGui of hoTools")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Helmut Ortmann")]
[assembly: AssemblyProduct("hoTools")]
[assembly: AssemblyCopyright("Copyright ©  Helmut Ortmann 2017")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(true)]

// The following Id is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("95E55ADF-0152-4D32-9414-94ACFA19C593")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
//[assembly: AssemblyVersion("1.2.*")]// if you use this default the registration of COM+ changes

// If you change something you have to update the component information in Files.wxs
// If not: you get registration error and the Addin isn't loaded in EA
// Assembly version is used to output release information for an assembly
[assembly: AssemblyVersion("4.2")] 
// possibly make same as ProductVersion in Product.wxs
[assembly: AssemblyFileVersion("4.2")]
[assembly: NeutralResourcesLanguage("en")]

