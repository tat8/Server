using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataObject
{
    public DataObject(string name)
    {
        Name = name;
    }

    public long Id { get; set; }
    public string Name { get; set; }
    public Dictionary<string, int> Properties { get; set; }
}
