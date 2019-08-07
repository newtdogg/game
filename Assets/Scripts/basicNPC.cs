using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicNPC : MonoBehaviour
{
    public NPC npc;
    public string status;
    // Start is called before the first frame update
    protected Animator hairAnim;
    protected Animator clothesAnim;
    public System.Random ran = new System.Random();
    protected AnimatorOverrideController hairAnimatorOverrideController;
    protected AnimatorOverrideController clothesAnimatorOverrideController;
    private EventController eventController;
    public DialogueManager dialogueManager;
    public AnimationClip[] idleHairAnims;
    public AnimationClip[] idleClotheAnims;
    public Dictionary<string, string[]> sentences;
    void Start()
    {
        populateDialogue();
        eventController = GameObject.Find("EventControllerCanvas").GetComponent<EventController>();
        dialogueManager = GameObject.Find("Dialogue").GetComponent<DialogueManager>();

        hairAnim = gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
        hairAnimatorOverrideController = new AnimatorOverrideController(hairAnim.runtimeAnimatorController);
        hairAnim.runtimeAnimatorController = hairAnimatorOverrideController;
        
        clothesAnim = gameObject.transform.GetChild(2).gameObject.GetComponent<Animator>();
        clothesAnimatorOverrideController = new AnimatorOverrideController(clothesAnim.runtimeAnimatorController);
        clothesAnim.runtimeAnimatorController = clothesAnimatorOverrideController;

        generateNPCSprites();
        int num = ran.Next(1, 100);
        var r = 118 + (1.06f * num);
        var g = 98 + (1.16f * num);
        var b = 66 + (1.36f * num); 
        Debug.Log(new Color(r/255, g/255, b/255));
        transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color = new Color(r/255, g/255, b/255);
    }

    // Update is called once per frame
    void Update()
    {
        hairAnim.SetBool("isMoving", false);
        clothesAnim.SetBool("isMoving", false);
        // npcAnimation();
    }

    private void npcAnimation(){
        
    }
    void OnMouseDown() {
        eventController.npcUILoad(npc);
        dialogueManager.StartDialogue(sentences["introduction"]);
        if(eventController.gameManager.questManager.Quests["TalktoNPC"]["status"] == "active"){
            eventController.questmarkActive("SpeakToNPC");
            eventController.gameManager.questManager.completeQuest(eventController.gameManager.questButtonImage, eventController.gameManager.questCanvasList, "TalktoNPC");
        }
    }

    public void generateNPCSprites() {
        var hairTypes = new List<string> { "BlackFlick", "BlackFlick" };
        var clothesTypes = new List<string> { "Mustard", "Mustard" };
        idleHairAnims = Resources.LoadAll<AnimationClip>($"Animations/Hair/{hairTypes[0]}/Idle");
        idleClotheAnims = Resources.LoadAll<AnimationClip>($"Animations/Clothes/{clothesTypes[0]}/Idle");
        hairAnimatorOverrideController["MaleShortIdleDown"] = idleHairAnims[0];
        clothesAnimatorOverrideController["MaroonShortIdleDown"] = idleClotheAnims[0];

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
