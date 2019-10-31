using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building
{
    public virtual string name { get; set; }
    public virtual bool unlocked { get; set; }
    public virtual Dictionary<string, int> cost { get; set; }
    public virtual List<string> upgradesTo { get; set; }
    public virtual string description { get; set; }
    public virtual string statBonusOrdering { get; set; }
    public virtual string bonusStat { get; set; }
    public virtual int width { get; set; }
    public virtual int height { get; set; }
    public virtual string type { get; set; }
    public virtual string upgradeType { get; set; }
    public virtual Dictionary<string, string[]> resourcesToImprove { get; set; }
    public virtual Dictionary<string, int> resourcesToImproveCosts { get; set; }

    public virtual Dictionary<string, Resource> startingResources { get; set; }
    public virtual Dictionary<string, int> spawnChance { get; set; }

    public Building(string buildingName, string t, string upgradeT, string desc, bool isUnlocked, int w, int h, Dictionary<string, int> cst){
        name = buildingName;
        type = t;
        upgradeType = upgradeT;
        description = desc;
        unlocked = isUnlocked;
        width = w;
        height = h;
        cost = cst;
    }

    public virtual void build(GameManager gameManager, Building buildingInfo){}
}