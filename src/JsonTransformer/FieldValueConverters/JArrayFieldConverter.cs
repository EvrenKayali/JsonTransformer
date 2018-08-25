using Newtonsoft.Json.Linq;

namespace JsonTransformer.FieldValueConverters
{
    public class JArrayFieldConverter : IFieldValueConverter
    {
        public JToken Convert(JToken value)
        {
            return JArray.Parse(value.ToString());
        }
    }
}