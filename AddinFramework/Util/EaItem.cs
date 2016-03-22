
namespace EAAddinFramework.Utils
{
    public class EaItem
    {
        public object Obj { get; }
        public object ObjType { get; }

        public EaItem(object obj, object objType)
        {
            Obj = obj;
            ObjType = objType;
        }
    }
}
