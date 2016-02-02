using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hoTools.Utils
{
    public class TaggedValue
    {
        public static EA.TaggedValue addTaggedValue(EA.Element shm, string name)
        {
            EA.TaggedValue tagStart = null;
            foreach (EA.TaggedValue taggedValue in shm.TaggedValues)
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
                tagStart = (EA.TaggedValue)shm.TaggedValues.AddNew(name, "Tag");
                shm.TaggedValues.Refresh();
            }
            return tagStart;
        }
    }
}
