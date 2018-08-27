namespace JsonTransformer.FieldValueConverters
{
    using Newtonsoft.Json.Linq;

    public interface IFieldValueConverter
    {
        JToken Convert(JToken value);
    }
}