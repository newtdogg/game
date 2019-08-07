using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

using UnityEngine;

public class Building : MonoBehaviour
{
    public string type;
    public List<Vector3> tilePositions;
    public Vector3 mainTilePosition;
    private EventController eventController;
    public GameObject crateObject;
    public PlayerController playerController;
    public System.Random ran = new System.Random();
    public NPC assignedNPC;
    private Tilemap boundariesTilemap;

    private Tilemap treesTilemap;

    public Dictionary<string, int> dimensions;
    public Dictionary<string, int> spawnChance;
    public Dictionary<string, Resource> resources;
    public string resourceType;
    public GameObject player;
    public int crateCount;
    public string npcName;
    public bool cutTree;
    // Start is called before the first frame update
    void Start()
    {
        crateCount = 0;
        crateObject = GameObject.Find("CrateClone");
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        eventController = GameObject.Find("EventControllerCanvas").GetComponent<EventController>();
        boundariesTilemap = GameObject.Find("Grid").transform.GetChild(1).gameObject.GetComponent<Tilemap>();
        treesTilemap = GameObject.Find("Grid").transform.GetChild(5).gameObject.GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        if(assignedNPC != null){
            npcName = assignedNPC.name;
            generateCrates();
            removeTrees();
        }
    }

    private void generateCrates() {
        foreach(KeyValuePair<string, int> resource in spawnChance){
            int num = ran.Next(1, resource.Value * 100 * 2);
            if(num == 1){
                Debug.Log("crate");
                var newCrate = Instantiate(crateObject, new Vector3 (tilePositions[0].x, tilePositions[0].y -1, 0), Quaternion.Euler(0,0,0));
                var crate = newCrate.GetComponent<Crate>();
                crate.player = player;
                crate.eventController = eventController;
                crate.controller = playerController;
                Debug.Log(type);
                crateCount += 1;
                if(crateCount % 5 == 0) {
                    cutTree = true;
                }
                crate.value = resources[resource.Key].value;
                crate.type = resource.Key;
                newCrate.GetComponent<SpriteRenderer>().sortingOrder = 10;
            }
        }       
    }

    private void removeTrees() {
        if(type == "woodcutters hut") {
            if(cutTree == true) {
                for(var y = -3; y < (dimensions["height"] + 3); y++){
                    for(var x = -3; x < (dimensions["width"] + 3); x++){
                        var tiles = GameTiles.instance.tiles;
                        var worldPoint = tilePositions[0];
                        var tileVector = new Vector3Int((int) worldPoint.x + x,(int) worldPoint.y + y, 0);
                        Debug.Log(tileVector);
                        var worldTile = tiles[tileVector];
                        if (worldTile.Type == "tree"){
                            // returns class Tile at given coords
                            worldTile.Type = "grass";
                            treesTilemap.SetTile(tileVector, null);
                            boundariesTilemap.SetTile(tileVector, null);
                            cutTree = false;
                            // worldTile.TilemapMember.SetTileFlags(worldTile.LocalPlace, TileFlags.None);
                            return;
                        }
                    }
                }
            }
        }
    }
}
