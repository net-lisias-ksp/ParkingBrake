using System.Reflection;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Parking Brake")]
[assembly: AssemblyDescription("Stop the sliding of a vessel")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(ParkingBrake.LegalMamboJambo.Company)]
[assembly: AssemblyProduct(ParkingBrake.LegalMamboJambo.Product)]
[assembly: AssemblyCopyright(ParkingBrake.LegalMamboJambo.Copyight)]
[assembly: AssemblyTrademark(ParkingBrake.LegalMamboJambo.Trademark)]
[assembly: AssemblyCulture("")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion(ParkingBrake.Version.Number)]
[assembly: AssemblyFileVersion(ParkingBrake.Version.Number)]
[assembly: KSPAssembly("ParkingBrake", ParkingBrake.Version.major, ParkingBrake.Version.minor)]

[assembly: KSPAssemblyDependency("KSPe", 2, 2)]
[assembly: KSPAssemblyDependency("KSPe.UI", 2, 2)]
