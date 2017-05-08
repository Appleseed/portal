using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("SelfUpdater")]
[assembly: AssemblyDescription("Appleseed Portal and Content Management System : Portal Self Update")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("ANANT Corporation")]
[assembly: AssemblyProduct("SelfUpdater")]
[assembly: AssemblyCopyright("Copyright © ANANT Corporation 2010-2017")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("1bbaa317-aebe-425c-8db3-a1aee724365d")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:
[assembly: AssemblyVersion("1.5.142.485")]
[assembly: AssemblyFileVersion("1.5.142.485")]

[assembly: PreApplicationStartMethod(typeof(SelfUpdater.Initializer), "Initialize")]
