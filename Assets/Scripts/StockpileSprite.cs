using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockpileSprite : MonoBehaviour {
    private PlayerController playerController;
    private GameManager gameManager;
    private mainNPC mainNpc;
    
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        mainNpc = GameObject.Find("NPC").GetComponent<mainNPC>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown(){
        if(playerController.holdingCrate == true){
            if(playerController.checkPlayerNextToTiles() == true){
                Debug.Log("Dropped Crate");
                if(gameManager.questManager.Quests["DeliverCrate"]["status"] == "active"){
                    mainNpc.dialogueManager.StartDialogue(mainNpc.sentences["npcArrive"]);
                    gameManager.questManager.completeQuest(gameManager.questButtonImage, gameManager.questCanvasList, "DeliverCrate");
                    gameManager.questManager.startQuest(gameManager.questButtonImage, gameManager.questCanvasList, "TalktoNPC");
                }
                // Debug.Log(gameManager.stockpile.stats[playerController.heldCrateScript.type]);
                Debug.Log(gameManager.stockpile.stats);
                gameManager.stockpile.stats[playerController.heldCrateScript.type].value += playerController.heldCrateScript.value;
                Debug.Log(playerController.heldCrateScript.value);
                Debug.Log(gameManager.stockpile.stats[playerController.heldCrateScript.type].value);
                Destroy(playerController.heldCrate);
                playerController.heldCrate = null;
                playerController.heldCrateScript = null;
                playerController.holdingCrate = false;
            }
        }
    }
}
