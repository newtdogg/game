using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource
{
    public string type;
    public float value;
    public float maxCapacity;
    public float crateValue;
    public string group;
    public float decayRate;

    public Resource(string bType, float v, float capacity, float cValue, string grp, float dr) {
        type = bType;
        value = v;
        maxCapacity = capacity;
        crateValue = cValue;
        group = grp;
        decayRate = dr;
    }
    
}
