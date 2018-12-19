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
		
	}
    void OnMouseDown()
    {
        var dist = Vector3.Distance(gameObject.transform.position, player.transform.position);
        if(dist < 0.8) {
            controller.crateType = this.type;
            controller.holdingCrate = true;
            controller.heldCrate = this.gameObject;
            gameObject.transform.position = new Vector3(1000, 0, 0);
        }
    }
}
