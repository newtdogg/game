using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBasicIntro : MonoBehaviour
{
	public bool canMove;
	public Animator anim;
	public Animator hairAnim;
    public Animator bodyAnim;
    public Animator clothesAnim;
    public Animator legsAnim;
	private Rigidbody2D rbody;
    private InventorySceneManager inventorySceneManager;
    public GameObject inventoryActive;
    private GameObject inventory;
    // private bool ;
    public int maxSlots;
    public int inventorySlot;
	private List<Vector3Int> exit;
    private SceneFadeout sceneFader;
    // Start is called before the first frame update
    void Start()
    {
        sceneFader = GameObject.Find("SceneFadeout").GetComponent<SceneFadeout>();
		rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.SetFloat("runMultiplyer", 0.8f);
        hairAnim = transform.GetChild(3).gameObject.GetComponent<Animator>();
        bodyAnim = transform.GetChild(4).gameObject.GetComponent<Animator>();
        clothesAnim = transform.GetChild(5).gameObject.GetComponent<Animator>();
        legsAnim = transform.GetChild(6).gameObject.GetComponent<Animator>();
		canMove = true;
		exit = new List<Vector3Int>(new Vector3Int[] {
            new Vector3Int(-27, -1, 0),
            new Vector3Int(-27, 0, 0),
            new Vector3Int(-27, 1, 0),
            new Vector3Int(-27, 2, 0),
            new Vector3Int(-27, 3, 0)
        });
        inventory = gameObject.transform.GetChild(1).gameObject;
        inventoryActive = inventory.transform.GetChild(0).gameObject;
        inventorySceneManager = GameObject.Find("InventorySceneManager").GetComponent<InventorySceneManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
		Vector2 movement_vector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        playerAnimation(movement_vector);
		var point = gameObject.transform.localPosition;
        var playerPosition = new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), 0);
		checkExit(playerPosition);
        inventoryScroll();
        inventorySceneManager.inventoryActive = inventoryActive.transform.GetSiblingIndex();
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

    private void checkExit(Vector3Int pos){
        foreach(var vec in exit){
            if(vec == pos){
                inventorySceneManager.inventoryActive = inventoryActive.transform.GetSiblingIndex();
                sceneFader.fadeToLevel("introductionMainWorld");
            }
        }
    }

}
