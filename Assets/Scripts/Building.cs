using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public string type;
    public List<Vector3> tilePositions;
    public NPC assignedNPC;
    public string npcName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(assignedNPC != null){
            npcName = assignedNPC.name;
        }
    }
}
