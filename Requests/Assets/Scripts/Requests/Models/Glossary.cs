using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glossary
{
    public const float RequestTime = 1f;

    public const string Object = "object";
    public const string Objects = "objects";
    public const string Id = "id";
    public const string Name = "name";
    public const string Value = "value";
    public const string StatusCode = "statusCode";
    public const string Property = "property";
    public const string TransformationMin = "transformation.min";
    public const string TransformationMax = "transformation.max";


    // Запросы к Серверу
    public const string ServerConnectionString = "https://server20190527123354.azurewebsites.net/api/";
    public const string SimpleGetObjects = ServerConnectionString + "objects";
    public const string CompactGetObjects = ServerConnectionString + "objects/compact";
    public const string GetObjectsStates = ServerConnectionString + "objects/states";
    public const string AddObject = ServerConnectionString + "objects/addObject";
    public const string AddBinding = ServerConnectionString + "bindings/addBinding";
    public const string ModifyBinding = ServerConnectionString + "bindings/modifyBinding";
    public const string RemoveBinding = ServerConnectionString + "bindings/removeBinding";
    public const string RemoveObject = ServerConnectionString + "objects/removeObject";
}
