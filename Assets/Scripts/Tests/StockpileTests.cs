using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class StockpileTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void StockpileInstatiate()
        {
            var stockpile = new Stockpile();
            Assert.AreEqual(stockpile.foodSurplus, 99.9f);
            Assert.AreEqual(stockpile.foodSupplyThreshold, 200);
            Assert.IsTrue(stockpile.stats.ContainsKey("Grain"));
            Assert.IsTrue(stockpile.stats.ContainsKey("Lumber"));
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        // [UnityTest]
        // public IEnumerator StockpileFoodDecay()
        // {
        //     var stockpile = new Stockpile();
        //     stockpile.totalFood = 100f;
        //     stockpile.updateFood(3);
        //     stockpile.surplusRate();
        //     yield return new WaitForSeconds(0.1f);
        //     Debug.Log(stockpile.totalFood);
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            // yield return null;
        // }
    }
}
