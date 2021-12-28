using System.Collections.Generic;
using hoTools.Utils;

namespace hoTools.EaServices.CopyTag
{
    public class ElTag
    {
        private List<EA.TaggedValue> _lTv = new List<EA.TaggedValue>();
        private List<EA.TaggedValue> _lTvEx = new List<EA.TaggedValue>();
        private EA.Element _el;
        public ElTag(EA.Element el)
        {
            _el = el;
            foreach (EA.TaggedValue tvEx in el.TaggedValuesEx)
            {
                _lTvEx.Add(tvEx);
            }
            foreach (EA.TaggedValue tv in el.TaggedValues)
            {
                _lTv.Add(tv);
            }
        }
        /// <summary>
        /// Copy TV to current element
        /// </summary>
        /// <param name="el"></param>
        public void ElTvCopy(EA.Element el)
        {
            foreach (EA.TaggedValue tv in _lTv)
            {
                var tvNew = TaggedValue.Add(el, tv.Name);
                TaggedValue.SetTaggedValue(tvNew, tv.Value);
                tv.Update();
            }
        }
    }
}
