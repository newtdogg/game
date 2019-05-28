using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireTownhall : MonoBehaviour
{
    public float MaxReduction;
	public float Min;
	public float Max;
	public float Strength;
	public bool increasing;
    private Light lightSource;
    private theMaster mainNpc;
    private PlayerController playerController;

    void Start()
    {
        Min = 10f;
        Max = 40f;
        increasing = true;
        Strength = 25;
        lightSource = gameObject.transform.GetChild(0).gameObject.GetComponent<Light>();
        mainNpc = GameObject.Find("theMaster").GetComponent<theMaster>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update(){
        Flickering();
        if(Vector3.Distance(playerController.transform.position, transform.position) < 0.6 && mainNpc.introTalk == false) {
			mainNpc.dialogueManager.StartDialogue(mainNpc.sentences["byfire"]);
			mainNpc.introTalk = true;
        }
    }

	private void Flickering(){
		if (increasing && lightSource.intensity < Max){
			lightSource.intensity += 0.3f;
		}
		if (increasing && lightSource.intensity >= Max){
			increasing = false;
			System.Random r = new System.Random();
			Max = r.Next(25, 45);
		}
		if (!increasing && lightSource.intensity > Min){
			lightSource.intensity -= 0.3f;
		}
		if (!increasing && lightSource.intensity <= Min){
			increasing = true;
			System.Random r = new System.Random();
			Max = r.Next(10, 25);
		}
	}

}
