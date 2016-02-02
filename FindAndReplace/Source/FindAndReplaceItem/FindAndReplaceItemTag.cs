using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hoTools.Find
{
    public class FindAndReplaceItemTag
    {
        protected string _name = null;
        protected string _value = null;
        #region Constructor
        public FindAndReplaceItemTag(string name, string value)
        {
            _name = name;
            _value = value;
        }
        #endregion
        #region Properties
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
    

        #endregion
        #region save
        public virtual void save() { return; }
        #endregion
        #region load
        public virtual void load() { return; }
        protected void load(string name, string value)
        {
            _name = name;
            _value = value;
        }
        #endregion

    }
    public class FindAndReplaceItemTagElement : FindAndReplaceItemTag
    {
        private EA.TaggedValue _tag = null;
        #region Constructor
        public FindAndReplaceItemTagElement(EA.TaggedValue tag) :base(tag.Name, tag.Value)
        {
            _tag = tag;
        }
        #endregion
        #region Properties
        public EA.TaggedValue Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }
        #endregion
        #region load
        public override void load()
        {
            load(_tag.Name, _tag.Value);
        }
        #endregion
        #region save
        public override void save()
        {
            _tag.Value = this._value;
            _tag.Update();
        }
        #endregion
    }
    public class FindAndReplaceItemTagAttribute : FindAndReplaceItemTag
    {
        EA.AttributeTag _tag = null;
        #region Constructor
        public FindAndReplaceItemTagAttribute(EA.AttributeTag tag) :base(tag.Name, tag.Value)
        {
            _tag = tag;
        }
        #endregion
        #region Properties
        public EA.AttributeTag Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }
        #endregion
        #region load
        public override void load()
        {
            
        }
        #endregion
        #region save
        public override void save()
        {
            _tag.Value = Value;
            _tag.Update();
        }
        #endregion
    }

    public class FindAndReplaceItemTagMethod : FindAndReplaceItemTag
    {
        EA.MethodTag _tag = null;
        #region Constructor
        public FindAndReplaceItemTagMethod(EA.MethodTag tag) :base(tag.Name, tag.Value)
        {
            _tag = tag;
        }
        #endregion
        #region Properties
        public EA.MethodTag Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }
        #endregion
        #region load
        public override void load()
        {
            
        }
        #endregion
        #region save
        public override void save()
        {
            _tag.Value = Value;
            _tag.Update();
        }
        #endregion
    }
    public class FindAndReplaceItemTagPackage : FindAndReplaceItemTag
    {
        private EA.TaggedValue _tag = null;
        #region Constructor
        public FindAndReplaceItemTagPackage(EA.TaggedValue tag)
            : base(tag.Name, tag.Value)
        {
            _tag = tag;
        }
        #endregion
        #region Properties
        public EA.TaggedValue Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }
        #endregion
        #region load
        public override void load()
        {

        }
        #endregion
        #region save
        public override void save()
        {
            _tag.Value = Value;
            _tag.Update();
        }
        #endregion
    }
}
