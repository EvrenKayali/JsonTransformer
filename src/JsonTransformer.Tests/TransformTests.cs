using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using Xunit;

namespace JsonTransformer.Tests
{
    public class TransformTests
    {
        JObject testData;

        public TransformTests()
        {
            testData = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(@"Data\data.json"));
        }

        [Fact]
        public void AllTypesShouldBeJObject()
        {
            var transformer = new Transformer();

            var actual = transformer.Transform(testData);
            Assert.IsType<JObject>(testData);
            Assert.IsType<JObject>(actual);


        }

        [Fact]
        public void AllPropertiesShouldShouldBeConvertedIfThereIsNoExcludedProperties()
        {
            var transformer = new Transformer();

            var transformedObject = transformer.Transform(testData);
            var actual = transformedObject.Properties().Count();
            var expected = testData.Properties().Count();
            
            Assert.Equal(expected, actual);

        }
        
        [Fact]
        public void AllPrefixesShuldBeCleanedFromProperties()
        {
            var transformer = new Transformer();

            var transformedObject = transformer.Transform(testData);

            var actual = transformedObject.Properties().Count(p => p.Name.StartsWith("@"));
            var expected = 0;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AllCDataFieldsMustBeTransFormed()
        {
            var transformer = new Transformer();

            var transformedObject = transformer.Transform(testData);

            var actual = transformedObject.Properties()
                .Any(p => p.Value.HasValues && p.Value.First.ToObject<JProperty>().Name == "#cdata-section");
             

            Assert.False(actual);
        }

        [Fact]
        public void ExcluededFieldsShouldBeExcludedFromTransformedObject()
        {
            var mappings = new FieldMappings();
            mappings
                .ExcludeField("creatorID")
                .ExcludeField("creatorName");

            var transformer = new Transformer(mappings);
            var transformedObject = transformer.Transform(testData);

            var actual = transformedObject.Properties()
                .Any(p => p.Name == "creatorID" || p.Name == "creatorName");

            Assert.False(actual);
        }

        [Fact]
        public void TransformNameShouldBeSameWithTheSettings()
        {
            var mappings = new FieldMappings();
            mappings
                .Field(f => f
                    .Name("nodeTypeAlias")
                    .WithTransformName("alias"));


            var transformer = new Transformer(mappings);
            var transformedObject = transformer.Transform(testData);

            var actual = transformedObject.Properties()
                .Any(p => p.Name == "alias" || p.Name != "nodeTypeAlias");

            Assert.True(actual);
        }
    }
}