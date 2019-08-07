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

    public QuestManager questManager;
    private GameObject questCanvas;
    public List<Text> questCanvasList;

    public Text timeText;
    private GameObject theSun;
    private Light sunLight;
    public Stockpile stockpile;
    public GameObject questButtonObj;
    private Button questButton;
    public Image questButtonImage;
    public int population;
    private Scene currentScene;
    public string currentSceneName;
    private Text foodDisplay;
    private GameObject resourceListClone;
    private List<string> resourceListItems;
    private int resourceListCount;
    private Text foodPercent;
    public bool paused;
    private Text surplus;
    public string nextScene;
    public Dictionary<Vector3Int, string> tiles;
	private GameObject player;

    // Start is called before the first frame update
    void Awake() {
        if(manager == null){
            DontDestroyOnLoad(gameObject);
            manager = this;
            questManager = new QuestManager();
            questCanvas = GameObject.Find("QuestCanvas");
            questManager.populateQuests();
            questCanvasList = new List<Text>();
            questCanvasList.Add(questCanvas.transform.GetChild(0).GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>());
            questCanvasList.Add(questCanvas.transform.GetChild(0).GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<Text>());
            questCanvasList.Add(questCanvas.transform.GetChild(0).GetChild(0).gameObject.transform.GetChild(2).gameObject.GetComponent<Text>());
            setBulletPoints(questCanvasList);
            questCanvas.transform.GetChild(0).gameObject.SetActive(false);
            foodDisplay = gameObject.transform.GetChild(2).GetChild(1).gameObject.GetComponent<Text>();
            surplus = gameObject.transform.GetChild(2).GetChild(2).gameObject.GetComponent<Text>();
            resourceListClone = gameObject.transform.GetChild(2).GetChild(3).gameObject;
			gameObject.transform.GetChild(0).gameObject.SetActive(false);
            resourceListItems = new List<string>();
            resourceListCount = 0;
            population = 1;
        }
        else if (manager != this){
            Destroy(gameObject);
        }
		player = GameObject.Find("Player");
        populateTiles();
        questButtonObj = gameObject.transform.GetChild(1).gameObject;
        questButtonObj.SetActive(false);
        questButton = questButtonObj.GetComponent<Button>();
        questButtonImage = questButtonObj.GetComponent<Image>();
        questButton.onClick.AddListener(() => questcanvasDisplayToggle());
		currentScene = SceneManager.GetActiveScene();
		currentSceneName = currentScene.name;
    }

    void Update()
    {
        if(paused == false){
            if(stockpile != null){
                stockpile.updateStockpile(population);
            }
            currentScene = SceneManager.GetActiveScene();
            currentSceneName = currentScene.name;
            toggleQuestCanvas();
			if(clock != null){
				if(clock.calculateTime() == true){
					writeDateTime();
				}
				sunPosition();
			}
            if(stockpile != null){
                stockpileStats();
            }
        }
    }

    private void setBulletPoints(List<Text> quests){
        foreach(Text quest in quests){
            if(quest.text == ""){
                quest.transform.GetChild(0).gameObject.SetActive(false);
            } else {
                quest.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }


    private void toggleQuestCanvas(){
        if(Input.GetKeyDown(KeyCode.Tab)){
            questcanvasDisplayToggle();
        }
    }

    public void npcGenerator(){

    }


    private void questcanvasDisplayToggle(){
        setBulletPoints(questCanvasList);
        if(questManager.notification == true){
            questButtonImage.color = Color.clear;
            questManager.notification = false;
        }
        if(questCanvas.transform.GetChild(0).gameObject.active == true){
            questCanvas.transform.GetChild(0).gameObject.SetActive(false);
        } else {
            questCanvas.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void sunPosition(){
		if(currentSceneName == "MainWorld" && theSun == null){
			Debug.Log("gotsun");
			theSun = GameObject.Find("Sun");
			theSun.transform.SetParent(player.transform);
			theSun.transform.position = new Vector3(-40, 0, -40);
			// sunLight = theSun.GetComponent<Light>();
		}
        float minutes = (60 * clock.hour) + clock.minute;
        float xConst = 0.1333f;
        float xPosition = 112 - (xConst * minutes);
        float rotConst = 0.1666f;
        float rotValue = (rotConst * minutes) - 140;
        if(currentSceneName == "MainWorld"){
            theSun.transform.rotation = Quaternion.Euler(0, rotValue, 0);
            theSun.transform.localPosition = new Vector3(xPosition, 0, -40);
        }

        // if(minutes < 390){
        //     sunLight.intensity = 32;
        // } else if(minutes > 390 && minutes < 840){
        //     sunLight.intensity -= 0.04f;
        // } else if(minutes > 840 && minutes < 1140){
        //     sunLight.intensity += 0.04f;
        // }
    }

    // ## SETUP ##

    // CLEAN UP THIS METHOD
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


    // ## UI DEBUGGING ##
    private void stockpileStats(){
        var foodstr = Mathf.Round(stockpile.totalFood);  
        var foodperc = Mathf.Round(stockpile.foodSupplyPercentage());      
        var foodmax = stockpile.foodCapacity;
        foodDisplay.text = $"Food: {foodstr}/{foodmax} ({foodperc}%)";
        surplus.text = $"Surplus: {stockpile.foodSurplus}%";
        var count = 4;
        foreach(KeyValuePair<string, Resource> resource in stockpile.stats){
            if(!resourceListItems.Contains(resource.Key)){
                var resourceListItem = Instantiate(resourceListClone, new Vector3(0, 0, 0),  Quaternion.Euler(0,0,0));
                resourceListItem.transform.SetParent(gameObject.transform.GetChild(2));
                resourceListItem.transform.localPosition = new Vector3(0, 168 - (25 * resourceListItems.Count), 0);
                Debug.Log($"resource list: {resource.Key}");
                var resourceVal = resource.Value.value;
                var resourceMax = resource.Value.maxCapacity;
                resourceListItem.GetComponent<Text>().text = $"{char.ToUpper(resource.Key[0]) + resource.Key.Substring(1)}: {resourceVal}/{resourceMax}"; 
                resourceListItems.Add(resource.Key);
                count += 1;
            } else {
                gameObject.transform.GetChild(2).GetChild(count).GetComponent<Text>().text = $"{char.ToUpper(resource.Key[0]) + resource.Key.Substring(1)}: {Mathf.Round(resource.Value.value)}/{Mathf.Round(resource.Value.maxCapacity)}";
                count += 1;
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

     void OnEnable(){
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable(){
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode){
        questCanvas.transform.GetChild(0).gameObject.SetActive(false);
        Debug.Log("here");
    }
}
