namespace JsonTransformer.FieldValueConverters
{
    using Newtonsoft.Json.Linq;

    public class IntFieldConverter : IFieldValueConverter
    {
        public JToken Convert(JToken value)
        {
            return int.Parse(value.ToString());
        }
    }
}