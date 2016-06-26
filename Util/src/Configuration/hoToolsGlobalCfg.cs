
namespace hoTools.Utils.Configuration
{
    public sealed class HoToolsGlobalCfg: IHoToolsGlobalCfg
    {
        string _paths;
        /// <summary>
        /// Allocate ourselves.
        /// We have a private constructor, so no one else can.
        /// </summary>
        static readonly HoToolsGlobalCfg _instance = new HoToolsGlobalCfg();
        HoToolsGlobalCfg()
        {
            
        }
        /// <summary>
        /// Access HoToolsGlobalCfg.Instance to get the singleton object.
        /// Then call methods on that instance.
        /// </summary>
        public static HoToolsGlobalCfg Instance
        {
            get { return _instance; }
        }
        public string GetSqlPaths()
        {
            return _paths;
        }
        public void SetSqlPaths(string paths)
        {
            _paths = paths;
        }
    }
}
