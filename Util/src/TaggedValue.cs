
namespace hoTools.Utils
{
    public static class TaggedValue
    {
        /// <summary>
        /// Get Tagged Value with 'Name'. If tagged value doesn't exists a new one is created
        /// </summary>
        /// <param name="el"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static EA.TaggedValue AddTaggedValue(EA.Element el, string name)
        {
            EA.TaggedValue tagStart = null;
            foreach (EA.TaggedValue taggedValue in el.TaggedValues)
            {
                if (taggedValue.Name == name)
                {
                    tagStart = taggedValue;
                    break;
                }
            }
            if (tagStart == null)
            {
                // create tagged value
                tagStart = (EA.TaggedValue)el.TaggedValues.AddNew(name, "Tag");
                el.TaggedValues.Refresh();
            }
            return tagStart;
        }
        /// <summary>
        /// Set Tagged Value with 'Name' to a value. If tagged value doesn't exists a new one is created. 
        /// </summary>
        /// <param name="el"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static EA.TaggedValue SetTaggedValue(EA.Element el, string name, string value)
        {
            EA.TaggedValue tg = AddTaggedValue(el, name);
            tg.Value = value;
            tg.Update();
            return tg;
        }
    }
}
