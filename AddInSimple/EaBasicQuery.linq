<Query Kind="Statements">
  <Connection>
    <ID>d4163967-31a9-4051-ac70-170962493218</ID>
    <Persist>true</Persist>
    <Driver Assembly="linq2db.LINQPad" PublicKeyToken="f19f8aed7feff67e">LinqToDB.LINQPad.LinqToDBDriver</Driver>
    <Server>localhost\SQLEXPRESS</Server>
    <Database>EAExample</Database>
    <DbVersion>13.00.4001</DbVersion>
    <CustomCxString>Server=localhost\SQLEXPRESS;Database=EAExample;Trusted_Connection=True;</CustomCxString>
    <DriverData>
      <providerName>SqlServer</providerName>
      <optimizeJoins>true</optimizeJoins>
      <allowMultipleQuery>false</allowMultipleQuery>
      <commandTimeout>0</commandTimeout>
      <providerVersion>SqlServer.2014</providerVersion>
    </DriverData>
  </Connection>
</Query>

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
q1.Dump("EAExample JET, SqlServer, MySQL,..");
// Total requirements
var sum = q.Concat(q1);
sum.Dump();