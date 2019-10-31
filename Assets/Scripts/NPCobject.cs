using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCobject : MonoBehaviour {
    public float oX;
    public float oY;
    public float nX;
    public float nY;
    public float xDir;
    public float yDir;
    public bool isMoving;
    public string name;
    public Rigidbody2D rbody;
    public NPC npc;
    public bool inContact;
    public string collisionSide;
    public int speed;
    public Vector2 movementDirection;
    public float lastColliderMove;
    public GameManager gameManager;
    private bool wasInContact;
    private string currentCollidingObject;

    public GameObject crate;
    public GameObject nearestCrate;
    public bool holdingCrate;
    public EventController eventController;

    public void npcMovement(Rigidbody2D rbody) {
        if(!isMoving) {
            var crates = Object.FindObjectsOfType<Crate>();
            // Debug.Log(crates.Length);
            if (crates.Length > 1) {
                float minDist = Mathf.Infinity;
                Vector3 currentPos = transform.position;
                inContact = false;
                foreach (Crate t in crates){
                    float dist = Vector3.Distance(t.gameObject.transform.position, currentPos);
                    if (dist < minDist){
                        nearestCrate = t.gameObject;
                        minDist = dist;
                        isMoving = true;
                    }
                }
            }
        }
        if(isMoving && !holdingCrate) {
            movementToObject(nearestCrate.transform.position, rbody);
        }
        if(isMoving && holdingCrate) {
            movementToObject(new Vector2(25f, 23f), rbody);
        }
    }

    void OnMouseDown() {
        Debug.Log(name);
        Debug.Log(eventController);
        eventController.npcUILoad(npc, gameObject.GetComponent<SpriteRenderer>().sprite);
        // dialogueManager.StartDialogue(sentences["introduction"]);
        // if(eventController.gameManager.questManager.Quests["TalktoNPC"]["status"] == "active"){
        //     eventController.questmarkActive("SpeakToNPC");
        //     eventController.gameManager.questManager.completeQuest(eventController.gameManager.questButtonImage, eventController.gameManager.questCanvasList, "TalktoNPC");
        // }
    }

    public void movementToObject(Vector2 obj, Rigidbody2D rbody) {
        // rounds the positon floats 
        if(!inContact) {
            oX = Mathf.Round(obj.x * 10f) / 10f;
            oY = Mathf.Round(obj.y * 10f) / 10f;
            nX = Mathf.Round(transform.position.x * 10f) / 10f;
            nY = Mathf.Round(transform.position.y * 10f) / 10f;
        }

        // if(!inContact) {
        //     oX = Mathf.RoundToInt(obj.x);
        //     oY = Mathf.RoundToInt(obj.y);
        //     nX = Mathf.RoundToInt(transform.position.x);
        //     nY = Mathf.RoundToInt(transform.position.y);
        // }
        
        xDir = nX < oX ? 0.1f : -0.1f;
        yDir = nY < oY ? 0.1f : -0.1f;

        var primaryDirection = (Mathf.Abs(oX) - Mathf.Abs(nX)) > (Mathf.Abs(oY) - Mathf.Abs(nY)) ? "x" : "y";
        
        // Debug.Log(collisionSide);
        // Debug.Log(currentCollidingObject);
        if(inContact) {
            if (collisionSide == "x") {
                movementDirection = new Vector2(0, yDir);
                // lastColliderMove = yDir;
                wasInContact = true;
                // Debug.Log("in contact, moving up/down");
            } else if (collisionSide == "y") {
                movementDirection = new Vector2(xDir, 0);
                // lastColliderMove = xDir;
                wasInContact = true;
                // Debug.Log("in contact, moving right/left");
            }
        } else {
            movementDirection = new Vector2(xDir, yDir);
            // lastDirection = primaryDirection;
            if(nX == oX) {
                movementDirection = new Vector2(0, yDir);
                // lastDirection = "y";
                // Debug.Log("level with x, moving up/down");
            } else if (nY == oY ) {
                movementDirection = new Vector2(xDir, 0);
                // lastDirection = "x";
                // Debug.Log("level with y, moving right/left");
            }
        }
        rbody.MovePosition(rbody.position + movementDirection * Time.deltaTime * speed * gameManager.gameSpeed);
    }

    // void OnTriggerEnter(Collision collision) {
    //     // Debug.Log(collision.contacts[0].point);
    // }
    void OnTriggerEnter2D (Collider2D col) {
        var colliderName = col.gameObject.name;
        if(colliderName != "Background"){
            if(colliderName == "CrateClone(Clone)" && !holdingCrate) {
                Debug.Log("pisssss");
                holdingCrate = true;
                crate = nearestCrate;
                crate.transform.position = new Vector3(1000, 0, 0);
            }
            if(colliderName == "StockpileDrop" && holdingCrate) {
                holdingCrate = false;
                isMoving = false;
                nearestCrate = null;
                var heldCrateScript = crate.GetComponent<Crate>();
                gameManager.stockpile.stats[heldCrateScript.type].value += heldCrateScript.value;
                gameManager.stockpile.surplusRate();
                Destroy(crate);
                rbody.MovePosition(rbody.position + new Vector2(xDir * -1, yDir * -1) * Time.deltaTime * speed);
            }
            // Debug.Log(colliderName);
            if(colliderName == "NPCclone(Clone)" || colliderName == "StockpileDrop" || colliderName == "Stockpile"){
                inContact = false;
            } else {
                inContact = true;
                var boundaries = new Dictionary <float, string> {
                    { col.bounds.max.y, "y" },
                    { col.bounds.max.x, "x" },
                    { col.bounds.min.y, "y" },
                    { col.bounds.min.x, "x" }
                }; 
                Debug.Log(col.bounds.min);
                Debug.Log(col.bounds.max);
                var sides = new float[] { transform.position.y + 0.6f, transform.position.x + 0.6f, transform.position.y - 0.6f, transform.position.x + 0.6f };
                var sideKeyFloat = 1000f;
                var count = 0;
                foreach (var boundary in boundaries){
                    if (Mathf.Abs(boundary.Key - sides[count]) < sideKeyFloat) {
                        sideKeyFloat = boundary.Key;
                        Debug.Log(sideKeyFloat);
                    }
                    count += 1;
                }
                collisionSide = boundaries[sideKeyFloat];
                if (colliderName == currentCollidingObject) {
                    collisionSide = collisionSide == "y" ? "x" : "y";
                    Debug.Log("same col lol");
                }
            }
            currentCollidingObject = colliderName;
           
        }
    }

    
 
    void OnTriggerExit2D (Collider2D col) {
        Debug.Log("out");
        inContact = false;
        // lastDirection = lastDirection == "y" ? "x" : "y";
        collisionSide = ""; 
    }
}