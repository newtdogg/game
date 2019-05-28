using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicNPC : MonoBehaviour
{
    public NPC npc;
    public string status;
    // Start is called before the first frame update
    private EventController eventController;
    public DialogueManager dialogueManager;
    public Dictionary<string, string[]> sentences;
    void Start()
    {
        populateDialogue();
        eventController = GameObject.Find("EventControllerCanvas").GetComponent<EventController>();
        dialogueManager = GameObject.Find("Dialogue").GetComponent<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown() {
        eventController.npcUILoad(npc);
        dialogueManager.StartDialogue(sentences["introduction"]);
        if(eventController.gameManager.questManager.Quests["TalktoNPC"]["status"] == "active"){
            eventController.questmarkActive("SpeakToNPC");
            eventController.gameManager.questManager.completeQuest(eventController.gameManager.questButtonImage, eventController.gameManager.questCanvasList, "TalktoNPC");
        }
    }

     private void populateDialogue(){
        sentences = new Dictionary<string, string[]>{
            { 
                "introduction", new string[] 
                { 
                    "Poo poo pee pee",
                    "A friendly face? I'm looking to trade my hard work for a place to rest my head"
                } 
            }
        };
    }
}
