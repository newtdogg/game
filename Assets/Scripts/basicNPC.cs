using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicNPC : NPCobject
{
    public string status;
    // Start is called before the first frame update
    protected Animator hairAnim;
    public System.Random ran = new System.Random();
    protected AnimatorOverrideController hairAnimatorOverrideController;
    protected AnimatorOverrideController clothesAnimatorOverrideController;
    public DialogueManager dialogueManager;
    public AnimationClip[] idleHairAnims;
    public AnimationClip[] idleClotheAnims;
    public Dictionary<string, string[]> sentences;
    public Animator bodyAnim;
    public Animator clothesAnim;
    public Animator legsAnim;


    void Start(){
        npc = new NPC("clone", gameObject);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        populateDialogue();
        eventController = GameObject.Find("EventControllerCanvas").GetComponent<EventController>();
        dialogueManager = GameObject.Find("Dialogue").GetComponent<DialogueManager>();

        hairAnim = gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
        hairAnimatorOverrideController = new AnimatorOverrideController(hairAnim.runtimeAnimatorController);
        hairAnim.runtimeAnimatorController = hairAnimatorOverrideController;
        
        clothesAnim = gameObject.transform.GetChild(2).gameObject.GetComponent<Animator>();
        clothesAnimatorOverrideController = new AnimatorOverrideController(clothesAnim.runtimeAnimatorController);
        clothesAnim.runtimeAnimatorController = clothesAnimatorOverrideController;
        isMoving = false;

        rbody = GetComponent<Rigidbody2D>();
        hairAnim = transform.GetChild(0).gameObject.GetComponent<Animator>();
        bodyAnim = transform.GetChild(1).gameObject.GetComponent<Animator>();
        clothesAnim = transform.GetChild(2).gameObject.GetComponent<Animator>();
        legsAnim = transform.GetChild(3).gameObject.GetComponent<Animator>();

        generateNPCSprites();
        int num = ran.Next(1, 100);
        var r = 118 + (1.06f * num);
        var g = 98 + (1.16f * num);
        var b = 66 + (1.36f * num); 
        transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color = new Color(r/255, g/255, b/255);
    }

    // Update is called once per frame
    void Update()
    {
        speed = 5 + (npc.stats["rapidity"]/2);
        hairAnim.SetBool("isMoving", false);
        clothesAnim.SetBool("isMoving", false);
        // npcAnimation();
        if(npc.employment == "moving crates"){
            npcMovement(rbody);
        }
        // Vector2 movement_vector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        npcAnimation(movementDirection);
        
    }

    private void npcAnimation(Vector2 movement_vector){
        if(isMoving == true && movement_vector != Vector2.zero){
            // if(holdingCrate == true){
            //     anim.SetBool("isHoldingCrate", true);
            //     // setCrateContent(movement_vector);
            // } else {
            //     anim.SetBool("isHoldingCrate", false);
            // }
                hairAnim.SetBool("isMoving", true);
                hairAnim.SetFloat("input_X", movement_vector.x);
                hairAnim.SetFloat("input_Y", movement_vector.y);
                bodyAnim.SetBool("isMoving", true);
                bodyAnim.SetFloat("input_X", movement_vector.x);
                bodyAnim.SetFloat("input_Y", movement_vector.y);
                clothesAnim.SetBool("isMoving", true);
                clothesAnim.SetFloat("input_X", movement_vector.x);
                clothesAnim.SetFloat("input_Y", movement_vector.y);
                legsAnim.SetBool("isMoving", true);
                legsAnim.SetFloat("input_X", movement_vector.x);
                legsAnim.SetFloat("input_Y", movement_vector.y);
            } else {
                // anim.SetBool("isMoving", false);
                hairAnim.SetBool("isMoving", false);
                bodyAnim.SetBool("isMoving", false);
                clothesAnim.SetBool("isMoving", false);
                legsAnim.SetBool("isMoving", false);
            }
            // var speed = movementSpeed();
            rbody.MovePosition(rbody.position + movement_vector * Time.deltaTime * speed);
        // } else {
        //     anim.SetBool("isMoving", false);
        // }

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
