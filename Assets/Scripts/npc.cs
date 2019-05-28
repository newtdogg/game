using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC
{

    public string name;
    public Dictionary<string, int> stats;

    public NPC(string npcName){
        stats = new Dictionary<string, int>{
            { "amount", 0 },
            { "max", 0 }
        };
        name = npcName;
    }

  
}
