using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
	public string name;
	public int sentenceIndex;
	public bool dialogueComplete;
	public bool inDialogue;

	[TextArea(3, 10)]
	public string[] sentences;

	public Dialogue()
   {
      populateDialogue();
   }

	public void StartDialogue(Text textBox){
		sentenceIndex = 0;
		textBox.text = sentences[sentenceIndex];
		inDialogue = true;
	}

	public void nextSentence(Text textBox){
		if(Input.GetMouseButtonDown(0)){
				if(sentenceIndex == sentences.Length - 1){
					dialogueComplete = true;
					textBox.transform.parent.gameObject.SetActive(false);
					inDialogue = false;
					return;
					
				}
				sentenceIndex += 1;
				textBox.text = sentences[sentenceIndex];				
			}
	}

	public void populateDialogue(){
		sentences = new string[3];
		sentences[0] = "deer";
        sentences[1] = "moose";
        sentences[2] = "boars";
	}
}
