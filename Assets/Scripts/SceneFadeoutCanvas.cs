using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFadeoutCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    public static SceneFadeoutCanvas controller;

    void Awake() {
        if(controller == null){
            DontDestroyOnLoad(gameObject);
            controller = this;
        }
        else if (controller != this){
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
