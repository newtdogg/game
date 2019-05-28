using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorIntro : MonoBehaviour {
    private BoxCollider2D collider;
    private GameObject player;
    public string scene;
    public Vector3Int nextScenePosition;
    private SceneFadeout sceneFader;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        collider = GetComponent<BoxCollider2D>();
        sceneFader = GameObject.Find("SceneFadeout").GetComponent<SceneFadeout>();
    }

    // Update is called once per frame

    void OnTriggerEnter2D(Collider2D collision){
        nextScene(scene, nextScenePosition);
    }

    public void nextScene(string scene, Vector3Int position){
        sceneFader.fadeToLevel(scene);
        Debug.Log(position);
        Debug.Log(scene);
    }
    // void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode){
    //     // player.transform.position = nextScenePosition;
    // }

    // void OnEnable(){
    //     SceneManager.sceneLoaded += OnLevelFinishedLoading;
    // }
         
    // void OnDisable(){
    //     SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    // }

}
