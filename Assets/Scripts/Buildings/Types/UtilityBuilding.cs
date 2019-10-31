using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityBuilding : Building
{
    public override string name { get; set; }
    public override string type { get; set; }
    public override bool unlocked { get; set; }
    public override Dictionary<string, int> cost { get; set; }
    public override string description { get; set; }
    public int housingValue { get; set; }
    public override int width { get; set; }
    public override int height { get; set; }
    public override string upgradeType { get; set; }

    public UtilityBuilding(string buildingName, string t, string upgradeT, string desc, bool isUnlocked, int w, int h, int houses, Dictionary<string, int> cst) : base(buildingName, t, upgradeT, desc, isUnlocked, w, h, cst){
        name = buildingName;
        type = t;
        upgradeType = upgradeT;
        description = desc;
        unlocked = isUnlocked;
        width = w;
        height = h;
        cost = cst;
        housingValue = houses;
    }

    public override void build(GameManager gameManager, Building buildingInfo) {
        gameManager.populationMax += housingValue;
    }

}
