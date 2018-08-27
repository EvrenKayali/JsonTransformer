namespace JsonTransformer.Tests
{
    using JsonTransformer.FieldValueConverters;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Shouldly;
    using System.IO;
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

            var generalCapabiltyCount = transformedObject["generalCapabilities"].Value<JArray>().Count;

            generalCapabiltyCount.ShouldBe(8);
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

            var firstElementOfConvertedArray = arr[0];

            firstElementOfConvertedArray.ShouldBe(-1);
        }

        [Fact]
        public void SameFieldCanTransformMultipleTimesWithDifferentNames()
        {
            var mappings = new FieldMappings();

            mappings
             .Field(f => f
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

            var isAllPathsExist = isPathExist && isIntPathExist && isStringPathExist;

            isAllPathsExist.ShouldBeTrue();
        }

        [Fact]
        public void IntValueConverter()
        {
            var mappings = new FieldMappings();

            mappings
             .Field(f => f
                    .Name("parentID")
                    .Converter<IntFieldConverter>());

            var transformer = new Transformer(mappings);

            var transformedObject = transformer.Transform(testData);

            var id = transformedObject["parentID"].ToObject<int>();

            id.ShouldBe(14735);
        }

        [Fact]
        public void ObjValueConverter()
        {
            var mappings = new FieldMappings();

            mappings
             .Field(f => f
                    .Name("objectField")
                    .Converter<JObjectFieldConverter>());

            var transformer = new Transformer(mappings);

            var transformedObject = transformer.Transform(testData);

            var nestedObject = transformedObject["objectField"].ToObject<JObject>();

            var nestedObjectFirstField = nestedObject.Properties().FirstOrDefault();

            nestedObjectFirstField.Name.ShouldBe("generalCapability");
            nestedObjectFirstField.Value.ShouldBe("15");
        }
    }
}