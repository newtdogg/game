using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CrateDebugger : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject crate;
    private Crate crateScript;
    private Text type;
    private PlayerController playerController;
    private Text value;
    public Image notification1;
    public Image notification2;
    public bool newCrate;

    
    
    void Start()
    {
        crate = gameObject.transform.parent.gameObject;
        crateScript = gameObject.transform.parent.gameObject.GetComponent<Crate>();
        type = gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
        value = gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<Text>();
        notification1 = gameObject.transform.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>();
        notification2 = gameObject.transform.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Image>();
        notify();
    }

    public void notify(){
        notification1.color = Color.yellow;
        notification2.color = Color.yellow;
        newCrate = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 pos = Camera.main.WorldToScreenPoint(crate.transform.position);
        
        // gameObject.transform.GetChild(0).gameObject.transform.position = pos;
        
        // type.text = $"Type: {crateScript.type}";
        // Debug.Log(crateScript.value);
        // value.text = $"Value: { Mathf.Round(crateScript.value)}";
        // Debug.Log($"crate{pos}");
        // Debug.Log($"crate{gameObject.transform.GetChild(0).gameObject.transform.localPosition}");
    }
}
