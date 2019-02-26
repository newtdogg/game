using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class EventController : MonoBehaviour
{
    public Vector3Int bed1;
    public Vector3Int bed2;
    private GameObject player;
    private Image sleepImage;
    private bool asleep;
    private GameObject eventCanvas;
    private GameObject sleep;
    private GameObject questionBox;
    private GameManager gameManager;
	private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        bed1 = new Vector3Int(-4, 0, 0);
        bed2 = new Vector3Int(-3, 0, 0);
        player = GameObject.Find("Player");
        eventCanvas = GameObject.Find("EventControllerCanvas");
        sleep = eventCanvas.transform.GetChild(0).gameObject;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        questionBox = eventCanvas.transform.GetChild(1).gameObject;
        questionBox.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(() => sleepFadeout());
        questionBox.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(() => notSleep());
        questionBox.SetActive(false);
        sleepImage = sleep.GetComponent<Image>();
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update(){
        rebuildStockpile(playerController.mousePointVector);
        leaveBuilding();
    }

    private void sleepFadeout(){
        // sleepImage.color = new Color(0, 0, 0, 0);
        gameManager.questManager.Quests["Quest2"]["status"] = "complete";
        gameManager.clock.nextDay();
        questionBox.SetActive(false);
    }

    private void checkInBed(){
         if(playerController.checkInPosition(bed1) == true){
            eventCanvas.transform.GetChild(1).gameObject.SetActive(true);
            // questionBox.transform.GetChild(2).gameObject.GetComponent<Button>();
		}
        if(playerController.checkOverTile(bed1) == true && asleep == true){
            Debug.Log("sleeping");
            sleepFadeout();
		}
    }

    private void notSleep(){
        questionBox.SetActive(false);
    }

    private void leaveBuilding(){
        if(SceneManager.GetActiveScene().name == "TownHall" && playerController.checkOverTile(new Vector3Int(-1, -4, 0))){
            SceneManager.LoadScene("main_world");
        }
    }

    private void rebuildStockpile(Vector3Int mouseVec){
        // if(gameManager.questManager.Quests["Quest2"]["status"] == "complete"){
            if(Input.GetMouseButtonDown(0)){
                if(gameManager.tiles[mouseVec] == "stockpile"){
                    // gameManager.questManager.Quests["Quest4"]["status"] = "complete";
                    gameManager.stockpile = new Stockpile(400);
                    Debug.Log("hello123");
                }
            }
        // }
    }

}
