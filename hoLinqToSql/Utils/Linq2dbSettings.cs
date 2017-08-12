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

            public hoLinq2DBSettings(string provider, string connectionString)
            {
                _provider = provider;
                _connectionString = connectionString;
            }


            public IEnumerable<IDataProviderSettings> DataProviders
            {
                get { yield break; }
            }

            public string DefaultConfiguration => "AccessForEA";    //??
            public string DefaultDataProvider => "Access";//??

            public IEnumerable<IConnectionStringSettings> ConnectionStrings
            {
                get
                {
                    yield return
                        new ConnectionStringSettings
                        {
                            Name = "AccessForEA",      // only name to show
                            ProviderName = _provider, // has to be correct driver name
                            ConnectionString = _connectionString
                        };
                }
            }
        }


}
