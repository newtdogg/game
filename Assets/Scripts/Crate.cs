using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Crate : MonoBehaviour {

    public GameObject player;
    private PlayerController controller;
    private SpriteRenderer crateSpriteRen;
    private EventController eventController;
    public string type;
    public float value;
    public float decayRate;

    void Start () {
        eventController = GameObject.Find("EventControllerCanvas").GetComponent<EventController>();
        player = GameObject.Find("Player");
        controller = player.GetComponent<PlayerController>();
        crateSpriteRen = gameObject.GetComponent<SpriteRenderer>();
        type = "food";
        value = 5000;
        decayRate = 3;
    }
	
	// Update is called once per frame
	void Update () {
        crateSpriteRen.sortingOrder = 200 - Mathf.RoundToInt(gameObject.transform.position.y * 10);
        crateValue();
        if(gameObject.transform.GetChild(0).GetChild(0).gameObject.active == true){
            Vector3 pos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            gameObject.transform.GetChild(0).GetChild(0).gameObject.transform.position = setNotification(pos);
        } 
	}

    private void nearCrateLayerOrder(){
        var dist = Vector3.Distance(player.transform.position, gameObject.transform.position);
        if(dist < 0.6) {
            if(player.transform.position.y > gameObject.transform.position.y + 0.4f){
                var sprite = gameObject.GetComponent<SpriteRenderer>();
                sprite.sortingOrder = 10;
            }
            if(player.transform.position.y < gameObject.transform.position.y + 0.4f){
                var sprite = gameObject.GetComponent<SpriteRenderer>();
                sprite.sortingOrder = 3;
            }
        }
        if(controller.holdingCrate == true) {
            controller.gameObject.transform.GetChild(4).gameObject.transform.position = controller.gameObject.transform.position;
        }
    }

    public Vector3 setNotification(Vector3 defaultPos){
        float crateX = gameObject.transform.position.x;
        float crateY = gameObject.transform.position.y;
        float playerX = player.transform.position.x;
        float playerY = player.transform.position.y;
        float diffX = crateX - playerX;
        float diffY = crateY - playerY;
        float EoSX = 6.9f;
        float EoSY = 3.8f;
        float gradient = diffY/diffX;
        // if(diffY > EoSY && diffX > EoSX){
        //     return Camera.main.WorldToScreenPoint(new Vector3(playerX + EoSX, playerY + EoSY, 0));
        // }
        // if(-diffY < -EoSY && diffX > EoSX){
        //     return Camera.main.WorldToScreenPoint(new Vector3(playerX + EoSX, playerY - EoSY, 0));
        // }
        // if(diffY < EoSY && -diffX < -EoSX){
        //     return Camera.main.WorldToScreenPoint(new Vector3(playerX - EoSX, playerY + EoSY, 0));
        // }
        // if(-diffY < -EoSY && -diffX < -EoSX){
        //     return Camera.main.WorldToScreenPoint(new Vector3(playerX - EoSX, playerY - EoSY, 0));
        // }
        if((diffY > 0 && diffY > EoSY && diffY > diffX)){
            var pos = new Vector3(playerX + (EoSX * gradient), playerY + EoSY, 0);
            // Debug.Log("Y+");
            return Camera.main.WorldToScreenPoint(pos);
        }
        if((diffX > 0 && diffX > EoSX && diffX > diffY)){
            var pos = new Vector3(playerX + EoSX, playerY + (EoSY * gradient), 0);
            // Debug.Log("X+");
            return Camera.main.WorldToScreenPoint(pos);
        }
        if((diffY < 0 && diffY < -EoSY && diffY < diffX)){
            var pos = new Vector3(playerX - (EoSX/gradient), playerY - EoSY, 0);
            // Debug.Log("Y-");
            return Camera.main.WorldToScreenPoint(pos);
        }
        if((diffX < 0 && diffX < -EoSX && diffX < diffY)){
            var pos = new Vector3(playerX - EoSX, playerY - (EoSY * gradient), 0);
            // Debug.Log("X-");
            return Camera.main.WorldToScreenPoint(pos);
        }
        
        return defaultPos;

    }
    
    private void crateValue(){
        value -= Time.deltaTime/decayRate;
    }
    void OnMouseDown()
    {
        var dist = Vector3.Distance(gameObject.transform.position, player.transform.position);
        if(dist < 1) {
            eventController.timer += Time.deltaTime;
            controller.crateType = type;
            controller.holdingCrate = true;
            controller.heldCrate = gameObject;
            controller.crateContentSprites = Resources.LoadAll<Sprite>($"CrateContents/{type}");
            player.transform.GetChild(4).gameObject.SetActive(true);
            controller.heldCrateScript = gameObject.GetComponent<Crate>();
            gameObject.transform.position = new Vector3(1000, 0, 0);
            gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
