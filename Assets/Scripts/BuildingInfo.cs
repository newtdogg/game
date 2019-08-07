using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInfo
{
    public string name;
    public bool unlocked;
    public Dictionary<string, int> cost;
    public Dictionary<string, Resource> startingResources;
    public Dictionary<string, int> spawnChance;
    public string description;
    public int width;
    public int height;

    public BuildingInfo(string buidlingName, bool isUnlocked, int w, int h,  Dictionary<string, Resource> resources, Dictionary<string, int> chance){
        name = buidlingName;
        unlocked = isUnlocked;
        width = w;
        height = h;
        startingResources = resources;
        spawnChance = chance;
    }

}
