using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class QuestManager
{
	public string name;

	public int sentenceIndex;
	public bool notification;
	public int questNum;
    public Dictionary<string, Dictionary<string, string>> Quests;
    // public Dictionary<string, string> QuestInfo;
    


	public string[] sentences;

	public void startQuest(Image buttonImage, List<Text> textBoxes, string quest){
		notification = true;
		buttonImage.color = Color.red;
		Quests[quest]["status"] = "active";
		Debug.Log($"started quest: {quest}");
		multiQuestDisplay(textBoxes);
	}

	public void multiQuestDisplay(List<Text> QuestBoxes){
		Debug.Log(QuestBoxes.Count);
		int count = 0;
		foreach(KeyValuePair<string, Dictionary<string, string>> kvp in  Quests ){
			if(kvp.Value["status"] == "active"){
				if(QuestBoxes[count]){
					Debug.Log(kvp.Value["status"]);
					QuestBoxes[count].text = kvp.Value["title"];
					count += 1;
				}
			}	
		}
	}
	public void populateQuests(){
		Quests = new Dictionary<string, Dictionary<string, string>>();
		
		// REAL QUESTS 

		// createQuest("InvestigateBuilding", "incomplete", "Investigate the old building");
		// createQuest("Sleep", "incomplete", "Find somewhere to rest in the Town Hall");
		// createQuest("Explore", "incomplete", "Explore");
		// createQuest("RebuildStockpile", "incomplete", "Rebuild the stockpile");
		// createQuest("dud", "incomplete", "Blah blah blah");
		// createQuest("DeliverCrate", "incomplete", "Deliver the crate to the stockpile");

		// QUESTS FOR TESTING 
		createQuest("SpeakToBarry", "incomplete", "");
		createQuest("Explore", "incomplete", "Explore");
		createQuest("RebuildStockpile", "incomplete", "Rebuild the stockpile");
		createQuest("dud", "incomplete", "Blah blah blah");
		createQuest("DeliverCrate", "incomplete", "Deliver the crate to the stockpile");
		createQuest("TalktoNPC", "incomplete", "Talk to the new arrival");
		createQuest("BuildFarm", "incomplete", "Build a farm");
	}

	public void completeQuest(Image buttonImage, List<Text> textBoxes, string quest){
		Quests[quest]["status"]= "complete";
		buttonImage.color = Color.green;
		notification = true;
		multiQuestDisplay(textBoxes);
		Debug.Log($"completed quest: {quest}");
	}

	public void createQuest(string name, string status, string title){
		var Quest = new Dictionary<string, string>();
        Quest.Add("title", title);
        Quest.Add("status", status);
        Quests.Add(name, Quest);
	}

}