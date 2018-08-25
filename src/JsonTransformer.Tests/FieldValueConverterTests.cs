using JsonTransformer.FieldValueConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
namespace JsonTransformer.Tests
{
    using System.Linq;
    using Xunit;

    public class FieldValueConverterTests
    {
        JObject testData;

        public FieldValueConverterTests()
        {
            testData = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(@"Data\data.json"));
        }

        [Fact]
        public void FieldValueConvertersShouldBeChosenFromFieldSettings()
        {
            var mappings = new FieldMappings();
            mappings
                .Field(f => f
                    .Name("generalCapabilities")
                    .Converter<JArrayFieldConverter>());

            var transformer = new Transformer(mappings);

            var transformedObject = transformer.Transform(testData);

            var actual = transformedObject["generalCapabilities"].Value<JArray>().Count;

            var expected = 8;

            Assert.Equal(expected, actual);
                    
        }

        [Fact]
        public void SplitedIntegerArrayConverterShoulConvertValueToIntArray()
        {
            var mappings = new FieldMappings();

            mappings
             .Field(f => f
                    .Name("path")
                    .Converter<SplitedIntArrayFieldConverter>());

            var transformer = new Transformer(mappings);

            var transformedObject = transformer.Transform(testData);
            var arr = transformedObject["path"].ToObject<int[]>();
          
            var actual = arr[0];
            var expected = -1;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SameFieldCanTransformMultipleTimesWithDifferentNames()
        {
            var mappings = new FieldMappings();

            mappings
             .Field(f=> f
                    .Name("path"))
             .Field(f => f
                    .Name("path")
                    .Converter<SplitedIntArrayFieldConverter>()
                    .WithTransformName("intPath"))
             .Field(f => f
                    .Name("path")
                    .Converter<JArrayFieldConverter>()
                    .WithTransformName("stringPath"));

            var transformer = new Transformer(mappings);

            var transformedObject = transformer.Transform(testData);

            var isPathExist = transformedObject.Properties().Any(p => p.Name == "path");
            var isIntPathExist = transformedObject.Properties().Any(p => p.Name == "intPath");
            var isStringPathExist = transformedObject.Properties().Any(p => p.Name == "stringPath");

            var actual = isPathExist && isIntPathExist && isStringPathExist;
            
            Assert.True(actual);
        }
    }
}