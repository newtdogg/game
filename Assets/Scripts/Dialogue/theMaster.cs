using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class theMaster : MonoBehaviour
{

	private PlayerController playerController;
	public DialogueManager dialogueManager;
	public Dictionary<string, string[]> sentences;
	private GameObject canvas;
	public bool introTalk;
	private Vector3Int beam;
	public GameManager gameManager;

	void Start() {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
		dialogueManager = GameObject.Find("Dialogue").GetComponent<DialogueManager>();
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		beam = new Vector3Int(-1, 0, 0);
		populateDialogue();
		introTalk = false;
		dialogueManager.StartDialogue(sentences["introduction"]);
	}

	void Update() {
		TriggerDialogue();
	}

	public void TriggerDialogue(){
		// if(playerController.checkInPosition(beam) == true){
		// 	gameManager.questManager.completeQuest(gameManager.questButtonImage, gameManager.questCanvasList, "InvestigateBuilding");
		// 	gameManager.questManager.startQuest(gameManager.questCanvasList, "Sleep");
		// 	dialogueManager.StartDialogue(sentences["underbeam"]);

		// }
		// if(playerController.alreadyInPosition[beam] == true && dialogueManager.inDialogue == true){
		// }


	}

	private void populateDialogue(){
        sentences = new Dictionary<string, string[]>{
			{ 
				"introduction", 
				new string[] { 
					"Hello...?", 
					"Come stand under the light so I can get a better look at you"
				} 
			},
            { 
				"byfire", 
				new string[] { 
					"Another traveller displaced by the great war?", "hmm...", 
					"This is all that remains of what was a once a thriving town, now abandoned", 
					"We had trade connections with civilisations all over the world", 
					"But enough about that…",
					"You look like you could do with a rest from the cold and dark",
					"Feel free to sleep on that bundle of hay in the corner"
				} 
			},
            { "max", new string[] { "test", "hello"} }
        };
    }
}
