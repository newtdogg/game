
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    // Use this for initialization
    public string crateType;
    public bool holdingCrate;
    public GameObject heldCrate;
    public GameObject crateObject;
    public Crate[] crates;
    public GameObject nearestCrate;
    private bool aboveCrate;
    public bool running;
    Rigidbody2D rbody;
    public Animator anim;
    public Vector3 lastPosition;
    public float stamina;
    public float food;
    public float maxStamina;
    public int walkSpeed;
    public Dictionary<string, Vector3> Buildings;
    public int runSpeed;
	public bool recharge;
    private Vector3 dropzone1;
    public System.Random ran = new System.Random();
    private Vector3 dropzone2;
    private Vector3 point;
    private SpriteMask spriteMask;
    private SpriteRenderer spriteRenderer;
    public bool underbeam;
    public bool canMove;
    public QuestManager questManager;
    private GameObject questCanvas;
    public Vector3Int playerPosition;
    public Dictionary<Vector3Int, bool> alreadyInPosition;
    
    private Text questText;
    
    public Vector3 townHallDoor;
   



    // Use this for initialization
    void Start()
    {
        lastPosition = new Vector3(0, 0, 0);
        crateType = "Empty";
        stamina = 2;
        maxStamina = 2;
        crateObject = GameObject.Find("CrateClone");
        crates = Object.FindObjectsOfType<Crate>();
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        dropzone1 = new Vector3(-7, -3, 0);
        dropzone2 = new Vector3(-7, -2, 0);
        Buildings = new Dictionary<string, Vector3>();
        food = 0;
        spriteMask = GetComponent<SpriteMask>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        canMove = true;
        questManager = new QuestManager();
        questManager.populateQuests();
        questText = GameObject.Find("QuestText").GetComponent<Text>();
        questManager.StartQuest1(questText);
        questCanvas = GameObject.Find("QuestCanvas");
        questCanvas.transform.GetChild(0).gameObject.SetActive(false);
        townHallDoor = new Vector3(-13, -3, 0);
        alreadyInPosition = new Dictionary<Vector3Int, bool>();
    }

    // Update is called once per frame
    void Update()
    {
        nearestCrate = GetClosestCrate(crates);
        if (stamina <= 0 && holdingCrate == true){
			dropCrate();
		}
        var point = gameObject.transform.localPosition;
        playerPosition = new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), 0);
        var mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var mousePointVector = new Vector3Int(Mathf.FloorToInt(mousePoint.x), Mathf.FloorToInt(mousePoint.y), 0);
        Vector2 movement_vector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        playerAnimation(movement_vector);
        getLastPosition(movement_vector);
        staminaCalculator();
        generateCrates();
        // spriteMask.sprite = spriteRenderer.sprite;
        toggleCanvas();
        questManager.questsCompleteCheck(questText);
        allAccessDoors(mousePointVector);
    }

    GameObject GetClosestCrate(Crate[] crates){
        GameObject tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Crate t in crates)
        {
            float dist = Vector3.Distance(t.gameObject.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = t.gameObject;
                minDist = dist;
            }
        }
        return tMin;
    }

    public bool checkInPosition(Vector3Int tilePosition){
        if(!alreadyInPosition.ContainsKey(tilePosition)){
            alreadyInPosition.Add(tilePosition, false);
        }
        if(playerPosition.x == tilePosition.x && playerPosition.y == tilePosition.y && alreadyInPosition[tilePosition] == false){
            alreadyInPosition[tilePosition] = true;
            return true;
        }
        if(playerPosition.x == tilePosition.x && playerPosition.y == tilePosition.y && alreadyInPosition[tilePosition] == true){
            // Debug.Log("already in position");
           return false;
        } else {
            // Debug.Log("no longer in position");
            alreadyInPosition[tilePosition] = false;
            return false;
        }
    }


    public bool checkOverTile(Vector3Int tilePosition){
        if(playerPosition.x == tilePosition.x && playerPosition.y == tilePosition.y){
            return true;
        } else {
            return false;
        }
    }

    private void toggleCanvas(){
        if(Input.GetKeyDown(KeyCode.Tab)){
            if(questCanvas.transform.GetChild(0).gameObject.active == true){
                questCanvas.transform.GetChild(0).gameObject.SetActive(false);
            } else {
                questCanvas.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }

	private void dropCrate() {
        point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		var worldPoint = new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), 0);
        
		var currentPosition = this.gameObject.transform.position;
        if(worldPoint == dropzone1 || worldPoint == dropzone2){
            food += 10;
            Debug.Log($"score: {food}");
        } else {
            heldCrate.transform.position = new Vector3(currentPosition.x + (lastPosition.x/2), currentPosition.y + (lastPosition.y/2) - 0.2f, 0);
        }
		
		holdingCrate = false;
		heldCrate = null;
	}

    private void generateCrates(){
        int num = ran.Next(1, 4000);
        
        foreach(KeyValuePair<string, Vector3> building in Buildings)
        {
            // Instantiate(crateObject, building.Value);
            if(num == 1){
                Debug.Log("here");
                Instantiate(crateObject, new Vector3(0, 0, 0),  Quaternion.Euler(0,0,0));
            }
        }
    }

    private void playerAnimation(Vector2 movement_vector){
        if(canMove == true){
            if (movement_vector != Vector2.zero) {
                anim.SetBool("isMoving", true);
                anim.SetFloat("input_X", movement_vector.x);
                anim.SetFloat("input_Y", movement_vector.y);
            } else {
                anim.SetBool("isMoving", false);
            }
            var speed = movementSpeed();
            rbody.MovePosition(rbody.position + movement_vector * Time.deltaTime * speed);
        }
        
    }
    public void getLastPosition(Vector2 movement_vector){
        if(movement_vector.x != 0 || movement_vector.y != 0){
            lastPosition.x = movement_vector.x;
            lastPosition.y = movement_vector.y;
        }
    }

    

    private bool isRunning(){
        // Debug.Log(stamina);
        if (Input.GetKey("space") && stamina > 0 && holdingCrate == false){
            return true;
        }
        return false;
    }

    private void staminaCalculator(){
        if (isRunning() || holdingCrate == true){
            stamina -= Time.deltaTime;
                if (stamina < 0) {
                stamina = -3;
				recharge = true;
            }
        }
        else if (stamina < maxStamina && holdingCrate == false) {
            stamina += Time.deltaTime;
        }
        if (isRunning() == false && stamina > -1 && stamina < 0 && holdingCrate == false){
            stamina = 2;
			recharge = false;
        }
    }

    private int movementSpeed(){
        walkSpeed = 2;
        runSpeed = 4;
        if(isRunning()){
            return runSpeed;
        }
        return walkSpeed;
    }

    private void anyDoorEntry(Vector3 door, Vector3 mousePosition, string scene){
        if(Input.GetMouseButtonDown(0)){
            if(door.y - 1 == playerPosition.y && door.x == playerPosition.x){
                if(mousePosition == door){
                    SceneManager.LoadScene(scene);
                }
            }
		}
    }

    private void allAccessDoors(Vector3Int mousePosition){
        anyDoorEntry(townHallDoor, mousePosition, "TownHall");
    }

}