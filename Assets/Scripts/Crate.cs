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
        controller.crateType = this.type;
        controller.holdingCrate = true;
        controller.pickingUpCrate = true;
        Destroy(gameObject);
    }
}
