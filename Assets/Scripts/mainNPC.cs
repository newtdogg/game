using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainNPC : MonoBehaviour
{
    private GameObject canvas;
    public DialogueManager dialogueManager;
    private PlayerController playerController;
    private GameObject player;
    private GameManager gameManager;
    private EventController eventController;
    public Dictionary<string, string[]> sentences;
    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = GameObject.Find("Dialogue").GetComponent<DialogueManager>();
        dialogueManager.hideDialogue();
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        populateDialogue();
        eventController = GameObject.Find("EventControllerCanvas").GetComponent<EventController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown(){
        if(dialogueManager.inDialogue == false && gameManager.questManager.Quests["SpeakToBarry"]["status"] == "active"){
            dialogueManager.StartDialogue(sentences["introduction"]);
            gameManager.questManager.completeQuest(gameManager.questButtonImage, gameManager.questCanvasList, "SpeakToBarry");            
            gameManager.questManager.startQuest(gameManager.questButtonImage, gameManager.questCanvasList, "RebuildStockpile");
            eventController.questmarkInactive("SpeakToNPC");
            eventController.questmarkActive("RebuildStockpile");
        } else if(dialogueManager.inDialogue == false && gameManager.questManager.Quests["RebuildStockpile"]["status"] == "complete" && gameManager.questManager.Quests["TalktoNPC"]["status"] != "complete"){
            dialogueManager.StartDialogue(sentences["crate"]);
            gameManager.questManager.startQuest(gameManager.questButtonImage, gameManager.questCanvasList, "DeliverCrate");
            eventController.questmarkInactive("SpeakToNPC");
        } else if(dialogueManager.inDialogue == false && gameManager.questManager.Quests["TalktoNPC"]["status"] == "complete"){
            dialogueManager.StartDialogue(sentences["buildFirstBuilding"]);
            gameManager.questManager.startQuest(gameManager.questButtonImage, gameManager.questCanvasList, "BuildFarm");
            eventController.questmarkInactive("SpeakToNPC");
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


    private void populateDialogue(){
        sentences = new Dictionary<string, string[]>{
            { 
                "introduction", new string[] 
                { 
                    "dud",
                    "Good morning…", 
                    "…",
                    "Sorry I didn’t give a proper introduction, can’t be too careful during times of war",
                    "My name is Barry Onions, who are you?",
                    "How did you end up here? With nothing to defend yourself? These areas are teaming with mercenaries and bandits",
                    "Well if you no longer have a home you can help me rebuild mine! We should set about improving this place",
                    "With both of us at work we could rebuild the town, maybe more passing travellers and other vagrants will settle here",
                    "The first task is to rebuild the stockpile",
                    "We used these stockpiles to store all goods that the town produced, be them farmed or foraged",
                    "This way we can always keep track of our stock",
                    "If you’re going to be transporting heavy goods to the stockpile, you’ll need these"
                }
            },
            { 
                "crate", new string[] 
                { 
                    "dud",
                    "Hey that wasn’t so bad",
                    "Now go grab that crate of food from other there and store it in the stockpile...",
                    "And be quick! Some don’t want things to spoil in the sun"
                } 
            },
            { 
                "traveller", new string[] 
                { 
                    "dud",
                    "Now...",
                    "Who’s this…?",
                    "Two travellers stumbleupon here a day apart? Well I never!",
                    "Hey, why not ask him if he wants to help out rebuild this place"
                } 
            },
            {
                "npcArrive", new string[]
                {
                    "dud",
                    "Hey who’s this…",
                    "Two travellers stumbleupon here a day apart? A strange coincidence!",
                    "Hey, why not ask him if he wants to help us"

                }
            },
            {
                "buildFirstBuilding", new string[]
                {
                    "dud",
                    "Well, now theres 3 of us lets get to work",
                    "May as well get the most out of this traveller before they carry on their journey",
                    "...and build it somewhere close to the stockpile"
                }
            }
        };
    }
}
