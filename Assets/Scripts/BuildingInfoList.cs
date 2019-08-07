using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInfoList
{
    public BuildingInfo farm;
    public BuildingInfo woodcuttersHut;
    private ResourceList resourceList;
    public Dictionary<string, BuildingInfo> b;
    public BuildingInfoList() {
        resourceList = new ResourceList();
        b = new Dictionary<string, BuildingInfo>();
        b.Add("farm", new BuildingInfo(
            "farm", true, 3, 2,
            new Dictionary<string, Resource>{
                { "Grain", resourceList.grain },
                { "Clay", resourceList.clay } 
            },
            new Dictionary<string, int>{ 
                { "Grain", 5 }, 
                { "Clay", 95 } 
            }
        ));
        b.Add("woodcutters hut", new BuildingInfo(
            "woodcutters hut", true, 2, 2, 
            new Dictionary<string, Resource>{ 
                { "Lumber", resourceList.lumber } 
            }, 
            new Dictionary<string, int>{ 
                { "Lumber", 5  } 
            } 
        ));
    }
    
}