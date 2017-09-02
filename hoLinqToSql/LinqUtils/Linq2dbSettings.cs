using System.Collections.Generic;
using LinqToDB.Configuration;

namespace hoLinqToSql.LinqUtils
{
        /// <summary>
        /// Set connection string before the first usage. It only sets the default configuration.
        /// Mostly it's better to use the Context Constructor to set provider and connectionString.
        /// - using (var db = new DataModels.EaDataModel(provider, connectionString)) 
        /// 
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
        public class HoLinq2DbSettings : ILinqToDBSettings
        {
            readonly string _provider;
            private readonly string _connectionString;

            /// <summary>
            /// Sets provider and connectionString for the next linq2db connection
            /// </summary>
            /// <param name="provider"></param>
            /// <param name="connectionString"></param>
            public HoLinq2DbSettings(string provider, string connectionString)
            {
                _provider = provider;
                _connectionString = connectionString;
            }

            /// <summary>
            /// Linq2db internal due to set connectionString
            /// </summary>
            public IEnumerable<IDataProviderSettings> DataProviders
            {
                get { yield break; }
            }

            public string DefaultConfiguration => _connectionString;   
            public string DefaultDataProvider => _provider;

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
