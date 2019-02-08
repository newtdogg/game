using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class theMaster : MonoBehaviour
{

	private Dialogue dialogue;
	private PlayerController playerController;
	private DialogueManager dialogueManager;
	private string[] sentences;
	private int sentenceIndex;
	private GameObject canvas;
	private Vector3Int beam;
	
	void Start() {
		dialogue = new Dialogue();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
		dialogueManager = GameObject.Find("Dialogue").GetComponent<DialogueManager>();
		dialogueManager.hideDialogue();
		beam = new Vector3Int(-1, 0, 0);
	}

	void Update() {
		TriggerDialogue();
	}

	public void TriggerDialogue(){
		if(playerController.checkInPosition(beam) == true){
			
			playerController.questManager.Quests["Quest1"]["status"] = "complete";
			dialogueManager.StartDialogue(dialogue);
			// dialogue.dialogueComplete = false;

		}
		if(playerController.alreadyInPosition[beam] == true && dialogue.dialogueComplete == false){
			dialogueManager.continueDialogue(dialogue);
		}
		

	}
}
