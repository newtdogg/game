using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainNPC : NPCobject
{
    private GameObject canvas;
    public DialogueManager dialogueManager;
    private PlayerController playerController;
    private BoxCollider2D collider;

    private BoxCollider2D areaCollider;
    
    private GameObject player;
    public Dictionary<string, string[]> sentences;
    // Start is called before the first frame update
    void Start()
    {
        npc = new NPC("Barry Onions");
        isMoving = false;
        dialogueManager = GameObject.Find("Dialogue").GetComponent<DialogueManager>();
        rbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        dialogueManager.hideDialogue();
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        populateDialogue();
        eventController = GameObject.Find("EventControllerCanvas").GetComponent<EventController>();
        areaCollider = transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>();
        inContact = false;
        speed = 5;
    }

    // Update is called once per frame
    void Update()
    {
        // speed = inContact ? 10 : 5;
        // if(gameManager.questManager.Quests["buildWoodcuttersHut"]["status"] == "complete") {
            // Debug.Log(inContact);
        
        if(npc.employment == "moving crates"){
            npcMovement(rbody);
        }
        // }
    }

    // void OnMouseDown(){
        // if(gameManager.questManager.Quests["RebuildStockpile"]["status"] == "complete" && gameManager.questManager.Quests["DeliverCrate"]["status"] != "complete") {
        //     gameManager.questManager.startQuest(gameManager.questButtonImage, gameManager.questCanvasList, "DeliverCrate");
        // }
        // if(dialogueManager.inDialogue == false) {
        //     switch(gameManager.questManager.mainActiveQuest) {
        //         case "SpeakToBarry":
        //             dialogueManager.StartDialogue(sentences["introduction"]);
        //             gameManager.questManager.completeQuest(gameManager.questButtonImage, gameManager.questCanvasList, "SpeakToBarry");            
        //             gameManager.questManager.startQuest(gameManager.questButtonImage, gameManager.questCanvasList, "RebuildStockpile");
        //             eventController.questmarkInactive("SpeakToNPC");
        //             eventController.questmarkActive("RebuildStockpile");
        //             break;
        //         case "DeliverCrate":
        //             dialogueManager.StartDialogue(sentences["crate"]);
        //             eventController.questmarkInactive("SpeakToNPC");
        //             break;
        //         case "TalktoNPC":
        //             dialogueManager.StartDialogue(sentences["buildFirstBuilding"]);
        //             gameManager.questManager.startQuest(gameManager.questButtonImage, gameManager.questCanvasList, "BuildFarm");
        //             eventController.questmarkInactive("SpeakToNPC");
        //             break;
        //         case "BuildFarm":
        //             dialogueManager.StartDialogue(sentences["buildWoodcuttersHut"]);
        //             gameManager.questManager.startQuest(gameManager.questButtonImage, gameManager.questCanvasList, "buildWoodcuttersHut");
        //             eventController.questmarkInactive("SpeakToNPC");
        //             break;
                
        //     }
        // }
    // }

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
            },
            {
                "buildWoodcuttersHut", new string[]
                {
                    "dud",
                    "Brilliant, but if we're to build more buildings to accomodate growth, we'll need a supply of wood",
                    "Build a Woodcutters Hut over by the forest"
                }
            }
        };
    }
    
}
