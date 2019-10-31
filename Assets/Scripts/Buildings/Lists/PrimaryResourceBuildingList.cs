using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryResourceBuildingList : BList
{
    private SecondaryResourceBuildingList srbl;
    private ResourceList resourceList;
    public override Dictionary<string, Building> buildings { get; set; }
    public PrimaryResourceBuildingList() {
        srbl = new SecondaryResourceBuildingList();
        resourceList = new ResourceList();
        buildings = new Dictionary<string, Building>();

        // buildingName | upgradeType | type | description | unlocked | width | height |
        // cost
        // statBonusOrdering
        // bonusStat
        // startingResources
        // spawnChance
        // upgradesTo

        var Croft = new ResourceBuilding(
            "Croft", "Primary Resource", "Secondary Resource", "Produces grain", true, 3, 2,
            new Dictionary<string, int>{ 
                { "Lumber", 120 },
            }
        );
        Croft.statBonusOrdering = "srdfa";
        Croft.bonusStat = "strength";
        Croft.startingResources = new Dictionary<string, Resource>{
            { "Grain", resourceList.Grain },
            { "Clay", resourceList.Clay }
        };
        Croft.spawnChance = new Dictionary<string, int>{
            { "Grain", 30 }, 
            { "Clay", 60 }
        };
        Croft.upgradesTo = new List<string>(){
             "Arable Farm"
        };


        var WoodcuttersHut =  new ResourceBuilding(
            "Woodcutter's Hut", "Primary Resource", "Secondary Resource", "Produces wood", true, 2, 2, 
            new Dictionary<string, int>{ 
                { "Lumber", 100 },
                { "Iron", 20 }
            }
        );
        WoodcuttersHut.statBonusOrdering = "sfdra";
        WoodcuttersHut.bonusStat = "fortitude";
        WoodcuttersHut.startingResources = new Dictionary<string, Resource>{ 
                { "Lumber", resourceList.Lumber } 
        };
        WoodcuttersHut.spawnChance = new Dictionary<string, int>{ 
                { "Lumber", 20  } 
        };
        WoodcuttersHut.upgradesTo = new List<string>(){};

        var StonecuttersHut =  new ResourceBuilding(
            "Stonecutter's Hut", "Primary Resource", "Secondary Resource", "Produces stone", true, 2, 2, 
            new Dictionary<string, int>{ 
                { "Lumber", 100 },
                { "Iron", 30 }
            }
        );
        StonecuttersHut.statBonusOrdering = "sfdra";
        StonecuttersHut.bonusStat = "fortitude";
        StonecuttersHut.startingResources = new Dictionary<string, Resource>{ 
                { "Stone", resourceList.Stone },
                { "Sand", resourceList.Sand },
                { "Clay", resourceList.Clay }
        };
        StonecuttersHut.spawnChance = new Dictionary<string, int>{ 
                { "Stone", 80 },
                { "Sand", 15 },
                { "Clay", 5 }
        };
        StonecuttersHut.upgradesTo = new List<string>(){};

        var Mine =  new ResourceBuilding(
            "Mine", "Primary Resource", "Secondary Resource", "Produces wood", true, 2, 2, 
            new Dictionary<string, int>{ 
                { "Lumber", 200 },
                { "Iron", 40 }  
            }
        );
        Mine.statBonusOrdering = "sfdra";
        Mine.bonusStat = "fortitude";
        Mine.startingResources = new Dictionary<string, Resource>{ 
                { "Iron Ore", resourceList.IronOre },
                { "Copper Ore", resourceList.CopperOre },
                { "Coal", resourceList.Coal },
                { "Stone", resourceList.Stone },
                { "Sand", resourceList.Sand }
        };
        Mine.spawnChance = new Dictionary<string, int>{ 
                { "Iron Ore", 30 },
                { "Copper Ore", 20 },
                { "Coal", 10 },
                { "Stone", 20 },
                { "Sand", 20 }
        };
        Mine.upgradesTo = new List<string>(){};

        buildings.Add("Croft", Croft);
        buildings.Add("Woodcutter's Hut", WoodcuttersHut);
        buildings.Add("Stonecutter's Hut", StonecuttersHut);
        buildings.Add("Mine", Mine);
    }   
}