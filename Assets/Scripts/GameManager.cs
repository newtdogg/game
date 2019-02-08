using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;
    public WorldClock clock;
    public Text dayText;
    public Text timeText;
    
    // Start is called before the first frame update
    void Awake() {
        if(manager == null){
            DontDestroyOnLoad(gameObject);
            manager = this;
        }
        else if (manager != this){
            Destroy(gameObject);
        }
        if(clock == null){
            clock = new WorldClock();
        }
        dayText = gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
        timeText = gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<Text>();
        
    }

    void Update()
    {
        if(clock.calculateTime() == true){
            writeDateTime();
        }
    }

    private void writeDateTime(){
        dayText.text = $"Day: {clock.day}";
        if(clock.minute < 10){
            timeText.text = $"Time: {clock.hour}:0{clock.minute}";
        } else {
            timeText.text = $"Time: {clock.hour}:{clock.minute}";
        }
        
    }
}
