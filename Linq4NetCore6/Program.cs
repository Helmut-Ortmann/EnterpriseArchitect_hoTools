// See https://aka.ms/new-console-template for more information


//--------------------------------------------------------------------------------------
// Only to demonstrate linq2db functionality
// - The linq2db environment allows
//   - .NET Core with T4 to generate the Data-Access classes (obsolete)
//   - CLI to generate the Data-Access classes I(new strategy, see T4 remarks below)
// - Access needs to be a X86 (32 bit) application, limitation of OLEDB Provider
//--------------------------------------------------------------------------------------
// Readme Nuget Package T4:  This nuget is obsoleted.
// Readme Nuget Package T4:  
// Readme Nuget Package T4:      If you use dabatabase scaffolding, consider migration to new scaffolding tool https://www.nuget.org/packages/linq2db.cli
// Readme Nuget Package T4:  Discussion thread: https://github.com/linq2db/linq2db/discussions/3531
// Readme Nuget Package T4:  
// Readme Nuget Package T4:  If you don 't use scaffolding, check migration notes here https://github.com/linq2db/linq2db/blob/master/NuGet/README.T4.md


using System.Data;
using DataModels;
using LinqToDB.Configuration;
using LinqToDB.DataProvider;
using LinqToSql.LinqUtils;
using LinqToSql.LinqUtils.Extensions;


Console.WriteLine("Hello, World!");
       
// Replace by your connection string
// - To access a repository: Make sure to have the providers loaded.


string eaConnectionString = $@"c:\Users\{Environment.UserName}\AppData\Roaming\Sparx Systems\EA\EAExample.eap";// Example database

// Access to Repository
EA.Repository  rep = new EA.Repository();


// ReSharper disable once UnusedVariable
var ret = rep.OpenFile(eaConnectionString);

string connectionString = LinqUtil.GetConnectionString(rep, out IDataProvider? provider, out string providerName);

var conBuilder = new LinqToDBConnectionOptionsBuilder();
if (provider != null)
    conBuilder.UseConnectionString(provider, connectionString);
else
    conBuilder.UseConnectionString(providerName, connectionString);

var conOption = new LinqToDBConnectionOptions(conBuilder);


using var db = new EaDataModel(conOption);

try
{
    var rootPackages = (from pkg in db.t_package
                //where pkg.Parent_ID == 0 // To show root packages/models
                orderby pkg.Name
                select new { Name = pkg.Name, Package = pkg.ea_guid }
            )
            .ToArray()
            .ToDataTable();
    foreach (var pkg in rootPackages.AsEnumerable())
    {
        Console.WriteLine(pkg.Field<string>("Name"));

    }

}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}


