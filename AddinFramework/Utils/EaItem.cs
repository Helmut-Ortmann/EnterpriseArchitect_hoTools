//using EA;
namespace EAAddinFramework.Utils
{
    /// <summary>
    /// EA item (Diagram, Package, Element,..) to remember. It stores: GUID, ObjectType as String, ObjectType, EA object as Object
    /// </summary>
    public class EaItem
    {
        public string Guid { get; private set; }
        public string SqlObjectType { get; private set; }
        public EA.ObjectType EaObjectType {get; set;}
        public object EaObject { get; set; }

        #region Constructor
        /// <summary>
        /// Constructor of an EAItem
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="sqlObjType"></param>
        /// <param name="eaObjectType"></param>
        /// <param name="eaObject"></param>
        public EaItem(string guid, string sqlObjType, EA.ObjectType eaObjectType, object eaObject)
        {
            Init(guid, sqlObjType, eaObjectType, eaObject);
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="sqlObjType"></param>
        /// <param name="eaObjectType"></param>
        public EaItem(string guid, string sqlObjType, EA.ObjectType eaObjectType)
        {
            Init(guid, sqlObjType, eaObjectType, null);
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="sqlObjType"></param>
        public EaItem(string guid, string sqlObjType)
        {
            Init(guid, sqlObjType, EA.ObjectType.otNone, null);
        }
        #endregion

        #region Initialize
        /// <summary>
        /// Initialize the EaItem
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="sqlObjType"></param>
        /// <param name="objType"></param>
        /// <param name="eaObject"></param>
        private void Init(string guid, string sqlObjType, EA.ObjectType objType, object eaObject)
        {
            Guid = guid;
            SqlObjectType = sqlObjType;
            EaObjectType = objType;
            EaObject = eaObject;
        }
        #endregion

    }
}
