namespace JsonTransformer
{
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;
    using System.Linq;

    public class Transformer
    {
        private readonly FieldMappings mappings;

        public Transformer() {}

        public Transformer(FieldMappings mappings)
        {
            this.mappings = mappings;
        }

        public JObject Transform(JObject json)
        {
            var properties = json.Properties();
            JObject newJobj = new JObject();

            foreach (var prop in properties)
            {
                var cleanName = CleanPrefix(prop.Name);
                var cleanProperty = CleanCData(prop);

                if (mappings != null)
                {
                    if (mappings.ExcludedFields.Any(p => p == cleanName)) continue;

                    AddTransformedFields(newJobj, mappings.FieldDefinitions, cleanName, prop.Value);
                }

                var isPropertyNameAddedAsTransformedField = newJobj.Properties().Any(p => p.Name == cleanName);

                if (!isPropertyNameAddedAsTransformedField)
                {
                    newJobj.Add(cleanName, cleanProperty.Value);
                }
            }

            return newJobj;
        }

        protected virtual string CleanPrefix(string propName)
        {
            if (propName.StartsWith("@"))
            {
                propName = propName.Remove(0, 1);
            }

            return propName;
        }

        protected virtual JProperty CleanCData(JProperty prop)
        {
            bool isCData;

            if (prop.Value.HasValues)
            {
                isCData = prop.Value.First.ToObject<JProperty>().Name == "#cdata-section";
                prop.Value = prop.Value.First.ToObject<JProperty>().Value;
            }

            return prop;
        }

        private void AddTransformedFields(JObject obj, IEnumerable<FieldDefinition> definitions, string fieldName, JToken value)
        {
            var defs = definitions.Where(a => a.FieldName == fieldName);

            if (defs.Any())
            {
                foreach (var def in defs.ToList())
                {
                    if (!string.IsNullOrWhiteSpace(def.TransformName))
                    {
                        fieldName = def.TransformName;
                    }

                    if (def.FieldConverter != null)
                    {
                        value = def.FieldConverter.Convert(value);
                    }

                    obj.Add(fieldName, value);
                }
            }
        }
    }
}