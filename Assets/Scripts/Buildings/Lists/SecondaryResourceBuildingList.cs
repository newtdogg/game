using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryResourceBuildingList : BList
{
    private ResourceList resourceList;
    public override Dictionary<string, Building> buildings { get; set; }
    public SecondaryResourceBuildingList() {
        resourceList = new ResourceList();
        buildings = new Dictionary<string, Building>();
        var ArableFarm = new ResourceBuilding(
            "Arable Farm", "Secondary Resource", "Tertiary Resource", "Produces grain", true, 3, 2,
            new Dictionary<string, int>{ 
                { "Lumber", 50 },
                { "Grain", 30 }
            }
        );
        ArableFarm.statBonusOrdering = "srdfa";
        ArableFarm.bonusStat = "strength";
        ArableFarm.startingResources = new Dictionary<string, Resource>{
                { "Grain", resourceList.Grain },
                { "Clay", resourceList.Clay }
        };
        ArableFarm.spawnChance = new Dictionary<string, int>{ 
                { "Grain", 5 }, 
                { "Clay", 95 }  
        };
        buildings.Add("Arable Farm", ArableFarm);
    }
    
}