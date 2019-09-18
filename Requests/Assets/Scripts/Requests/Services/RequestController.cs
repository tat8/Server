using Assets.Requests;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;

public class RequestController
{
    private Converter _converter;
    private List<DataObject> _dataObjectsGlobal;

    public RequestController()
    {
        _converter = new Converter();
        _dataObjectsGlobal = new List<DataObject>();
    }

    /// <summary>
    /// Возвращает текущее состояние всех объектов со всеми параметрами
    /// </summary>
    /// <returns> список объектов DataObject </returns>
    public IEnumerable<DataObject> Get()
    {
        var www = UnityWebRequest.Get(Glossary.SimpleGetObjects);

        var message = SendRequest(www);
        var answer = message[Glossary.Value].Value<JObject>();

        var dataObjects = new List<DataObject>();
        var objects = answer[Glossary.Objects].Values<JObject>().ToList();

        foreach (var jObject in objects)
        {
            dataObjects.Add(_converter.JObjectToDataObject(jObject));
        }

        _dataObjectsGlobal = dataObjects;
        return _dataObjectsGlobal;
    }

    /// <summary>
    /// Получает текущее состояние объектов только тех параметров, которые изменились с предыдущего запроса,
    /// или все параметры, если это первый запрос;
    /// затем проверяет, какие параметры изменились, а какие нет,
    /// после чего возвращает текущее состояние всех параметров
    /// </summary>
    /// <returns> текущее состояние всех параметров </returns>
    public IEnumerable<DataObject> CompactGet()
    {
        var www = UnityWebRequest.Get(Glossary.CompactGetObjects);

        var message = SendRequest(www);
        var answer = message[Glossary.Value].Value<JObject>();
        
        // Получение измененных параметров
        var dataObjects = new List<DataObject>();
        var objects = answer[Glossary.Objects].Values<JObject>().ToList();

        foreach (var jObject in objects)
        {
            dataObjects.Add(_converter.JObjectToDataObject(jObject));
        }
        
        return FindDifference(dataObjects);
    }
    
    /// <summary>
    /// Возвращает набор состояний всех объектов со всеми параметрами за последние 5 минут (или меньше, если еще не накопилось)
    /// с указанием времени для каждого состояния
    /// </summary>
    /// <returns></returns>
    public Dictionary<DateTime, IEnumerable<DataObject>> GetStates()
    {
        var www = UnityWebRequest.Get(Glossary.GetObjectsStates);

        var message = SendRequest(www);
        var answer = message[Glossary.Value].Values<JObject>();

        var states = _converter.JObjectToDataObjectStates(answer);

        return states;
    }
    
    /// <summary>
    /// Добавляет объект данных (DataObject)
    /// </summary>
    /// <param name="dataObject"> Объект для добавления </param>
    public DataObject AddObject(DataObject dataObject)
    {
        var formData = new List<IMultipartFormSection>
        {
            new MultipartFormDataSection(Glossary.Name, dataObject.Name)
        };

        UnityWebRequest www = UnityWebRequest.Post(Glossary.AddObject, formData);

        var message = SendRequest(www);
        var answer = message[Glossary.Value].Value<JObject>();

        var dataObjectResult = _converter.JObjectToDataObject(answer);

        return dataObjectResult;
    }

    /// <summary>
    /// Добавляет привязку (Binding) к существующему объекту
    /// </summary>
    /// <param name="binding"> привязка для добавления </param>
    public void AddBinding(Binding binding)
    {
        var formData = _converter.BindingToFormData(binding);

        UnityWebRequest www = UnityWebRequest.Post(Glossary.AddBinding, formData);
        SendRequest(www);
    }

    public void ModifyBinding(Binding binding)
    {
        var formData = _converter.BindingToFormData(binding);

        UnityWebRequest www = UnityWebRequest.Post(Glossary.ModifyBinding, formData);
        SendRequest(www);
    }

    public void RemoveBinding(Binding binding)
    {
        var formData = _converter.BindingToFormData(binding);

        UnityWebRequest www = UnityWebRequest.Post(Glossary.RemoveBinding, formData);
        SendRequest(www);
    }

    public void RemoveObject(DataObject dataObject)
    {
        var formData = new List<IMultipartFormSection>
        {
            new MultipartFormDataSection(Glossary.Id, Convert.ToString(dataObject.Id)),
            new MultipartFormDataSection(Glossary.Name, dataObject.Name)
        };

        UnityWebRequest www = UnityWebRequest.Post(Glossary.RemoveObject, formData);

        SendRequest(www);
    }

    private JObject SendRequest(UnityWebRequest www)
    {
        www.certificateHandler = new CustomCertificateHandler();
        var wwwOperation = www.SendWebRequest();
        while (!wwwOperation.isDone)
        { }

        return CheckAnswer(www);
    }

    private JObject CheckAnswer(UnityWebRequest www)
    {
        if (www.isNetworkError || www.isHttpError)
        {
            throw new Exception(www.error);
        }

        var message = www.downloadHandler.text;
        var messageJson = JsonConvert.DeserializeObject(message) as JObject;

        if (messageJson == null)
        {
            throw new Exception("Null message received");
        }

        if (messageJson[Glossary.StatusCode].Value<int>() != 200)
        {
            throw new Exception(messageJson[Glossary.Value]?.Value<string>());
        }

        return messageJson;
    }

    /// <summary>
    /// Находит изменившиеся параметры и изменяет в переменной _dataObjectsGlobal их значения
    /// </summary>
    /// <param name="dataObjectsCompact"> объекты с изменившимися параметрами </param>
    /// <returns> обновленный _dataObjectsGlobal </returns>
    private IEnumerable<DataObject> FindDifference(IEnumerable<DataObject> dataObjectsCompact)
    {
        // Находим изменившиеся параметры и изменяем в переменной _dataObjects их значения
        foreach (var compactDataObject in dataObjectsCompact)
        {
            var dataObjectFound = false;
            foreach (var globalDataObject in _dataObjectsGlobal)
            {
                if (compactDataObject.Id == globalDataObject.Id)
                {
                    foreach (var property in compactDataObject.Properties)
                    {
                        globalDataObject.Properties[property.Key] = property.Value;
                    }
                    dataObjectFound = true;
                }
            }

            if (!dataObjectFound)
            {
                _dataObjectsGlobal.Add(compactDataObject);
            }
        }

        return _dataObjectsGlobal;
    }

}
