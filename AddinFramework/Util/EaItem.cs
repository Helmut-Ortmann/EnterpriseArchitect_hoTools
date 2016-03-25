//using EA;
namespace EAAddinFramework.Utils
{
    public class EaItem
    {
        public string Guid { get; private set; }
        public string SqlObjectType { get; private set; }
        public EA.ObjectType EaObjectType {get; set;}
        public object EaObject { get; set; }

        public EaItem(string guid, string sqlObjType, EA.ObjectType eaObjectType, object eaObject)
        {
            init(guid, sqlObjType, eaObjectType, eaObject);
        }

        public EaItem(string guid, string sqlObjType, EA.ObjectType eaObjectType)
        {
            init(guid, sqlObjType, eaObjectType, null);
        }
        public EaItem(string guid, string sqlObjType)
        {
            init(guid, sqlObjType, EA.ObjectType.otNone, null);
        }
        void init(string guid, string sqlObjType, EA.ObjectType objType, object eaObject)
        {
            Guid = guid;
            SqlObjectType = sqlObjType;
            EaObjectType = objType;
            EaObject = eaObject;
        }

    }
}
