using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceList
{
    public Resource clay;
    public Resource grain;
    public Resource lumber;
   
    public ResourceList() {
        clay = new Resource("Clay", 0f, 100f, 50f, "Structure", 0f);
        grain = new Resource ("Grain", 100f, 200f, 50f, "Food", 10f);
        lumber = new Resource ("Lumber", 50f, 200f, 50f, "Structure", 0f);
    }
    
}