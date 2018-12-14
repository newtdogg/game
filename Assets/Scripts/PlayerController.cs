using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour {

    // Use this for initialization
    public string crateType;
    public bool holdingCrate;
    public bool pickingUpCrate;
    public GameObject crateObject;

    // Use this for initialization
    void Start()
    {
        crateType = "Empty";
        crateObject = GameObject.Find("CrateClone");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && holdingCrate == true){
            var currentPosition = this.gameObject.transform.position;
            GameObject newCrate = Instantiate(crateObject);
            newCrate.transform.position = currentPosition;
            holdingCrate = false;       
        }
    }

   
}