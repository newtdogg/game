using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class QuestManagerObject : MonoBehaviour
{
    // Start is called before the first frame update
    public QuestManager questManager;
    private GameObject questCanvas;
    public List<Text> questCanvasList;
    public GameObject questButtonObj;
    private Button questButton;
    public Image questButtonImage;
    void Start()
    {
        questManager = new QuestManager();
        questCanvas = gameObject;
        questManager.populateQuests();
        questCanvasList = new List<Text>();
        questCanvasList.Add(questCanvas.transform.GetChild(0).GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>());
        questCanvasList.Add(questCanvas.transform.GetChild(0).GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<Text>());
        questCanvasList.Add(questCanvas.transform.GetChild(0).GetChild(0).gameObject.transform.GetChild(2).gameObject.GetComponent<Text>());
        setBulletPoints(questCanvasList);
        questCanvas.transform.GetChild(0).gameObject.SetActive(false);

        questButtonObj = gameObject.transform.GetChild(1).gameObject;
        questButtonObj.SetActive(false);
        questButton = questButtonObj.GetComponent<Button>();
        questButtonImage = questButtonObj.GetComponent<Image>();
        questButton.onClick.AddListener(() => questcanvasDisplayToggle());
        
    }

    // Update is called once per frame
    void Update()
    {
        toggleQuestCanvas();
    }

    private void setBulletPoints(List<Text> quests){
        foreach(Text quest in quests){
            if(quest.text == ""){
                quest.transform.GetChild(0).gameObject.SetActive(false);
            } else {
                quest.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    private void toggleQuestCanvas(){
        if(Input.GetKeyDown(KeyCode.Tab)){
            questcanvasDisplayToggle();
        }
    }

    private void questcanvasDisplayToggle(){
        setBulletPoints(questCanvasList);
        if(questManager.notification == true){
            questButtonImage.color = Color.clear;
            questManager.notification = false;
        }
        if(questCanvas.transform.GetChild(0).gameObject.active == true){
            questCanvas.transform.GetChild(0).gameObject.SetActive(false);
        } else {
            questCanvas.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
