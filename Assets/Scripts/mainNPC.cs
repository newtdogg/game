using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainNPC : MonoBehaviour
{
    private Dialogue dialogue;
    private GameObject canvas;
    private DialogueManager dialogueManager;
    private PlayerController playerController;
    private GameObject player;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        dialogue = new Dialogue();
        dialogueManager = GameObject.Find("Dialogue").GetComponent<DialogueManager>();
        dialogueManager.hideDialogue();
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.questManager.Quests["Quest2"]["status"] == "complete"){
            gameObject.transform.position = new Vector3(-6, -4, 0);
        }
        isInDialogue();
    }

    void OnMouseDown(){
        if(dialogue.inDialogue == false){
            dialogueManager.StartDialogue(dialogue);
            gameManager.questManager.Quests["Quest3"]["status"] = "complete";
        }
    }

    private void nearPlayerLayerOrder(){
        var dist = Vector3.Distance(player.transform.position, gameObject.transform.position);
        if(dist < 0.6) {
            if(player.transform.position.y > gameObject.transform.position.y + 0.4f){
                var sprite = gameObject.GetComponent<SpriteRenderer>();
                sprite.sortingOrder = 10;
                Debug.Log("1");
            }
            if(player.transform.position.y < gameObject.transform.position.y + 0.4f){
                var sprite = gameObject.GetComponent<SpriteRenderer>();
                sprite.sortingOrder = 3;
                Debug.Log("2");
            }
        }
    }

    private void isInDialogue(){
        if(dialogue.inDialogue == true){
            dialogueManager.continueDialogue(dialogue);
        }
    }
}
