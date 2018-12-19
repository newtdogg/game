using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
    Animator anim;
    public Vector3 lastPosition;
    public float stamina;
    public float maxStamina;
    public int walkSpeed;
    public int runSpeed;
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
    }

    // Update is called once per frame
    void Update()
    {
        var currentPosition = this.gameObject.transform.position;
        nearestCrate = GetClosestCrate(crates);
        if (Input.GetMouseButtonDown(1) && holdingCrate == true){
            Debug.Log(lastPosition.x);
            Debug.Log(lastPosition.y);
            heldCrate.transform.position = new Vector3(currentPosition.x + (lastPosition.x/2), currentPosition.y + (lastPosition.y/2) - 0.2f, 0);
            holdingCrate = false;
            heldCrate = null;
        }

        Vector2 movement_vector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        playerAnimation(movement_vector);
        getLastPosition(movement_vector);
        nearCrateLayerOrder(nearestCrate);
        staminaCalculator();
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

    private void playerAnimation(Vector2 movement_vector){
        if (movement_vector != Vector2.zero)
        {
            anim.SetBool("isMoving", true);
            anim.SetFloat("input_X", movement_vector.x);
            anim.SetFloat("input_Y", movement_vector.y);
        } else
        {
            anim.SetBool("isMoving", false);
        }
        var speed = movementSpeed();
        Debug.Log(speed);
        Debug.Log(isRunning());
        rbody.MovePosition(rbody.position + movement_vector * Time.deltaTime * speed);
    }

    public void getLastPosition(Vector2 movement_vector){
        if(movement_vector.x != 0 || movement_vector.y != 0){
            lastPosition.x = movement_vector.x;
            lastPosition.y = movement_vector.y;
        }
    }
 
    private void nearCrateLayerOrder(GameObject crate){
        var dist = Vector3.Distance(gameObject.transform.position, crate.transform.position);
        if(dist < 0.8) {
            if(gameObject.transform.position.y > crate.transform.position.y){
                if(aboveCrate != true){
                    var sprite = crate.GetComponent<SpriteRenderer>();
                    sprite.sortingOrder = 10;
                    aboveCrate = true;
                }
            }
            if(gameObject.transform.position.y < crate.transform.position.y){
                if(aboveCrate == true){
                    var sprite = crate.GetComponent<SpriteRenderer>();
                    sprite.sortingOrder = 5;
                    aboveCrate = false;
                }
            }
        }
    }

    private bool isRunning(){
        if (Input.GetKey("space") && stamina > 0){
            Debug.Log(stamina);
            return true;
        }
        return false;
    }

    private void staminaCalculator(){
        if (isRunning()){
            stamina -= Time.deltaTime;
                if (stamina < 0) {
                stamina = 0;
            }
        }
        // else if (stamina < maxStamina) {
        //     stamina += Time.deltaTime;
        // }
    }

    private int movementSpeed(){
        walkSpeed = 2;
        runSpeed = 4;
        if(isRunning()){
            return runSpeed;
        }
        return walkSpeed;
    }
   
}