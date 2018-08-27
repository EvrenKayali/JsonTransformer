namespace JsonTransformer
{
    using JsonTransformer.FieldValueConverters;

    public class FieldDefinition
    {
        public string FieldName { get; set; }
        public string TransformName { get; set; }
        public IFieldValueConverter FieldConverter { get; set; }

        public FieldDefinition Name(string fieldName)
        {
            this.FieldName = fieldName;
            return this;
        }

        public FieldDefinition WithTransformName(string fieldName)
        {
            this.TransformName = fieldName;
            return this;
        }

        public FieldDefinition Converter<T>() where T : IFieldValueConverter, new()
        {
            this.FieldConverter = new T();
            return this;
        }
    }
}