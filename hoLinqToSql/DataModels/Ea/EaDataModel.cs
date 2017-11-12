using LinqToDB.DataProvider;


// ReSharper disable once CheckNamespace
namespace DataModels
{
    public partial class EaDataModel
    {
        /// <summary>
        /// Create lind2db context wit new Provide, ConnectionString
        /// </summary>
        /// <param name="dataProvider"></param>
        /// <param name="connectionString"></param>
        public EaDataModel(IDataProvider dataProvider, string connectionString) : base(dataProvider, connectionString)
        {
            InitDataContext();
        }
        


    }
}
