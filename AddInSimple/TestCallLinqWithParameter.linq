<Query Kind="Program">
  <Connection>
    <ID>dc97a378-2f1b-42f9-8f32-774012bf0e94</ID>
    <Persist>true</Persist>
    <Driver Assembly="linq2db.LINQPad" PublicKeyToken="f19f8aed7feff67e">LinqToDB.LINQPad.LinqToDBDriver</Driver>
    <Provider>System.Data.OleDb</Provider>
    <Server>d:\hoData\Projects\00Current\ZF\Work\Software_Architekturdesign_WLE_Work.eap</Server>
    <DbVersion>04.00.0000</DbVersion>
    <CustomCxString>Provider=Microsoft.Jet.OLEDB.4.0;Data Source=d:\hoData\Projects\00Current\ZF\Work\Software_Architekturdesign_WLE_Work.eap;</CustomCxString>
    <DriverData>
      <providerName>Access</providerName>
      <optimizeJoins>true</optimizeJoins>
      <allowMultipleQuery>false</allowMultipleQuery>
      <commandTimeout>0</commandTimeout>
    </DriverData>
  </Connection>
</Query>

//
// Test the hoTools call of LINQPad with context information:
// - Context item
// - Diagram
// - Selected Diagram Objects
// - Selected Diagram Connector
// - Selected TreeSelected Elements
// - Search Term
// You can do this with: hoLinqToSql.LinqUtils.LinqPad Method: GetArg()

void Main(string[] args)
{
	// Component to run on or ""
	string object_type = "Package";
#if CMD
		if (args != null && args.Length > 0)
		{
		object_type = args[0];
		args.Dump("");
		"_".Dump($"Run by Batch for type '{object_type}'");
		}
#else
	"_".Dump("Run by Dialog");
#endif
	runCode(object_type);
}

// Define other methods and classes here
void runCode(string object_type) {
	var read = (from o in t_object
			   where o.Object_Type == object_type || o.Object_Type == ""
			   select new { o.Name}).Take(10);
    read.Dump();

}