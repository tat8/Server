using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformation
{
    public Transformation(int min, int max)
    {
        Max = max;
        Min = min;
    }

    public int Max { get; set; }
    public int Min { get; set; }
}
