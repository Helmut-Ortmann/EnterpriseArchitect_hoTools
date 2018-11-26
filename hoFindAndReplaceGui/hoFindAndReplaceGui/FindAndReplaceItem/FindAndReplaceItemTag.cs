namespace hoTools.Find
{
    /// <summary>
    /// SearchReplace item with all possible values to change in it.
    /// </summary>
    public class FindAndReplaceItemTag
    {
        private string _name ;
        private string _value;
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
            get => _name;
            set => _name = value;
        }
        public string Value
        {
            get => _value;
            set => _value = value;
        }
    

        #endregion
        #region save
        public virtual void Save() { }
        #endregion
        #region load
        public virtual void Load() { }
        protected void Load(string name, string value)
        {
            _name = name;
            _value = value;
        }
        #endregion

    }
    public class FindAndReplaceItemTagElement : FindAndReplaceItemTag
    {
        private EA.TaggedValue _tag ;
        #region Constructor
        public FindAndReplaceItemTagElement(EA.TaggedValue tag) :base(tag.Name, Utils.TaggedValue.GetTaggedValue(tag))
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
        #region Load
        public override void Load()
        {
            Load(_tag.Name, _tag.Value);
        }
        #endregion
        #region save
        public override void Save()
        {
            Utils.TaggedValue.SetTaggedValue(_tag,Value);
        }
        #endregion
    }
    public class FindAndReplaceItemTagAttribute : FindAndReplaceItemTag
    {
        EA.AttributeTag _tag;
        #region Constructor
        public FindAndReplaceItemTagAttribute(EA.AttributeTag tag) :base(tag.Name, Utils.TaggedValue.GetTaggedValue(tag))
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
        #region Load
        public override void Load()
        {
            
        }
        #endregion
        #region save
        public override void Save()
        {
            _tag.Value = Value;
            _tag.Update();
        }
        #endregion
    }

    public class FindAndReplaceItemTagMethod : FindAndReplaceItemTag
    {
        EA.MethodTag _tag;
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
        #region lLoad
        public override void Load()
        {
            
        }
        #endregion
        #region Save
        public override void Save()
        {
            _tag.Value = Value;
            _tag.Update();
        }
        #endregion
    }
    public class FindAndReplaceItemTagPackage : FindAndReplaceItemTag
    {
        private EA.TaggedValue _tag;
        #region Constructor
        public FindAndReplaceItemTagPackage(EA.TaggedValue tag)
            : base(tag.Name, Utils.TaggedValue.GetTaggedValue(tag))
        {
            _tag = tag;
        }
        #endregion
        #region Properties
        public EA.TaggedValue Tag
        {
            get => _tag;
            set => _tag = value;
        }
        #endregion
        #region Load
        public override void Load()
        {

        }
        #endregion
        #region Save
        public override void Save()
        {
            _tag.Value = Value;
            _tag.Update();
        }
        #endregion
    }
}
