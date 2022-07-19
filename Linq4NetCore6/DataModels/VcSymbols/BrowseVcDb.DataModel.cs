using System;
using LinqToDB.DataProvider;

// ReSharper disable once CheckNamespace
namespace DataModels.VcSymbols
{
    // ReSharper disable once InconsistentNaming
    public partial class BrowseVcDB
    {
        /// <summary>
        /// Create lind2db context wit new Provide, ConnectionString for Visual Studio Code Symbol SQLite Database
        /// </summary>
        /// <param name="dataProvider"></param>
        /// <param name="connectionString"></param>
        public BrowseVcDB(IDataProvider dataProvider, string connectionString) : base(dataProvider, connectionString)
        {
            InitDataContext();
        }


        
    }
}
