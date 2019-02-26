using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldClock
{
    public float day = 0;
    public float second = 0;
    public float minute = 0;
    public float hour = 22;

    public int timescale = 300;

    public bool calculateTime(){
        second += Time.deltaTime * timescale;
        if (second >= 60) {
            minute++;
            second = 0;
            return true;
        } else if (minute >= 60) {
            hour++;
            minute = 0;
            return true;
        } else if (hour >= 24) {
            day++;
            hour = 0;
            return true;
        }
        return false;
    }

    public void nextDay(){
        day++;
        hour = 9;
        minute = 0;
        second = 0;
    }
}
