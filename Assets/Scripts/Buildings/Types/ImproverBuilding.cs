using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImproverBuilding : Building
{
    public override string name { get; set; }
    public override bool unlocked { get; set; }
    public override Dictionary<string, int> cost { get; set; }
    public override string description { get; set; }
    public override List<string> upgradesTo { get; set; }
    public override int width { get; set; }
    public override int height { get; set; }
    public override string statBonusOrdering { get; set; }
    public override string bonusStat { get; set; }
    public override string type { get; set; }
    public override string upgradeType { get; set; }

    public override Dictionary<string, string[]> resourcesToImprove { get; set; }
    public override Dictionary<string, int> resourcesToImproveCosts { get; set; }

    public ImproverBuilding(string buildingName, string t, string upgradeT, string desc, bool isUnlocked, int w, int h, Dictionary<string, int> cst) : base(buildingName, t, upgradeT, desc, isUnlocked, w, h, cst)
    {
        name = buildingName;
        upgradeType = upgradeT;
        type = t;
        description = desc;
        unlocked = isUnlocked;
        width = w;
        height = h;
        cost = cst;
    }

    public override void build(GameManager gameManager, Building buildingInfo) {
        foreach(KeyValuePair<string, Resource> resource in buildingInfo.startingResources) {
            if(!gameManager.stockpile.stats.ContainsKey(resource.Key)) {
                gameManager.stockpile.stats.Add(resource.Key, resource.Value);
            }
        }
    }
}
