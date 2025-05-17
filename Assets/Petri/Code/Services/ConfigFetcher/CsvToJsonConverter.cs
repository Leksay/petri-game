using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Petri.Services
{
    public static class CsvToJsonConverter
    {
        public static List<JObject> CsvToJObject(string csv)
        {
            var result = new List<JObject>();
            var lines = csv.Split('\n');
            if (lines.Length < 2)
                return result;
            
            var headers = lines[0].Split(',');


            for (var i = 1; i < lines.Length; i++)
            {
                var jObject = new JObject();
                var values = lines[i].Split(',');
                if (values.Length != headers.Length)
                {
                    Debug.LogWarning($"Row {i} has mismatched columns. Skipping.");
                    continue;
                }

                for (var fieldId = 0; fieldId < headers.Length; fieldId++)
                {
                    jObject.Add(headers[fieldId], values[fieldId]);
                }
                
                result.Add(jObject);
            }

            return result;
        }

        public static List<T> TryConvert<T>(string csv) where T :class
        {
            var jObjects = CsvToJObject(csv);
            var result = new List<T>();
            
            foreach (var jObject in jObjects)
            {
                var castedObject = jObject.Cast<T>();
                result.Add(castedObject as T);
            }

            return result;
        }
    }
}