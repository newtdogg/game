using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    // Start is called before the first frame update
    private Text textBox;
    private PlayerController playerController;
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }


    public void hideDialogue(){
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
    public void StartDialogue(Dialogue dialogue){
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        playerController.anim.SetBool("isMoving", false);
        textBox = gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
        dialogue.StartDialogue(textBox);
    }
    public void continueDialogue(Dialogue dialogue){
        playerController.canMove = false;
        dialogue.nextSentence(textBox);
        if(dialogue.dialogueComplete == true){
			playerController.canMove = true;
		}
    }

    
}
