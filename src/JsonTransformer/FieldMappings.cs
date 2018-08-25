using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonTransformer
{
    public class FieldMappings
    {
        public List<string> ExcludedFields { get; set; }
        public List<FieldDefinition> FieldDefinitions { get; set; }

        public FieldMappings()
        {
            ExcludedFields = new List<string>();
            FieldDefinitions = new List<FieldDefinition>();
        }
        public FieldMappings ExcludeField(string fieldName)
        {
            ExcludedFields.Add(fieldName);
            return this;
        }

        public FieldMappings Field(Func<FieldDefinition,FieldDefinition> addFieldDefinition)
        {
            var fieldDefiner = new FieldDefinition();
            var a = addFieldDefinition(fieldDefiner);
            FieldDefinitions.Add(a);
            return this;
        }
    }
}
