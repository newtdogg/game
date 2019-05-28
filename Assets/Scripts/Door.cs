using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
    private BoxCollider2D collider;
    public string scene;
    public Vector3Int nextScenePosition;
    private EventController eventController;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        eventController = GameObject.Find("EventControllerCanvas").GetComponent<EventController>();
    }

    // Update is called once per frame

    void OnTriggerEnter2D(Collider2D collision){
        eventController.nextScene(scene, nextScenePosition);
    }

}
