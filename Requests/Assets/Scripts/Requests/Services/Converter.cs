using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Converter
{

    public DataObject JObjectToDataObject(JObject jObject)
    {
        var properties = new Dictionary<string, int>();

        foreach (var jToken in jObject.Children())
        {
            var jProperty = (JProperty) jToken;
            if (jProperty.Name == Glossary.Id || jProperty.Name == Glossary.Name)
                continue;
            
            properties.Add(jProperty.Name, jProperty.Value.Value<int>());
        }

        var dataObject = new DataObject(jObject[Glossary.Name].Value<string>())
        {
            Id = jObject[Glossary.Id].Value<long>(),
            Properties = properties
        };

        return dataObject;
    }

    public Dictionary<DateTime, IEnumerable<DataObject>> JObjectToDataObjectStates(IEnumerable<JObject> jObjects)
    {
        var states = new Dictionary<DateTime, IEnumerable<DataObject>>();

        foreach (var jObject in jObjects)
        {
            var currentStateTime = jObject["utcTime"].Value<DateTime>().ToLocalTime();

            var dataObjectsList = new List<DataObject>();
            var jDataObjectsList = jObject["state"]["objects"].Values<JObject>().ToList();
            foreach (var jDataObject in jDataObjectsList)
            {
                var dataObject = JObjectToDataObject(jDataObject);
                dataObjectsList.Add(dataObject);
            }

            states.Add(currentStateTime, dataObjectsList);
        }

        return states;
    }

    public List<IMultipartFormSection> BindingToFormData(Binding binding)
    {
        return new List<IMultipartFormSection>
        {
            new MultipartFormDataSection(Glossary.Name, binding.Name),
            new MultipartFormDataSection(Glossary.Object, Convert.ToString(binding.Object)),
            new MultipartFormDataSection(Glossary.Property, binding.Property),
            new MultipartFormDataSection(Glossary.TransformationMin, Convert.ToString(binding.Transformation.Min)),
            new MultipartFormDataSection(Glossary.TransformationMax, Convert.ToString(binding.Transformation.Max))
        };
    }

}
