using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class QuestManager
{
	public string name;

	public int sentenceIndex;
    public Dictionary<string, Dictionary<string, string>> Quests;
    public Dictionary<string, string> currentQuest;
    // public Dictionary<string, string> QuestInfo;
    


	public string[] sentences;

	public void StartQuest1(Text textBox){
		textBox.text = Quests["Quest1"]["title"];
        currentQuest = Quests["Quest1"];
	}
	public void populateQuests(){
		Quests = new Dictionary<string, Dictionary<string, string>>();
		createQuest("Quest1", "incomplete", "Talk to beam");
		createQuest("Quest2", "incomplete", "Leave the town hall");
	}

	public void questsCompleteCheck(Text textBox){
		for(var i = 1; i < Quests.Count; i++){
			var quest1 = $"Quest{i}";
			var quest2 = $"Quest{i + 1}";
			if(Quests[quest1]["status"] == "complete" && Quests[quest2]["status"] == "incomplete"){
				currentQuest =  Quests[quest2];
				textBox.text = currentQuest["title"];
			}
		}

		// foreach(KeyValuePair<string, Dictionary<string, string>> quest in Quests){
		// 	if(quest["status"] == complete)
		// }
	}
	public void createQuest(string name, string status, string title){
		var Quest = new Dictionary<string, string>();
        Quest.Add("title", title);
        Quest.Add("status", status);
        Quests.Add(name, Quest);
	}

}