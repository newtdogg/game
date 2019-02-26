using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(() => startGame());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startGame(){
        SceneManager.LoadScene("main_world");
    }
}
