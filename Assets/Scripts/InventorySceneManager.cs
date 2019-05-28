using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySceneManager : MonoBehaviour
{
    public int inventoryActive;
    private static InventorySceneManager inventorySceneManager;
    void Awake() {
        if(inventorySceneManager == null){
            DontDestroyOnLoad(gameObject);
            inventorySceneManager = this;
        }
        else if (inventorySceneManager != this){
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        setActiveUI(inventoryActive);
    }

    private void setActiveUI(int inventoryIndex) {
        Transform childTransform = gameObject.transform.GetChild(0);
        foreach (Transform child in childTransform) {
            child.gameObject.SetActive(false);
        }
        childTransform.GetChild(inventoryIndex).gameObject.SetActive(true);
    }
}
