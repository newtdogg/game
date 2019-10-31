using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingList
{
    public Dictionary<string, BList> b;
    
    public BuildingList() {
        b = new Dictionary<string, BList> {
            { "Primary Resource", new PrimaryResourceBuildingList() },
            { "Secondary Resource", new SecondaryResourceBuildingList() },
            { "Utility", new PrimaryUtilityBuildingList() },
            { "Improver", new PrimaryImproverBuildingList() }
        };
    }
    
}