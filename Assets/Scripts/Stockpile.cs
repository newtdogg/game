using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stockpile
{
    public Dictionary<string, Dictionary<string, float>> stats;
    public Dictionary<string, float> resource;
    public int decayrate;
    public int foodSupplyThreshold;
    public float foodSurplus;

    public Stockpile(float lumberAmount = 400, float foodAmount = 400){
        foodSurplus = 96;
        foodSupplyThreshold = 200;
        stats = new Dictionary<string,  Dictionary<string, float>>();
        var foodDic = new Dictionary<string, float>{
            { "amount", foodAmount },
            { "max", 1000 }
        };
        var lumberDic = new Dictionary<string, float>{
            { "amount", lumberAmount },
            { "max", 1000 }
        };
        stats.Add("food", foodDic);
        stats.Add("lumber", lumberDic);
        decayrate = 4;
    }

    public void updateStockpile(float population){
        stats["food"]["amount"] -= updateFood(population);
        removeSurplus();
    }

    public float updateFood(float population){
        return (Time.deltaTime/decayrate) * population;
    }

    public float foodSupplyPercentage(){
        return (stats["food"]["amount"]/stats["food"]["max"]) * 100;
    }

    public void surplusRate(){
        if(stats["food"]["amount"] > foodSupplyThreshold){
            var diff = stats["food"]["amount"] - foodSupplyThreshold;
            foodSurplus += diff * 0.001f;
        }
    }

    private void removeSurplus(){
        foreach(KeyValuePair<string, Dictionary<string, float>> resource in stats){
            var resourceDic = resource.Value;
            if(resourceDic["amount"] > resourceDic["max"]){
                resourceDic["amount"] = resourceDic["max"];
            }
        }
    }
}
