<Query Kind="Program">
  <Connection>
    <ID>c1a57346-f0e4-4d03-9b76-f851da370b1b</ID>
    <Persist>true</Persist>
    <Driver Assembly="linq2db.LINQPad" PublicKeyToken="f19f8aed7feff67e">LinqToDB.LINQPad.LinqToDBDriver</Driver>
    <Provider>System.Data.OleDb</Provider>
    <Server>c:\Program Files (x86)\Sparx Systems\EA\EAExample.eap</Server>
    <DbVersion>04.00.0000</DbVersion>
    <CustomCxString>Provider=Microsoft.Jet.OLEDB.4.0;Data Source=c:\Program Files (x86)\Sparx Systems\EA\EAExample.eap;</CustomCxString>
    <DriverData>
      <providerName>Access</providerName>
      <optimizeJoins>true</optimizeJoins>
      <allowMultipleQuery>false</allowMultipleQuery>
      <commandTimeout>0</commandTimeout>
    </DriverData>
  </Connection>
</Query>

void Main(string[] args)
{
string showString = "";
#if   CMD
if (args != null && args.Length > 0)
showString = $"Batch: '{args[0]}";

#else

showString = "Dialog:";

#endif


// Total amount of Object_Types
var count = t_object.Count();

// All object_types summary:
// - Type
// - Count
// - Percentage
// - Total count of object_types
var q =
(from c in t_object.AsEnumerable()
group c by c.Object_Type into g
orderby g.Key

select new
{
Type = $"{g.Key}",
Prozent = $"{ (float)g.Count()*100/count:00.00}%",
Count = g.Count(),
Total = count
});


// Requirement summary:
// - Type
// - Count
// - Percentage
// - Total count of requirements
var countReq = t_object.Where(e => e.Object_Type == "Requirement").Count();
var q1 =
(from c in t_object.AsEnumerable()
where c.Object_Type == "Requirement"
group c by c.Stereotype into g
orderby g.Key

select new
{
Type = (g.Key == null) ? "Simple" : $"<<{g.Key}>>",
  Prozent = $"{ (float)g.Count() * 100 / countReq:00.00}%",
  Count = g.Count(),
  Total = countReq
  });
  q1.Dump($"{showString} Requirement types");
  // Total requirements
  var sum = q.Concat(q1);
  sum.Dump($"{showString}: Object types");
  }

  // Define other methods and classes here
