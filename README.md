# Json Transformer

Process and transform your JSON in a fluent way.

## JSON before transformation

```json
{
  "@id": "14739",
  "@key": "bacfe271-827a-4675-bbcf-6b8d535aec5c",
  "@parentID": "14735",
  "@creatorID": "0",
  "@createDate": "2017-06-01T07:21:47",
  "@updateDate": "2018-02-23T01:29:29",
  "@nodeName": "ACELA1428",
  "@path": "-1,1068,1961,11568,11574,14735,14739",
  "@isDoc": "",
  "@template": "0",
  "@nodeTypeAlias": "contentDescription",
  "generalCapabilities": {
    "#cdata-section": "[{\"generalCapability\":\"Personal and Social Capability\",\"element\":\"Social management\",\"subElement\":\"Communicate effectively\"},{\"generalCapability\":\"Literacy\",\"element\":\"Comprehending texts through listening, reading and viewing\",\"subElement\":\"Listen and respond to learning area texts\"},{\"generalCapability\":\"Personal and Social Capability\",\"element\":\"Social awareness\",\"subElement\":\"Understand relationships\"},{\"generalCapability\":\"Literacy\",\"element\":\"Word Knowledge\",\"subElement\":\"Understand learning area vocabulary\"},{\"generalCapability\":\"Literacy\",\"element\":\"Composing texts through speaking, writing and creating\",\"subElement\":\"Use language to interact with others\"},{\"generalCapability\":\"Literacy\",\"element\":\"Comprehending texts through listening, reading and viewing\",\"subElement\":\"Comprehend texts\"},{\"generalCapability\":\"Personal and Social Capability\",\"element\":\"Self-management\",\"subElement\":\"Express emotions appropriately\"},{\"generalCapability\":\"Intercultural Understanding\",\"element\":\"Recognising culture and developing respect\",\"subElement\":\"Investigate culture and cultural identity\"}]"
  },
  "subStrand": {
    "#cdata-section": "Language for interaction"
  },
  "scOTTerms": {
    "#cdata-section": "3033,6915"
  },
  "tags": {
    "#cdata-section": "11573,11572"
  },

  "description": {
    "#cdata-section": "Explore how language is used differently at home and school depending on the relationships between people"
  },
  "strand": {
    "#cdata-section": "Language"
  },
  "code": {
    "#cdata-section": "ACELA1428"
  }
}
```
## Usage
  ```csharp
    var mappings = new FieldMappings();
            mappings
                .ExcludeField("creatorID")
                .ExcludeField("key")
                .ExcludeField("isDoc")
                .ExcludeField("template")
                .Field(f => f
                     .Name("id")
                     .Converter<IntFieldConverter>())
                .Field(f => f
                     .Name("parentID")
                     .Converter<IntFieldConverter>())
                .Field(f => f
                     .Name("generalCapabilities")
                     .WithTransformName("capabilities")
                     .Converter<JArrayFieldConverter>())
                .Field(f => f
                    .Name("path")
                    .Converter<SplitedIntArrayFieldConverter>())
                .Field(f => f
                     .Name("tags")
                     .Converter<SplitedIntArrayFieldConverter>())
                .Field(f => f
                     .Name("scOTTerms")
                     .Converter<SplitedIntArrayFieldConverter>());

            Transformer transformer = new Transformer(mappings);
           
            var transformedObj =  transformer.Transform(testData);
  ```
## Transformed JSON

```json
{
    "id": 14739,
    "parentID": 14735,
    "createDate": "2017-06-01T07:21:47",
    "updateDate": "2018-02-23T01:29:29",
    "nodeName": "ACELA1428",
    "path": [
        -1,
        1068,
        1961,
        11568,
        11574,
        14735,
        14739
    ],
    "nodeTypeAlias": "contentDescription",
    "capabilities": [
        {
            "generalCapability": "Personal and Social Capability",
            "element": "Social management",
            "subElement": "Communicate effectively"
        },
        {
            "generalCapability": "Literacy",
            "element": "Comprehending texts through listening, reading and viewing",
            "subElement": "Listen and respond to learning area texts"
        },
        {
            "generalCapability": "Personal and Social Capability",
            "element": "Social awareness",
            "subElement": "Understand relationships"
        },
        {
            "generalCapability": "Literacy",
            "element": "Word Knowledge",
            "subElement": "Understand learning area vocabulary"
        },
        {
            "generalCapability": "Literacy",
            "element": "Composing texts through speaking, writing and creating",
            "subElement": "Use language to interact with others"
        },
        {
            "generalCapability": "Literacy",
            "element": "Comprehending texts through listening, reading and viewing",
            "subElement": "Comprehend texts"
        },
        {
            "generalCapability": "Personal and Social Capability",
            "element": "Self-management",
            "subElement": "Express emotions appropriately"
        },
        {
            "generalCapability": "Intercultural Understanding",
            "element": "Recognising culture and developing respect",
            "subElement": "Investigate culture and cultural identity"
        }
    ],
    "subStrand": "Language for interaction",
    "scOTTerms": [
        3033,
        6915
    ],
    "tags": [
        11573,
        11572
    ],
    "description": "Explore how language is used differently at home and school depending on the relationships between people",
    "strand": "Language",
    "code": "ACELA1428"
}
```

