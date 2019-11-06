using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceList
{
    public Resource Clay;
    public Resource Grain;
    public Resource Lumber;
    public Resource Stone;
    public Resource Sand;
    public Resource IronOre;
    public Resource CopperOre;
    public Resource Iron;
    public Resource Copper;
    public Resource Coal;
   
    public ResourceList() {
        // type | value | maxCapacity | crate value | group | decayRate
        Clay = new Resource("Clay", 10f, 100f, 10f, "structure", 0f);
        Grain = new Resource ("Grain", 100f, 200f, 2000f, "food", 10f);
        Lumber = new Resource ("Lumber", 220f, 600f, 10f, "structure", 0f);
        Stone = new Resource ("Stone", 50f, 200f, 10f, "structure", 0f);
        Sand = new Resource ("Sand", 50f, 200f, 10f, "structure", 0f);
        IronOre = new Resource ("Iron Ore", 50f, 200f, 10f, "structure", 0f);
        CopperOre = new Resource ("Copper Ore", 50f, 200f, 10f, "structure", 0f);
        Coal = new Resource ("Copper Ore", 50f, 200f, 10f, "structure", 0f);
        Iron = new Resource ("Iron", 100f, 200f, 10f, "structure", 0f);
        Copper = new Resource ("Copper", 50f, 200f, 10f, "structure", 0f);
    }
    
}