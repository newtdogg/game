using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class NPC
{

    public string name;
    public Dictionary<string, int> stats;
    public string employment;
    public string statOrder;
    public System.Random ran = new System.Random();

    public NPC(string npcName){
        
        int st = ran.Next(1, 5);
        int ac = ran.Next(1, 5);
        int rp = ran.Next(1, 5);
        int dx = ran.Next(1, 5);
        int ft = ran.Next(1, 5);
        stats = new Dictionary<string, int>{
            { "strength", st },
            { "acuity", ac },
            { "rapidity", rp },
            { "dexterity", dx },
            { "fortitude", ft },
        };
        statOrder = calculateStatOrder();
        name = npcName;
        employment = "idle";
    }

    private string calculateStatOrder() {
        var numList = stats.Values.ToList();
        numList.Sort();
        numList.Reverse();
        var count = 0;
        var str = "";
        var usedStats = new List<string>();
        foreach(var num in numList) {
            foreach(KeyValuePair<string, int> stat in stats){
                if(num == stat.Value && !usedStats.Contains(stat.Key)){
                    str += stat.Key[0];
                    usedStats.Add(stat.Key);
                    break;
                }   
            }
        }
        return str;
    }

  
}
