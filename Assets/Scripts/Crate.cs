using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour {

    public GameObject player;
    private PlayerController controller;
    string type;

    void Start () {
        player = GameObject.Find("Player");
        controller = player.GetComponent<PlayerController>();
        type = "food";
    }
	
	// Update is called once per frame
	void Update () {
		nearCrateLayerOrder();
	}

    private void nearCrateLayerOrder(){
        var dist = Vector3.Distance(player.transform.position, gameObject.transform.position);
        if(dist < 0.6) {
            if(player.transform.position.y > gameObject.transform.position.y + 0.4f){
                var sprite = gameObject.GetComponent<SpriteRenderer>();
                sprite.sortingOrder = 10;
                Debug.Log("1");
            }
            if(player.transform.position.y < gameObject.transform.position.y + 0.4f){
                var sprite = gameObject.GetComponent<SpriteRenderer>();
                sprite.sortingOrder = 3;
                Debug.Log("2");
            }
        }
    }
    
    void OnMouseDown()
    {
        var dist = Vector3.Distance(gameObject.transform.position, player.transform.position);
        if(dist < 1) {
            controller.crateType = this.type;
            controller.holdingCrate = true;
            controller.heldCrate = this.gameObject;
            gameObject.transform.position = new Vector3(1000, 0, 0);
        }
    }
}
