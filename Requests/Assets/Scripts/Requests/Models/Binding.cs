using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Binding
{
    public Binding(string name, long objectId, string propertyName, int minValue, int maxValue)
    {
        Name = name;
        Object = objectId;
        Property = propertyName;
        Transformation = new Transformation(minValue, maxValue);
    }

    public string Name { get; set; }

    /// <summary>
    /// Id связанного объекта
    /// </summary>
    public long Object { get; set; }
    public string Property { get; set; }
    public Transformation Transformation { get; set; }
}
