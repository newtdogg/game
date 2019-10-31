using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryImproverBuildingList : BList
{
    private ResourceList resourceList;
    public override Dictionary<string, Building> buildings { get; set; }
    public PrimaryImproverBuildingList() {
        resourceList = new ResourceList();
        buildings = new Dictionary<string, Building>();
        var CraftingStation = new ImproverBuilding(
            "Crafting Station", "Primary Improver", "Secondary Resource", "Improves resources and shit", true, 3, 2,
            new Dictionary<string, int>{ 
                { "Lumber", 100 },
                // { "Stone", 30 },
                { "Iron", 20 }
            }
        );
        CraftingStation.statBonusOrdering = "srdfa";
        CraftingStation.bonusStat = "strength";
        CraftingStation.startingResources = new Dictionary<string, Resource>{
            { "Iron", resourceList.Iron }
        };
        CraftingStation.spawnChance = new Dictionary<string, int>{ 
                { "Iron", 60 }
        };
        CraftingStation.resourcesToImprove = new Dictionary<string, string[]>{
            { "Iron", new string[] { "Lumber", "Grain" } }
        };
        CraftingStation.resourcesToImproveCosts = new Dictionary<string, int>{ 
            { "Lumber", 50 }, 
            { "Grain", 10 }
        };
        CraftingStation.upgradesTo = new List<string>(){};
        buildings.Add("Crafting Station", CraftingStation);
    }
    
}