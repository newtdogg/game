using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine;

public class BuildingObject : MonoBehaviour
{
    public List<Vector3> tilePositions;
    public Vector3 mainTilePosition;
    private EventController eventController;
    public GameObject crateObject;
    public PlayerController playerController;
    public NPC assignedNPC;
    private Tilemap boundariesTilemap;
    public Building building;
    public Dictionary <float, string> boundaries;
    private Tilemap treesTilemap;

    public Dictionary<string, int> dimensions;
    public string resourceType;
    private GameObject buildingCanvas;
    public GameObject player;
    public string bonusStat;
    public BList upgradeList;
    public string statBonusOrdering;
    public Dictionary<string, int> spawnChance;

    public int crateCount;
    public string npcName;
    public bool cutTree;
    // Start is called before the first frame update
    void Start()
    {
        // building = new Building("", "", "", "", true, 1, 1, new Dictionary<string, int>{});
        // building.spawnChance = new Dictionary<string, int>{};
        // building.upgradesTo = new List<string>(){};

        crateCount = 0;
        crateObject = GameObject.Find("CrateClone");
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        eventController = GameObject.Find("EventControllerCanvas").GetComponent<EventController>();
        boundariesTilemap = GameObject.Find("Grid").transform.GetChild(1).gameObject.GetComponent<Tilemap>();
        treesTilemap = GameObject.Find("Grid").transform.GetChild(5).gameObject.GetComponent<Tilemap>();
        buildingCanvas = GameObject.Find("BuildingCanvas");
        spawnChance = new Dictionary<string, int>();
    }

    // Update is called once per frame
    void Update()
    {
        if(assignedNPC != null){
            npcName = assignedNPC.name;
            if(building.name == "Crafting Station") {
                upgradeResource();
            } else {
                generateCrates();
            }
            removeTrees();
        }
    }

    void OnMouseDown() {
        buildingCanvas.transform.GetChild(4).gameObject.SetActive(true);
        var modal = buildingCanvas.transform.GetChild(4).gameObject;
        foreach (Transform child in modal.transform.GetChild(2)) {
            GameObject.Destroy(child.gameObject);
        }
        modal.transform.GetChild(0).GetComponent<Text>().text = building.name;
        modal.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(() => buildingCanvas.transform.GetChild(4).gameObject.SetActive(false));
        var count = 0;
        
        foreach(var upgrade in building.upgradesTo) {
            var upgradedbuilding = upgradeList.buildings[upgrade];
            var upgradeButton = Instantiate(modal.transform.GetChild(3).gameObject, new Vector3(0, 0, 0),  Quaternion.Euler(0,0,0));
            upgradeButton.transform.SetParent(modal.transform.GetChild(2));
            upgradeButton.transform.localPosition = new Vector3(-214 + (107 * count), -134, 0);
            upgradeButton.transform.GetChild(0).GetComponent<Text>().text = upgrade;
            upgradeButton.GetComponent<Button>().onClick.AddListener(() => selectBuildingButton(upgradedbuilding));
        }
        var spawnChanceString = "";
        foreach(KeyValuePair<string, int> resource in spawnChance){
            spawnChanceString += $"{resource.Key}: {resource.Value}\n";
        }
        modal.transform.GetChild(4).GetComponent<Text>().text = spawnChanceString;
    }

    public void selectBuildingButton(Building building){
        foreach(var resource in building.cost) {
            if(!(eventController.gameManager.stockpile.stats[resource.Key].value >= resource.Value)){
                return;
            }
        }
        buildingCanvas.transform.GetChild(3).gameObject.SetActive(true);
        var buildingModal = buildingCanvas.transform.GetChild(3).gameObject;
        buildingModal.transform.GetChild(0).gameObject.GetComponent<Text>().text = building.name;
        buildingModal.transform.GetChild(1).gameObject.GetComponent<Text>().text = building.description;
        buildingModal.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(() => confirmBuildingButton(building));
		

		// buildingLayer = GameObject.Find("BuildingPlacingMap");
        // buildingLayer.SetActive(true);
    }

    public void setSpawnChance() {
        foreach(KeyValuePair<string, int> resource in building.spawnChance){
            var defValue = (resource.Value * (100 - (assignedNPC.stats[bonusStat]))/ eventController.gameManager.gameSpeed);// * 2;
            if(assignedNPC.statOrder == statBonusOrdering) {
                defValue = (defValue/100) * 90;
            }
            spawnChance.Add(resource.Key, defValue);
        }
    }

    private void confirmBuildingButton(Building building) {
        Debug.Log(building.name);
        eventController.gameManager.stockpile.calculateBuildingCost(building.cost);
    }

    private void generateCrates() {
        foreach(KeyValuePair<string, int> resource in spawnChance){
            System.Random ran = new System.Random();
            var crateSpawnChanceInt = ran.Next(1, resource.Value);
            if(crateSpawnChanceInt == 1){
                var newCrate = Instantiate(crateObject, new Vector3 (tilePositions[0].x, tilePositions[0].y - 0.5f, 0), Quaternion.Euler(0,0,0));
                var crate = newCrate.GetComponent<Crate>();
                Debug.Log(resource);

                // refactor crate
                crate.player = player;
                crate.eventController = eventController;
                crate.controller = playerController;
                crateCount += 1;
                if(crateCount % 5 == 0) {
                    cutTree = true;
                }
                crate.value = building.startingResources[resource.Key].value;
                crate.type = resource.Key;
                newCrate.GetComponent<SpriteRenderer>().sortingOrder = 10;
            }
        }       
    }

    private void upgradeResource() {
        foreach(KeyValuePair<string, int> resource in spawnChance){
            System.Random ran = new System.Random();
            var crateSpawnChanceInt = ran.Next(1, resource.Value);
            if(crateSpawnChanceInt == 1){
                Debug.Log("oi");
                foreach(var costResource in building.resourcesToImprove[resource.Key]){
                    eventController.gameManager.stockpile.stats[costResource].value -= building.resourcesToImproveCosts[costResource];
                }
                var newCrate = Instantiate(crateObject, new Vector3 (tilePositions[0].x, tilePositions[0].y -1, 0), Quaternion.Euler(0,0,0));
                var crate = newCrate.GetComponent<Crate>();

                // refactor crate
                crate.player = player;
                crate.eventController = eventController;
                crate.controller = playerController;
                crateCount += 1;
                crate.value = building.startingResources[resource.Key].value;
                crate.type = resource.Key;
                newCrate.GetComponent<SpriteRenderer>().sortingOrder = 10;
            }
        }
    }

    private void removeTrees() {
        if(building.name == "Woodcutter's Hut" && cutTree) {
            for(var y = -3; y < (building.height + 3); y++){
                for(var x = -3; x < (building.width + 3); x++){
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
                        // worldTile.TilemapMember.SetTileFlags(worldTile.LocalPlace, TileFlags.None);
                        cutTree = false;
                        return;
                    }
                }
            }
        }
    }

}
