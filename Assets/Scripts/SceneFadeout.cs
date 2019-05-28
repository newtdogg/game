using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class SceneFadeout : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator sceneFadeAnimator;
    public static SceneFadeout controller;
    private string levelToLoad;
    private GameObject player;
    private PlayerController playerController;
    private PlayerBasicIntro playerBasicIntro;
    private PlayerIntro playerIntro;


    void Start()
    {
        sceneFadeAnimator = GetComponent<Animator>();
        player = GameObject.Find("Player");
        playerBasicIntro = player.GetComponent<PlayerBasicIntro>();
        playerController = player.GetComponent<PlayerController>();
        // sceneFadeAnimator.Play("sceneFadeOut");
    }

    public void fadeToLevel(string sceneName) {
        levelToLoad = sceneName;
        if(levelToLoad == "introductionMainWorld") {
            playerBasicIntro.canMove = false;
        }
        if(levelToLoad == "TownHallTutorial1") {
            playerIntro.canMove = false;
        }
        sceneFadeAnimator.SetTrigger("FadeOut");
    }

    public void onFadeComplete(){
        if(levelToLoad == "TownHallTutorial2") {
            sleepTutorial();
        }
        SceneManager.LoadScene(levelToLoad);
        sceneFadeAnimator.SetTrigger("FadeOut");
    }

    private void sleepTutorial(){
        var gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.clock = new WorldClock();
        gameManager.transform.GetChild(0).gameObject.SetActive(true);
        gameManager.dayText = gameManager.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>();
        gameManager.timeText = gameManager.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Text>();
        gameManager.questButtonObj.SetActive(true);
        gameManager.questManager.startQuest(gameManager.questButtonImage, gameManager.questCanvasList, "Explore");
        gameManager.questManager.startQuest(gameManager.questButtonImage, gameManager.questCanvasList, "SpeakToBarry");
        gameManager.clock.nextDay();
        playerController.canMove = true;
    }

    // Update is called once per frame
    void Update(){
    }

    public void resetAnimator() {
        sceneFadeAnimator.Rebind();
        if(levelToLoad == "introductionMainWorld") {
            playerIntro = GameObject.Find("Player").GetComponent<PlayerIntro>();
            playerIntro.canMove = true;
        };
        if(levelToLoad == "TownHallTutorial1") {
            playerController = GameObject.Find("Player").GetComponent<PlayerController>();
            playerIntro.canMove = false;
        }
    }



}
