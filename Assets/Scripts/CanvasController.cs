using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public static CanvasController controller;

    void Awake() {
        if(controller == null){
            DontDestroyOnLoad(gameObject);
            controller = this;
        }
        else if (controller != this){
            Destroy(gameObject);
        }
    }
}
