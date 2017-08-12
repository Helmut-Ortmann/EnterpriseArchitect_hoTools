using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB.Configuration;

namespace hoLinqToSql.Utils
{
        /// <summary>
        /// Set connection string
        /// </summary>
        public class ConnectionStringSettings : IConnectionStringSettings
        {
            public string ConnectionString { get; set; }
            public string Name { get; set; }
            public string ProviderName { get; set; }
            public bool IsGlobal => false;
        }
        /// <summary>
        /// Set the settings for Linq2DB.
        /// - Provider
        /// - ConnectionString
        /// </summary>
        public class hoLinq2DBSettings : ILinqToDBSettings
        {
            readonly string _provider;
            private readonly string _connectionString;

            /// <summary>
            /// Sets provider and connectionString for the next linq2db connection
            /// </summary>
            /// <param name="provider"></param>
            /// <param name="connectionString"></param>
            public hoLinq2DBSettings(string provider, string connectionString)
            {
                _provider = provider;
                _connectionString = connectionString;
            }

            /// <summary>
            /// Linq2db internal sue to set connectionString
            /// </summary>
            public IEnumerable<IDataProviderSettings> DataProviders
            {
                get { yield break; }
            }

            public string DefaultConfiguration => "AccessForEA";    //??
            public string DefaultDataProvider => "Access";//??

            /// <summary>
            /// Linq2db internal sue to set connectionString
            /// </summary>
            public IEnumerable<IConnectionStringSettings> ConnectionStrings
            {
                get
                {
                    yield return
                        new ConnectionStringSettings
                        {
                            Name = "Linq2db_EA",      // only name to show
                            ProviderName = _provider, // has to be correct driver name
                            ConnectionString = _connectionString
                        };
                }
            }
        }


}
