namespace JsonTransformer.FieldValueConverters
{
    using Newtonsoft.Json.Linq;

    public class JObjectFieldConverter : IFieldValueConverter
    {
        public JToken Convert(JToken value)
        {
            return JObject.Parse(value.ToString());
        }
    }
}