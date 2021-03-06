﻿namespace JsonTransformer.FieldValueConverters
{
    using Newtonsoft.Json.Linq;
    using System;

    public class SplitedIntArrayFieldConverter : IFieldValueConverter
    {
        public JToken Convert(JToken value)
        {
            int[] arr = Array.ConvertAll(value.ToString().Split(','), s => int.Parse(s));
            return JArray.FromObject(arr);
        }
    }
}