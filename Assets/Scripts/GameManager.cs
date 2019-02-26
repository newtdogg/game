using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;
    public WorldClock clock;
    public Text dayText;
    public Text questText;

    public QuestManager questManager;
    private GameObject questCanvas;
    public Text timeText;
    private GameObject theSun;
    private Light sunLight;
    public Stockpile stockpile;
    public Dictionary<Vector3Int, string> tiles;
    
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
        populateTiles();
        questManager = new QuestManager();
        questManager.populateQuests();
        questText = GameObject.Find("QuestText").GetComponent<Text>();
        questManager.StartQuest1(questText);
        questCanvas = GameObject.Find("QuestCanvas");
        questCanvas.transform.GetChild(0).gameObject.SetActive(false);
        dayText = gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
        timeText = gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<Text>();
        theSun = GameObject.Find("Sun");
        sunLight = theSun.GetComponent<Light>();
        
    }

    void Update()
    {
        questManager.questsCompleteCheck(questText);
        toggleCanvas();
        if(clock.calculateTime() == true){
            writeDateTime();
        }
        sunPosition();
    }


    private void toggleCanvas(){
        if(Input.GetKeyDown(KeyCode.Tab)){
            Debug.Log("here");
            if(questCanvas.transform.GetChild(0).gameObject.active == true){
                questCanvas.transform.GetChild(0).gameObject.SetActive(false);
            } else {
                questCanvas.transform.GetChild(0).gameObject.SetActive(true);
            }
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

    public void sunPosition(){
        float minutes = (60 * clock.hour) + clock.minute;
        float xConst = 0.1333f;
        float xPosition = 112 - (xConst * minutes);
        float rotConst = 0.1666f;
        float rotValue = (rotConst * minutes) - 140;
        theSun.transform.rotation = Quaternion.Euler(0, rotValue, 0);
        theSun.transform.localPosition = new Vector3(xPosition, 0, -40);
        // if(minutes < 390){
        //     sunLight.intensity = 32;
        // } else if(minutes > 390 && minutes < 840){
        //     sunLight.intensity -= 0.04f;
        // } else if(minutes > 840 && minutes < 1140){
        //     sunLight.intensity += 0.04f;
        // }
    }

    void OnEnable(){
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable(){
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode){
        questCanvas = GameObject.Find("QuestCanvas");
    }

    void populateTiles(){
        tiles = new Dictionary<Vector3Int, string>();
        tiles.Add(new Vector3Int(-12, -9, 0), "stockpile");
        tiles.Add(new Vector3Int(-12, -8, 0), "stockpile");
        tiles.Add(new Vector3Int(-11, -9, 0), "stockpile");
        tiles.Add(new Vector3Int(-11, -8, 0), "stockpile");
        tiles.Add(new Vector3Int(-11, -7, 0), "stockpile");
        tiles.Add(new Vector3Int(-10, -7, 0), "stockpile");
        tiles.Add(new Vector3Int(-10, -8, 0), "stockpile");
        tiles.Add(new Vector3Int(-10, -9, 0), "stockpile");
        tiles.Add(new Vector3Int(-9, -8, 0), "stockpile");
        tiles.Add(new Vector3Int(-9, -9, 0), "stockpile");
    }
}
