namespace JsonTransformer.FieldValueConverters
{
    using Newtonsoft.Json.Linq;

    public class JArrayFieldConverter : IFieldValueConverter
    {
        public JToken Convert(JToken value)
        {
            return JArray.Parse(value.ToString());
        }
    }
}