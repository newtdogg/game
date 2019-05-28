using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;



public class EventController : MonoBehaviour
{
    public Vector3Int bed1;
    private GameObject player;
    private Image sleepImage;
    private bool asleep;
    public bool paused;
    private GameObject sleep;
    private DialogueManager dialogueManager;
	// public static EventController ec;

    private GameObject questionBox;
    public Vector3 nextScenePosition;
    private GameObject npcClone;
    private bool npcUIactive;
    public GameManager gameManager;
    private GameObject questNotificationParent;
    private mainNPC mainNpc;
    private Dictionary<string, int> questNotifications;
    private GameObject npcUI;
	private PlayerController playerController;

    private GameObject newNPC;
    private SceneFadeout sceneFader;
    public float timer;

    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = GameObject.Find("Dialogue").GetComponent<DialogueManager>();

        bed1 = new Vector3Int(-4, 0, 0);
        player = GameObject.Find("Player");
		playerController = player.GetComponent<PlayerController>();
        sleep = gameObject.transform.GetChild(0).gameObject;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        questionBox = gameObject.transform.GetChild(1).gameObject;
        questionBox.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(() => sleepFadeout());
        questionBox.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(() => notSleep());
        sceneFader = GameObject.Find("SceneFadeout").GetComponent<SceneFadeout>();
        questionBox.SetActive(false);
        questNotifications = new Dictionary<string, int>();
		sleepImage = sleep.GetComponent<Image>();
        npcUI = gameObject.transform.GetChild(2).gameObject;
        npcUI.SetActive(false);
        timer = 0;
    }

    // Update is called once per frame
    void Update(){
        pauseToggle();
        if(paused == false){
            checkInBed();
            rebuildStockpile(playerController.mousePointVector);
            if (playerController.stamina <= 0 && playerController.holdingCrate == true){
                dropCrate(playerController.mousePointVector);
            }
            if(Input.GetMouseButtonDown(0) && playerController.holdingCrate == true && timer == 0){
                dropCrate(playerController.mousePointVector);
            }
        }
        if(dialogueManager.inDialogue == true){
            if(Input.GetMouseButtonDown(0)){
                dialogueManager.nextSentence();
            }
        }
        if(npcUI.active == true && Input.GetMouseButtonDown(0) && timer == 0) {
            Debug.Log("heythere");
            npcUI.SetActive(false);
            unpause();
        }
        // Not sure about this one chief
        if(gameManager.stockpile != null){
            createNPCcheck();
        }
        timer = 0;
    }

    public void nextScene(string scene, Vector3Int position){
        nextScenePosition = position;
        sceneFader.fadeToLevel(scene);
    }
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode){
        if(gameManager != null){
            player.transform.position = nextScenePosition;
            Debug.Log("next scene pos");
        }
        if(gameManager) {
            if (gameManager.currentSceneName == "TownHallTutorial2") {
                npcClone = GameObject.Find("NPCclone");
                questNotificationParent = GameObject.Find("QuestNotifications");
                setQuestMarks();
                questmarkActive("SpeakToNPC");
                questmarkInactive("RebuildStockpile");
            }
        }
        // npcUI.SetActive(false);
    }

    void OnEnable(){
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }
         
    void OnDisable(){
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void checkInBed(){
        if(gameManager.currentSceneName == "TownHallTutorial1"){
            if(playerController.checkInPosition(bed1) == true){
                gameObject.transform.GetChild(1).gameObject.SetActive(true);
                playerController.canMove = false;
                // questionBox.transform.GetChild(2).gameObject.GetComponent<Button>();
            }
            if(playerController.checkOverTile(bed1) == true && asleep == true){
                sleepFadeout();  
            }
        }
    }

    private void sleepFadeout() {
        questionBox.SetActive(false);
        sceneFader.fadeToLevel("TownHallTutorial2");
    }


    public void npcUILoad(NPC npc){
        pause();
        gameObject.transform.GetChild(2).gameObject.SetActive(true);
        var name = npcUI.transform.GetChild(0).gameObject.GetComponent<Text>();
        npcUI.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = newNPC.GetComponent<SpriteRenderer>().sprite;
        name.text = npc.name;
        npcUIactive = true;
        timer += Time.deltaTime;
        Debug.Log("loaded");
    }



    private void dropCrate(Vector3Int mouseVec) {
		var currentPosition = playerController.transform.position;
        playerController.heldCrate.transform.position = new Vector3(
            currentPosition.x + (playerController.lastPosition.x/2),
            currentPosition.y + (playerController.lastPosition.y/2) - 0.2f,
            0
        );
        Debug.Log(playerController.heldCrate.transform.position);
		playerController.holdingCrate = false;
        playerController.heldCrate.transform.GetChild(1).gameObject.SetActive(true);
		playerController.heldCrate = null;
	}

    public void createNPCcheck(){
        if(gameManager.stockpile.foodSurplus >= 100){
            gameManager.stockpile.foodSurplus = 0;
            newNPC = Instantiate(npcClone, new Vector3(-14, -4, 0),  Quaternion.Euler(0,0,0));
            Debug.Log("new NPC");
            var npc = new NPC("larry");
            var npcScript = npcClone.GetComponent<basicNPC>();
            npcScript.status = "idle";
            npcScript.npc = npc;
        }
        if(npcUIactive == true){
            if(Input.GetMouseButtonDown(1)){
                unpause();
                npcUI.SetActive(false);
            }
        }
    }

    public void pause(){
        gameManager.paused = true;
        playerController.canMove = false;
        paused = true;
    }

    public void unpause(){
        gameManager.paused = false;
        playerController.canMove = true;
        paused = false;
    }

    public void pauseToggle(){
        if (Input.GetKeyDown(KeyCode.Escape)){
            if(paused == false){
                pause();
            } else {
                unpause();
            }
        }
    }
    private void notSleep(){
        questionBox.SetActive(false);
        playerController.canMove = true;
    }

    private void rebuildStockpile(Vector3Int mouseVec){
        if(dialogueManager.inDialogue == false && gameManager.questManager.Quests["RebuildStockpile"]["status"] == "active" && SceneManager.GetActiveScene().name == "MainWorld"){
            if(Input.GetMouseButtonDown(0)){
                if(gameManager.tiles[mouseVec] != null){
                    if(gameManager.tiles[mouseVec] == "stockpile"){
                        gameManager.questManager.completeQuest(gameManager.questButtonImage, gameManager.questCanvasList, "RebuildStockpile");
                        gameManager.questManager.startQuest(gameManager.questButtonImage, gameManager.questCanvasList, "DeliverCrate");
                        gameManager.transform.GetChild(2).gameObject.SetActive(true);
                        gameManager.stockpile = new Stockpile();
                        gameManager.population = 2;
                        var stockpile = GameObject.Find("Stockpile");
                        var sprite = stockpile.GetComponent<SpriteRenderer>();
                        sprite.sortingOrder = 10;
                        questmarkActive("SpeakToNPC");
                        questmarkInactive("RebuildStockpile");
                    }
                }
            }
        }
    }

    private void setQuestMarks(){
        addQuest("SpeakToNPC", 0);
        addQuest("RebuildStockpile", 1);
    }

    public void questmarkActive(string name){
        questNotificationParent.transform.GetChild(questNotifications[name]).gameObject.SetActive(true);
    }

    public void questmarkInactive(string name){
        questNotificationParent.transform.GetChild(questNotifications[name]).gameObject.SetActive(false);
        Debug.Log($"{name}: inactive");
    }

    private void addQuest(string name, int index){
        questNotifications.Add(name, index);
        questNotificationParent.transform.GetChild(index).gameObject.SetActive(false);
    }
  

}
