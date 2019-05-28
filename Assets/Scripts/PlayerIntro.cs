using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerIntro : MonoBehaviour
{
    public bool canMove;
    private List<Vector3Int> exit;
    private Rigidbody2D rbody;
    public GameObject inventoryActive;
    private InventorySceneManager inventorySceneManager;

    private GameObject inventory;
    public Animator anim;
    public Animator hairAnim;
    public Animator bodyAnim;
    public Animator clothesAnim;
    public Animator legsAnim;

    public int maxSlots;
    private SceneFadeout sceneFader;
    public int inventorySlot;
    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        canMove = false;
        inventorySlot = 0;
        maxSlots = 1;
        inventory = gameObject.transform.GetChild(1).gameObject;
        inventorySceneManager = GameObject.Find("InventorySceneManager").GetComponent<InventorySceneManager>();
        inventoryActive = gameObject.transform.GetChild(1).GetChild(inventorySceneManager.inventoryActive).gameObject;
        deactivateInventorySlots();
        setInventoryItem(inventorySceneManager.inventoryActive);
        sceneFader = GameObject.Find("SceneFadeout").GetComponent<SceneFadeout>();
        hairAnim = transform.GetChild(3).gameObject.GetComponent<Animator>();
        bodyAnim = transform.GetChild(4).gameObject.GetComponent<Animator>();
        clothesAnim = transform.GetChild(5).gameObject.GetComponent<Animator>();
        legsAnim = transform.GetChild(6).gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        inventoryScroll();
        Vector2 movement_vector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        playerAnimation(movement_vector);
        var point = gameObject.transform.localPosition;
        var playerPosition = new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), 0);
		var mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var mousePointVector = new Vector3Int(Mathf.FloorToInt(mousePoint.x), Mathf.FloorToInt(mousePoint.y), 0);
		allAccessDoors(mousePointVector, playerPosition);

    }

    private void playerAnimation(Vector2 movement_vector){
        if(inventoryActive.name == "Torch"){
            anim.SetBool("isHoldingTorch", true);
        } else {
            anim.SetBool("isHoldingTorch", false);
        }
        if(canMove == true){
           if (movement_vector != Vector2.zero) {
                anim.SetBool("isMoving", true);
                anim.SetFloat("input_X", movement_vector.x);
                anim.SetFloat("input_Y", movement_vector.y);
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
            var speed = 3;
            rbody.MovePosition(rbody.position + movement_vector * Time.deltaTime * speed);
        } else {
            anim.SetBool("isMoving", false);
        }

    }

    private void inventoryScroll(){
        if(Input.GetAxis("Mouse ScrollWheel") > 0 && inventorySlot <= maxSlots){
            deactivateInventorySlots();
            if(inventorySlot == maxSlots){
                inventorySlot = 0;
            } else {
                inventorySlot += 1;
            }
            if(inventorySlot <= inventory.transform.childCount){
                inventoryActive = inventory.transform.GetChild(inventorySlot).gameObject;
            }
            setInventoryItem(inventorySlot);
        }
        if(Input.GetAxis("Mouse ScrollWheel") < 0){
            deactivateInventorySlots();
            if(inventorySlot == 0){
                inventorySlot = maxSlots;
            } else {
                inventorySlot -= 1;
            }
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
    public void setInventory(){
		inventory.transform.GetChild(inventorySlot).gameObject.SetActive(true);
		inventoryActive = inventory.transform.GetChild(inventorySlot).gameObject;
    }

    private void inventoryItemsSelect(){}


						// //////////// //
						// DOOR METHODS //
						// //////////// //

	private void anyDoorEntry(Vector3 door, Vector3 mousePosition, Vector3Int playerPosition, string scene){
        if(Input.GetMouseButtonDown(0)){
            if(door.y - 1 == playerPosition.y && door.x == playerPosition.x){
                if(mousePosition == door){
                    sceneFader.fadeToLevel(scene);
                }
            }
		}
    }

    private void allAccessDoors(Vector3Int mousePosition, Vector3Int playerPosition){
        anyDoorEntry(new Vector3Int(-13, -3, 0), mousePosition, playerPosition, "TownHall");
    }


	 // VECTOR TOOL
    private void vectorLog(Vector3Int mousePosition, Vector3Int playerPosition, Vector3 mousePoint, Vector3 rawPlayerPosition){
        if(Input.GetMouseButtonDown(0)){
            Debug.Log(mousePosition);
            Debug.Log(Camera.main.WorldToScreenPoint(mousePoint));
            Debug.Log(playerPosition);
            Debug.Log(rawPlayerPosition);
        }
    }
}
