using LinqToDB.DataProvider;

namespace hoLinqToSql.DataModels
{
    /// <summary>
    /// Add additional initializations for linq2db
    /// </summary>
    public partial class EaDataModel : LinqToDB.Data.DataConnection
    {
        /// <summary>
        /// Create lind2db context wit new ProviderName and ConnectionString
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="connectionString"></param>
        public EaDataModel(string providerName, string connectionString) : base(providerName, connectionString)
        {
        }
        /// <summary>
        /// Create lind2db context wit new DataProvider and ConnectionString
        /// </summary>
        /// <param name="dataProvider"></param>
        /// <param name="connectionString"></param>
        public EaDataModel(IDataProvider dataProvider, string connectionString) : base(dataProvider, connectionString)
        {
        }
        


    }
}
