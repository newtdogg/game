using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryUtilityBuildingList : BList
{
    private ResourceList resourceList;
    public override Dictionary<string, Building> buildings { get; set; }
    public PrimaryUtilityBuildingList() {
        resourceList = new ResourceList();
        buildings = new Dictionary<string, Building>();
        buildings.Add("House", new UtilityBuilding(
            "House", "Utility", "Secondary Utility", "Place to sleep", true, 3, 2, 3,
            new Dictionary<string, int>{ 
                { "Lumber", 50 }  
            }
        ));
        buildings.Add("Market Stall", new UtilityBuilding(
            "Market Stall", "Utility", "Secondary Utility", "Place to sleep", true, 3, 2, 3,
            new Dictionary<string, int>{ 
                { "Lumber", 50 }  
            }
        ));
        buildings.Add("Well", new UtilityBuilding(
            "Well", "Utility", "Secondary Utility", "Place to sleep", true, 3, 2, 3,
            new Dictionary<string, int>{ 
                { "Lumber", 50 }  
            }
        ));
        buildings.Add("Inn", new UtilityBuilding(
            "Inn", "Utility", "Secondary Utility", "Place to sleep", true, 3, 2, 3,
            new Dictionary<string, int>{ 
                { "Lumber", 50 }  
            }
        ));
    }
    
}