using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class DialogueManager : MonoBehaviour
{
    // Start is called before the first frame update
    public bool inDialogue;

    private PlayerController playerController;
    private Text textBox;
    private int sentenceIndex;
    private string[] sentences;

    void Awake()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        textBox = gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>();
    }

    public void StartDialogue(string[] dialogue){
        sentences = dialogue;
        textBox.text = sentences[0];
		sentenceIndex = 0;
		inDialogue = true;
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        playerController.anim.SetBool("isMoving", false);
		playerController.canMove = false;
	}

	public void nextSentence(){
        playerController.canMove = false;
		if(Input.GetMouseButtonDown(0)){
            if(sentenceIndex == sentences.Length -1){
                playerController.canMove = true;
                gameObject.transform.GetChild(0).gameObject.SetActive(false);
                inDialogue = false;
                return;
            }
            sentenceIndex += 1;
            textBox.text = sentences[sentenceIndex];
        }
	}


    public void hideDialogue(){
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }


    void OnEnable(){
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable(){
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode){
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }


}
