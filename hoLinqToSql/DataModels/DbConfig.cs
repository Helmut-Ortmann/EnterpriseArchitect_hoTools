using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace hoLinqToSql.DataModels
{
    public class DbConfig
    {
        public const bool IsMySqlConnector = false;

        // Initializing the properties for different databases
        // See: https://linq2db.github.io/articles/general/databases.html
        // Connection strings: https://www.connectionstrings.com/
        public static readonly Dictionary<string, DatabaseProperties> DatabaseProperties = new Dictionary<string, DatabaseProperties>
{
    {
        "MYSQL",            // db name
        new DatabaseProperties
        {
            MacroName = "#DB=MySql#",
            OdbcDiverName = "",  // not needed
            LinqName = "MySql",
            BoolStringParser = boolString => "1 true TRUE".Contains(boolString),
            BoolToStringConverter = b => b ? "1" : "0"
        }
    },
    {
        "MySqlConnector",   // db name
        new DatabaseProperties
        {
            MacroName = "#DB=MySql#",
            OdbcDiverName = "",  // not needed
            LinqName = "MySql",
            BoolStringParser = boolString => "1 true TRUE".Contains(boolString),
            BoolToStringConverter = b => b ? "1" : "0"
        }
    },
    {   // SQLite
        "SL3",          // db name
        new DatabaseProperties
        {
            MacroName = "#DB=SQLITE#",
            OdbcDiverName = "System.Data.Sqlite",
            LinqName = "System.Data.Sqlite",
            BoolStringParser = boolString => "1 true TRUE".Contains(boolString),
            BoolToStringConverter =  b => b ? "1" : "0"
        }
    },
    {   // ACCESS2007
        "ACCESS",       // db name
        new DatabaseProperties
        {
            MacroName = "#DB=ACCESS2007#",
            BoolStringParser = boolString => "1 true TRUE".Contains(boolString),
            BoolToStringConverter =  b => b ? "1" : "0"
        }
    },
    {   // JET
        "JET",       // db name
        new DatabaseProperties
        {
            MacroName = "#DB=JET#",
            OdbcDiverName = "Microsoft.JET.OLEDB.4.0",
            LinqName = "Access",
            BoolStringParser = boolString => "1 true TRUE".Contains(boolString),
            BoolToStringConverter =  b => b ? "1" : "0"
        }
    },
    {   // SQL Server
        "SqlSvr",            // db name
        new DatabaseProperties
        {
            MacroName = "#DB=SqlSvr#",
            OdbcDiverName = "SqlServer",
            LinqName = "SqlServer",
            BoolStringParser = boolString => boolString.Equals("Y", StringComparison.OrdinalIgnoreCase),
            BoolToStringConverter = b => b ? "Y" : "N"
        }
    },
        {   // ORACLE
        "ORACLE",            // db name
        new DatabaseProperties
        {
            MacroName = "#DB=ORACLE#",
            BoolStringParser = boolString => boolString.Equals("Y", StringComparison.OrdinalIgnoreCase),
            BoolToStringConverter = b => b ? "Y" : "N"
        }
    },
        {   // POSTGRES
            "POSTGRES",      // db name
            new DatabaseProperties
            {
                MacroName = "#DB=ORACLE#",
                BoolStringParser = boolString => boolString.Equals("Y", StringComparison.OrdinalIgnoreCase),
                BoolToStringConverter = b => b ? "Y" : "N"
            }
        },
    {   // FIREBIRD
            "FIREBIRD",      // db name
            new DatabaseProperties
            {
                MacroName = "#DB=FIREBIRD#",
                BoolStringParser = boolString => boolString.Equals("Y", StringComparison.OrdinalIgnoreCase),
                BoolToStringConverter = b => b ? "Y" : "N"
            }
        },
    {   // ASA
        "ASA",           // db name
        new DatabaseProperties
        {
            MacroName = "#DB=ASA#",
            BoolStringParser = boolString => boolString.Equals("Y", StringComparison.OrdinalIgnoreCase),
            BoolToStringConverter = b => b ? "Y" : "N"
        }
    }
};
        /// <summary>
        /// Get DB properties for EA DB according to Repository type
        /// </summary>
        /// <returns>DatabaseProperties</returns>
        public static DatabaseProperties GetDbProperties(EA.Repository rep)
        {
            // Find Database
            var dbProperty = DatabaseProperties.Where(x => x.Key.ToLower() == RepType(rep).ToLower())
                .Select(x => x.Value).FirstOrDefault();
            if (dbProperty == null)
            {

                MessageBox.Show(
                    $@"DB rep.RepositoryType()/RepType(rep): '{rep.RepositoryType()}/{RepType(rep)}' in macro #DB=...# not supported in SQL, only 'ACCESS', 'MYSQL', 'JET', 'SQLite/SL3', 'SL3'!

This can happen:
- If you use a different DB
- EA.Repository object 'rep' with: EA Server RPC issues, Shutdown of Repository", $@"hoTools/hoReverse DB {RepType(rep)} not supported or EA Server down");
                return null;
            }
            return dbProperty;
        }
        /// <summary>
        /// Get DB properties for dbName
        /// </summary>
        /// <returns>DatabaseProperties</returns>
        public static DatabaseProperties GetDbProperties(string dbName)
        {

            var dbProperty = DatabaseProperties.Where(x => x.Key == dbName)
                .Select(x => x.Value).FirstOrDefault();
            if (dbProperty == null)
            {

                MessageBox.Show(
                    $@"DB rep.RepositoryType()/RepType(rep): '{dbName}/{dbName}' in macro #DB=...# not supported in SQL, only 'ACCESS', 'MYSQL', 'JET', 'SQLite/SL3', 'SL3'!

This can happen:
- If you use a different DB
- EA.Repository object 'rep' with: EA Server RPC issues, Shutdown of Repository", $@"hoTools/hoReverse DB {dbName} not supported or EA Server down");
                return null;
            }
            return dbProperty;
        }
        /// <summary>
        /// General abstraction of the EA repository type
        ///
        /// See:
        /// https://linq2db.github.io/articles/general/databases.html
        ///
        /// Rational:
        /// Adapt special Repository Types like:
        /// ACCESS
        /// ACCESS207
        /// Asa
        /// Firebird
        /// JET
        /// MSQL/MySqlConnector
        /// Oracle
        /// Postgres
        /// SqlSvr
        /// SL3       SQLite
        /// </summary>
        /// <param name="rep"></param>
        /// <returns></returns>
        public static string RepType(EA.Repository rep)
        {
            string repType = rep.RepositoryType().ToUpper();
            if (repType == "MYSQL" && IsMySqlConnector) return "MySqlConnector";  // if using MySqlConnector
            if (repType.StartsWith("ACCESS")) return "ACCESS";
            return repType;
        }
    }
    /// <summary>
    /// Properties of the EA Databases
    /// </summary>
    public class DatabaseProperties
    {
        /// <summary>
        /// Name of the EA macro associated with the database
        /// </summary>
        public string MacroName { get; set; }
        
        /// <summary>
        /// Name of the ODBC driver
        /// </summary>
        public string OdbcDiverName { get; set; }

        /// <summary>
        /// Linq driver name
        /// </summary>
        public string LinqName { get; set; }
        /// <summary>
        /// Function to parse a bool string to get the bool representation
        /// </summary>
        public Func<string, bool> BoolStringParser { get; set; }
        
        /// <summary>
        /// Function to convert a boolean to a string representation
        /// </summary>
        public Func<bool, string> BoolToStringConverter { get; set; }
    }
}
