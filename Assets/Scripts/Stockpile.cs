using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stockpile
{
    public Dictionary<string, Resource> stats;
    public Dictionary<string, float> resource;
    public int decayrate;
    public float totalFood;
    public int foodSupplyThreshold;
    public float foodSurplus;
    private ResourceList resourceList;
    public float foodCapacity;

    public Stockpile(){
        foodSurplus = 99.9f;
        foodSupplyThreshold = 200;
        totalFood = 0f;
        foodCapacity = 0f;
        stats = new Dictionary<string,  Resource>();
        resourceList = new ResourceList();
        stats.Add("grain", resourceList.grain);
        stats.Add("lumber", resourceList.lumber);
    }

    public void updateStockpile(float population){
        totalFood = updateFood(population);
        surplusRate();
        removeSurplus();
    }

    public float updateFood(float population){
        var allFood = 0f;
        var foodCount = 0f;
        var totalCapacity = 0f;
        foreach(KeyValuePair<string, Resource> resource in stats){
            var resourceDic = resource.Value;
            if(resourceDic.group == "Food") {
                // Debug.Log(resourceDic.value);
                resourceDic.value -= (Time.deltaTime/resourceDic.decayRate) * population;
                foodCount += 1;
                allFood += resourceDic.value;
                totalCapacity += resourceDic.maxCapacity;
            }
        }
        foodCapacity = totalCapacity/foodCount;
        return allFood/foodCount;
    }

    public float foodSupplyPercentage(){
        return (totalFood/foodCapacity) * 100;
    }

    public void surplusRate(){
        if(totalFood > foodSupplyThreshold){
            var diff = totalFood - foodSupplyThreshold;
            foodSurplus += diff * 0.1f;
        }
    }

    private void removeSurplus(){
        foreach(KeyValuePair<string, Resource> resource in stats){
            var resourceDic = resource.Value;
            if(resourceDic.value > resourceDic.maxCapacity){
                resourceDic.value = resourceDic.maxCapacity;
            }
        }
    }

    public void updateStat(string stat, float amount) {
        stats[stat].value += amount;
    }
}
