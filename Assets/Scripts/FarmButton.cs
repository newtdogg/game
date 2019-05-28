// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.SceneManagement;


// public class FarmButton : MonoBehaviour {

// 	public Button button;
// 	public GameObject buildingLayer;
// 	public GameObject buildingTilemap;
// 	public TestOurTile buildingTileController;
// 	public string buttonType;
// 	// Use this for initialization
// 	void Start () {
// 		button = gameObject.GetComponent<Button>();
// 		button.onClick.AddListener(TaskOnClick);
// 		buttonType = gameObject.GetComponentInChildren<Text>().text;
// 		buildingLayer = GameObject.Find("BuildingPlacingMap");
//         buildingTileController = buildingLayer.GetComponent<TestOurTile>();
// 	}
	
// 	// Update is called once per frame
// 	void Update () {
		
// 	}

	// void TaskOnClick()
	// {
    //     Debug.Log("You have clicked the button!");
	// 	buildingTileController.tileType = buttonType;
	// 	buildingTileController.status = "active";
		
	// 	// buildingLayer = GameObject.Find("BuildingPlacingMap");
    //     // buildingLayer.SetActive(true);
    // }
// }
