using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hoTools.Utils;

namespace hoTools.EaServices.src.CopyTag
{
    public class ElTag
    {
        private List<EA.TaggedValue> _lTv;
        private List<EA.TaggedValue> _lTvEx;
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
