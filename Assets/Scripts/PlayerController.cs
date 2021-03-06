﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    // Use this for initialization

    public Animator hairAnim;
    public Animator bodyAnim;
    public Animator clothesAnim;
    public Animator legsAnim;


    public string crateType;
    private static PlayerController playerController;
    public bool holdingCrate;
    public GameObject heldCrate;
    public Crate heldCrateScript;
    public GameObject crateObject;
    public Crate[] crates;
    public SpriteRenderer crateContent;
    public GameObject nearestCrate;
    private bool aboveCrate;
    public Sprite[] crateContentSprites;
    
    public bool running;
    private Rigidbody2D rbody;
    public Animator anim;
    public Vector3 lastPosition;
    public float stamina;
    public float food;
    public float maxStamina;
    public int walkSpeed;
    public Dictionary<string, Vector3> Buildings;
    public int runSpeed;
	public bool recharge;

    private InventorySceneManager inventorySceneManager;
    public GameObject inventoryActive;
    private GameObject inventory;
    public Vector3Int mousePointVector;
    public Vector3Int townHallPosition;
    public System.Random ran = new System.Random();

    private Vector3 point;
    public int maxSlots;
    public int inventorySlot;
    private SpriteMask spriteMask;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer crateInnerSprite;
    public bool canMove;
    public Vector3Int playerPosition;
    public Dictionary<Vector3Int, bool> alreadyInPosition;

    public GameManager gameManager;
    public Vector3 townHallDoor;
    public float timer;

    void Awake() {
        if(playerController == null){
            DontDestroyOnLoad(gameObject);
            playerController = this;
        }
        else if (playerController != this){
            Destroy(gameObject);
        }
    }


    // Use this for initialization
    void Start(){
        hairAnim = transform.GetChild(6).gameObject.GetComponent<Animator>();
        bodyAnim = transform.GetChild(7).gameObject.GetComponent<Animator>();
        clothesAnim = transform.GetChild(8).gameObject.GetComponent<Animator>();
        legsAnim = transform.GetChild(9).gameObject.GetComponent<Animator>();
        
        lastPosition = new Vector3(0, 0, 0);
        crateType = "Empty";
        stamina = 5;
        maxStamina = 5;
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Buildings = new Dictionary<string, Vector3>();
        food = 0;
        walkSpeed = 5;
        spriteMask = GetComponent<SpriteMask>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        canMove = true;
        townHallDoor = new Vector3(-13, -3, 0);
        alreadyInPosition = new Dictionary<Vector3Int, bool>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        crateContent = gameObject.transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>();
        gameObject.transform.GetChild(4).gameObject.SetActive(false);
        crateInnerSprite = gameObject.transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>();
        timer = 0;
        inventory = gameObject.transform.GetChild(10).gameObject;
        inventoryActive = inventory.transform.GetChild(0).gameObject;
        inventorySceneManager = GameObject.Find("InventorySceneManager").GetComponent<InventorySceneManager>();
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.sortingOrder = 200 - Mathf.RoundToInt(gameObject.transform.position.y * 10);
        crateInnerSprite.sortingOrder = 201 - Mathf.RoundToInt(gameObject.transform.position.y * 10);
        if(holdingCrate == false) {
            gameObject.transform.GetChild(4).gameObject.SetActive(false);
        }
        maxSlots = 1;
        var point = gameObject.transform.localPosition;
        playerPosition = new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), 0);
        var mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePointVector = new Vector3Int(Mathf.FloorToInt(mousePoint.x), Mathf.FloorToInt(mousePoint.y), 0);
        // vectorLog(mousePointVector, playerPosition, mousePoint, point);
        Vector2 movement_vector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        playerAnimation(movement_vector);
        getLastPosition(movement_vector);
        // staminaCalculator();
        timer = 0;
        // generateCrates();
        // spriteMask.sprite = spriteRenderer.sprite;
        inventoryScroll();
        inventorySceneManager.inventoryActive = inventoryActive.transform.GetSiblingIndex();
    }

    private void inventoryScroll(){
        if(Input.GetAxis("Mouse ScrollWheel") != 0){
            inventorySlot += (int) (Input.GetAxis("Mouse ScrollWheel") * 10);
            if(inventorySlot > maxSlots) {
                inventorySlot = 0;
            }
            if(inventorySlot < 0) {
                inventorySlot = maxSlots;
            }
            deactivateInventorySlots();
            setInventoryItem(inventorySlot);
        }
    }

    private void deactivateInventorySlots(){
        foreach (Transform child in inventory.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void setInventoryItem(int slot){
        inventory.transform.GetChild(slot).gameObject.SetActive(true);
        inventoryActive = inventory.transform.GetChild(slot).gameObject;
        anim.SetBool("isMoving", false);
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
           return false;
        } else {
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

    public bool checkPlayerNextToTiles(){
        if((mousePointVector.x + 1 == playerPosition.x && mousePointVector.y + 1 == playerPosition.y)  ||
          (mousePointVector.x + 0 == playerPosition.x && mousePointVector.y + 1 == playerPosition.y)  ||
          (mousePointVector.x + -1 == playerPosition.x && mousePointVector.y + 1 == playerPosition.y) ||
          (mousePointVector.x + 1 == playerPosition.x && mousePointVector.y + 0 == playerPosition.y)  ||
          (mousePointVector.x + -1 == playerPosition.x && mousePointVector.y + 0 == playerPosition.y) ||
          (mousePointVector.x + 1 == playerPosition.x && mousePointVector.y + -1 == playerPosition.y) ||
          (mousePointVector.x + 0 == playerPosition.x && mousePointVector.y + -1 == playerPosition.y) ||
          (mousePointVector.x + -1 == playerPosition.x && mousePointVector.y + -1 == playerPosition.y))
          {
              return true;
          } else {
              return false;
          }

    }



    private void generateCrates(){
        // int num = ran.Next(1, 4000);

        // foreach(KeyValuePair<string, Vector3> building in Buildings)
        // {
        //     // Instantiate(crateObject, building.Value);
        //     if(num == 1){
        //         Instantiate(crateObject, new Vector3(0, 0, 0),  Quaternion.Euler(0,0,0));
        //     }
        // }
    }

    private void playerAnimation(Vector2 movement_vector){
        if(canMove == true){
            if(holdingCrate == true){
                anim.SetBool("isHoldingCrate", true);
                // setCrateContent(movement_vector);
            } else {
                anim.SetBool("isHoldingCrate", false);
            }
            if (movement_vector != Vector2.zero) {
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
                anim.SetBool("isMoving", false);
                hairAnim.SetBool("isMoving", false);
                bodyAnim.SetBool("isMoving", false);
                clothesAnim.SetBool("isMoving", false);
                legsAnim.SetBool("isMoving", false);
            }
            // var speed = movementSpeed();
            rbody.MovePosition(rbody.position + movement_vector * Time.deltaTime * walkSpeed);
        } else {
            anim.SetBool("isMoving", false);
        }

    }
    public void getLastPosition(Vector2 movement_vector){
        if(movement_vector.x != 0 || movement_vector.y != 0){
            if(movement_vector.x != 0 && movement_vector.y == 0){
                lastPosition.x = movement_vector.x;
                lastPosition.y = 0;
            } else if(movement_vector.y != 0 && movement_vector.x == 0){
                lastPosition.y = movement_vector.y;
                lastPosition.x = 0;
            }
        }
    }



    // private bool isRunning(){
    //     if (Input.GetKey("space") && stamina > 0 && holdingCrate == false){
    //         return true;
    //     }
    //     return false;
    // }

    // private void staminaCalculator(){
    //     if (isRunning() || holdingCrate == true){
    //         stamina -= Time.deltaTime;
    //             if (stamina < 0) {
    //             stamina = -6;
	// 			recharge = true;
    //         }
    //     }
    //     else if (stamina < maxStamina && holdingCrate == false) {
    //         stamina += Time.deltaTime;
    //     }
    //     if (isRunning() == false && stamina > -1 && stamina < 0 && holdingCrate == false){
    //         stamina = 5;
	// 		recharge = false;
    //     }
    // }

    // private int movementSpeed(){
    //     walkSpeed = 2;
    //     runSpeed = 4;
    //     if(isRunning()){
    //         return runSpeed;
    //     }
    //     return walkSpeed;
    // }


    // VECTOR TOOL
    private void vectorLog(Vector3Int mousePosition, Vector3Int playerPosition, Vector3 mousePoint, Vector3 rawPlayerPosition){
        if(Input.GetMouseButtonDown(0)){
            Debug.Log(mousePosition);
            Debug.Log(Camera.main.WorldToScreenPoint(mousePoint));
            Debug.Log(playerPosition);
            Debug.Log(rawPlayerPosition);
        }
    }

    private void setCrateContent(Vector2 movement_vector){
        Debug.Log(crateContentSprites[0]);
        if(movement_vector.y == 1){
            crateContent.sprite = crateContentSprites[0];
        } else if (movement_vector.x == 1){
            crateContent.sprite = crateContentSprites[1];
        } else if (movement_vector.y == -1){
            crateContent.sprite = crateContentSprites[2];
        } else if (movement_vector.x == -1){
            crateContent.sprite = crateContentSprites[3];
        }
    }
}