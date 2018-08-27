namespace JsonTransformer.Tests
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Shouldly;
    using System.IO;
    using System.Linq;
    using Xunit;

    public class TransformTests
    {
        JObject testData;

        public TransformTests()
        {
            testData = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(@"Data\data.json"));
        }

        [Fact]
        public void AllPropertiesShouldBeConvertedIfThereIsNoExcludedProperties()
        {
            Transformer transformer = new Transformer();

            var transformedObject = transformer.Transform(testData);
            var actual = transformedObject.Properties().Count();
            var expected = testData.Properties().Count();

            actual.ShouldBe(expected);
        }

        [Fact]
        public void AllPrefixesShouldBeCleanedFromProperties()
        {
            var countBeforeTransform = testData.Properties().Count(p => p.Name.StartsWith("@"));

            var transformer = new Transformer();
            var transformedObject = transformer.Transform(testData);

            var countAfterTransform = transformedObject.Properties().Count(p => p.Name.StartsWith("@"));

            countBeforeTransform.ShouldBeGreaterThan(0);
            countAfterTransform.ShouldBe(0);
        }

        [Fact]
        public void AllCDataFieldsMustBeTransFormed()
        {
            var transformer = new Transformer();

            var transformedObject = transformer.Transform(testData);

            var isThereAnyCDataField = transformedObject.Properties()
                .Any(p => p.Value.HasValues && p.Value.First.ToObject<JProperty>().Name == "#cdata-section");


            isThereAnyCDataField.ShouldBeFalse();
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

            var isFieldsWithTransformNameExist = transformedObject.Properties()
                                                                  .Any(p => p.Name == "alias");

            var isTransformedFieldRemoved = !transformedObject.Properties()
                                                             .Any(p => p.Name == "nodeTypeAlias");

            isFieldsWithTransformNameExist.ShouldBeTrue();
            isTransformedFieldRemoved.ShouldBeTrue();
        }
    }
}
