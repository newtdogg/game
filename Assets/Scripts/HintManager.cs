using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintManager : MonoBehaviour
{
    // Start is called before the first frame update
    public string currentScene;
    public bool inHint;
    private Image hintImage;
    private string currentHint;
    private PlayerBasicIntro playerBasicController;
    private PlayerController playerController;
    public float timer;

     private static HintManager hintManager;
    void Awake() {
        if(hintManager == null){
            DontDestroyOnLoad(gameObject);
            hintManager = this;
        }
        else if (hintManager != this){
            Destroy(gameObject);
        }
    }
    void Start(){
        hintImage = gameObject.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Image>();
        if(currentScene == "MainWorld") {
            playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        } else {
            playerBasicController = GameObject.Find("Player").GetComponent<PlayerBasicIntro>();
            setHint("Find Shelter!", "wasd");
        }
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(inHint == true && timer == 0){
            if(Input.GetMouseButtonDown(0)){
                if(currentHint == "Find Shelter!") {
                    setHint("Grab a torch from the campfire", "wasd");
                    return;
                }
                playerBasicController.canMove = true;
                gameObject.transform.GetChild(0).gameObject.SetActive(false);
                inHint = false;
            }
        }
        timer = 0;
    }

    public void setHint(string hint, string hintImageName){
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        hintImage.sprite = Resources.Load<Sprite>($"Hints/{hintImageName}");
        gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = hint;
        // playerController.GetComponent<Animator>().SetBool("isMoving", false);
        inHint = true;
        playerBasicController.canMove = false;
        currentHint = hint;
        Debug.Log(hint);
    }
}
