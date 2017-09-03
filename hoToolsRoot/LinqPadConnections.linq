<Query Kind="Program" />

//
// Display all specified LINQPad connections. You can configure this script by:
// - *.xml file of LINQPad connections
// - server to search for
// - database to search for
//


void Main(string[] args)
{
	// Configuration
	// - For file base connection: Only server with full path
	// - For a database connection: server + db
	// The connection name: This what you see in the LINQPad Connection view 
	//'[Access] c:\programme\...EAExample.eap\ (v.04.00.000)'
	//'[SqlServer] localhost\SQLEXPRESS\EAExample (v.13.00.4001)'
	string LINQPadConnectionFile = @"c:\Users\helmu_000\AppData\Roaming\LINQPad\ConnectionsV2.xml";
	string server = $@"C:\Users\helmu_000\AppData\Roaming\Sparx Systems\EA\EAExample.eap";
	string db = null;
	
	XDocument xConnections = XDocument.Load(LINQPadConnectionFile);


	var connections = from c in xConnections.Descendants("Connection")
					  orderby c.Element("Server").Value, c.Element("Database")?.Value, c.Element("DriverData")?.Element("providerName")?.Value
					  select new
					  {
						  Display = c.Element("DisplayName")?.Value,
						  Server = c.Element("Server").Value,
						  Database = c.Element("Database")?.Value,
						  Provider = c.Element("DriverData")?.Element("providerName")?.Value,
						  DBVersion = c.Element("DbVersion")?.Value,
						  ID = c.Element("ID").Value
					  };
	connections.Dump("LINQPad Connections");

	string[] connectionSearch = new string[] { $"Connections:  {LINQPadConnectionFile}", $"Server:          {server}", $"Database:       {db}" };
    connectionSearch.Dump("LINQPad search for in list of LINQPad connections");
	
	string provider = null;
	string linqPadConnectionsV2 = @"C:\Users\helmu_000\AppData\Roaming\LINQPad\ConnectionsV2.xml";
	XDocument xConnection = XDocument.Load(linqPadConnectionsV2);
	var connection = (from c in xConnection.Descendants("Connection")
					  where c.Element("Server").Value.ToLower() == server.ToLower() &&
							(String.IsNullOrWhiteSpace(db) || c.Element("Database")?.Value == db)
					  //&& c.Element("DriverData")?.Element("providerName")?.Value == provider
					  select new
					  {
						  Provider = c.Element("DriverData")?.Element("providerName")?.Value == null ? "" : $"[{c.Element("DriverData").Element("providerName").Value}]",
						  Server = c.Element("Server")?.Value,
						  DataBase = c.Element("Database")?.Value == null ? "" : $"{c.Element("Database")?.Value}",
						  DbVersion = c.Element("DbVersion")?.Value == null ? "" : $"(v.{c.Element("DbVersion").Value})"
					  }).FirstOrDefault();
	string linqPadConnectionName = "";
	connection.Dump("LINQPad current connection details");
	linqPadConnectionName = $@"{connection.Provider} {connection.Server}\{connection.DataBase} {connection.DbVersion}";
	// adapt string to special provider needs
	switch (connection.Provider)
	{
		// LINQPad standard driver
		case null:
			linqPadConnectionName = $@"{connection.Server}.{connection.DataBase}";
			break;
	}
	linqPadConnectionName.Dump("LINQPad current connection cxname to use with LPRUn.exe");
	//startLprun(linqPadConnectionName);
}
	// run LPRun.exe


string startLprun(string connection)
{
		// initialize ProzessInfo
		Process p = new Process();
		ProcessStartInfo _startInfo = new ProcessStartInfo();
		p.StartInfo.CreateNoWindow = false;
		p.StartInfo.UseShellExecute = false;
		p.StartInfo.FileName = @"c:\Program Files (x86)\LINQPad5\LPRun.exe";
		p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
	    string linqPadFile = Util.CurrentQueryPath;
	    p.StartInfo.Arguments = $@"-lang=program -format=html ""-cxname={connection}"" ""{Util.CurrentQueryPath}""  "; 
		p.StartInfo.Arguments.Dump();
		p.StartInfo.RedirectStandardOutput = true;
		p.StartInfo.RedirectStandardError = true;
		p.EnableRaisingEvents = true;

		p.Start();
		string output = p.StandardOutput.ReadToEnd();

		int exitCode = 0;
		using (StreamReader s = p.StandardError)
		{
			

		    string error = s.ReadToEnd();
			p.WaitForExit(60000);
			exitCode = p.ExitCode;
			if (exitCode != 0)
			{
				$"Output: {output}Error: {error}".Dump("Error occured");
			}
			else
			{
				"".Dump("OK");
			}
		}
	    return exitCode.ToString();
}