using Newtonsoft.Json.Linq;

namespace JsonTransformer.FieldValueConverters
{
    public interface IFieldValueConverter
    {
        JToken Convert(JToken value);
    }
}