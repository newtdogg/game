using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;


public class BuildingController : MonoBehaviour
{
    public Sprite sprite;
    private Dictionary<string, Vector3Int> BuildingList;
    private Dictionary<string, Building> Buildings;

	private List<List<Vector3Int>> tileCoords;
	private List<List<WorldTile>> tileArray;
	private int width;
	private int height;
	private int padding;

    private Vector3Int tileVector;
    private GameObject boundaries;
    private Tilemap boundariesTilemap;
    private Vector3 point;
    private GameObject tilemapObjects;
    private Tilemap buildingTilemap;
    public GameObject buildingMenu;
    private bool buildingCanvasActive;
    public Tilemap buildingPlacingMap;
    private GameObject buildingClone;
	private List<Vector3> allBuildingTilesPosition;
    private BuildingInfoList buildingInfoList;
    public Tilemap ground;
    private TileBase tileObject;
    private TileData tileData;
    private WorldTile worldTile;
    public string tilemapStatus;
    public string tileType;
    private EventController eventController;
    private ResourceList resourceList;
    public GameObject player;
    private PlayerController controller;

    void Start()
    {
        player = GameObject.Find("Player");
        resourceList = new ResourceList();
        buildingInfoList = new BuildingInfoList();
		allBuildingTilesPosition = new List<Vector3>();
        controller = player.GetComponent<PlayerController>();
        tilemapStatus = "inactive";
        BuildingList = new Dictionary<string, Vector3Int>();
        buildingMenu = gameObject.transform.GetChild(0).gameObject;
        buildingMenu.transform.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(() => selectBuildingButton("farm"));
        buildingMenu.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(() => selectBuildingButton("woodcutters hut"));
        buildingMenu.SetActive(false);
        buildingClone = GameObject.Find("BuildingDefault");
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
        tilemapObjects = GameObject.Find("Grid");
        // ground = tilemapObjects.transform.GetChild(0).gameObject.GetComponent<Tilemap>();
        buildingTilemap = tilemapObjects.transform.GetChild(2).gameObject.GetComponent<Tilemap>();
        boundariesTilemap = tilemapObjects.transform.GetChild(1).gameObject.GetComponent<Tilemap>();
        buildingPlacingMap = tilemapObjects.transform.GetChild(3).gameObject.GetComponent<Tilemap>();
        getBuildingTileClones();
        eventController = GameObject.Find("EventControllerCanvas").GetComponent<EventController>();
    }

    // Update is called once per frame
    private void Update(){
        if(controller.gameManager.questManager.Quests["TalktoNPC"]["status"] == "complete"){
            if (Input.GetKeyDown("b")){
                if(buildingCanvasActive == true){
                    gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    buildingCanvasActive = false;
                } else {
                    gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    buildingCanvasActive = true;
                }
            }
            if(tilemapStatus == "active") {
                if (buildingMenu.active == true) {
                    buildingMenu.SetActive(false);
                    buildingCanvasActive = false;
                }
                foreach(KeyValuePair<string, BuildingInfo> building in buildingInfoList.b){
                    if(tileType == building.Value.name) {
                        var b = building.Value;
                        if(hoverTiles(b.width, b.height, building.Key) == true) {
                            if (Input.GetMouseButtonDown(0)){
                                GameObject newBuilding = Instantiate(buildingClone, new Vector3(0, 0, 0),  Quaternion.Euler(0,0,0));
                                var buildingTiles = setBuildingTiles(building.Key);
                                newBuilding.GetComponent<Building>().tilePositions = buildingTiles;
                                newBuilding.GetComponent<Building>().type = tileType;
                                newBuilding.GetComponent<Building>().resources = b.startingResources;
                                newBuilding.GetComponent<Building>().spawnChance = b.spawnChance;
                                foreach(KeyValuePair<string, Resource> resource in b.startingResources) {
                                    if(!controller.gameManager.stockpile.stats.ContainsKey(resource.Key)) {
                                        controller.gameManager.stockpile.stats.Add(resource.Key, resource.Value);
                                    }
                                }
                                npcSelectionScreen(newBuilding.GetComponent<Building>());
                                if(building.Key == "farm" && controller.gameManager.questManager.Quests["BuildFarm"]["status"] == "active") {
                                    controller.gameManager.questManager.completeQuest(controller.gameManager.questButtonImage, controller.gameManager.questCanvasList, "BuildFarm");
                                    eventController.questmarkActive("SpeakToNPC");
                                }
                                if(building.Key == "woodcutters hut" && controller.gameManager.questManager.Quests["buildWoodcuttersHut"]["status"] == "active") {
                                    controller.gameManager.questManager.completeQuest(controller.gameManager.questButtonImage, controller.gameManager.questCanvasList, "buildWoodcuttersHut");
                                }
                            }
                        }
                    } else {
                        Debug.Log("blocked");
                    }
                }
            }
        }
    }

    private void highlightTile(Dictionary<Vector3,WorldTile> tiles, Vector3 coords, WorldTile worldTile, Color color){
        if (tiles.TryGetValue(coords, out worldTile)){
            worldTile.TilemapMember.SetTileFlags(worldTile.LocalPlace, TileFlags.None);
            worldTile.TilemapMember.SetColor(worldTile.LocalPlace, color);

        }
    }

    private void dehighlightAllTiles(Dictionary<Vector3,WorldTile> tiles){
        foreach(KeyValuePair<Vector3,WorldTile> tile in tiles){
            tile.Value.TilemapMember.SetColor(tile.Value.LocalPlace, Color.clear);
        }
    }

    private bool checkTile(WorldTile tile, string type){
        // Debug.Log(tile.Type);
        if(tile.Type == type){
            return true;
        }
        return false;
    }


    private bool hoverTiles(int xLength, int yLength, string type) {
        var tiles = GameTiles.instance.tiles; // This is our Dictionary of tiles
        padding = 2;
        width = xLength + (padding * 2);
        height = yLength + (padding * 2);
        // var centerTiles = returnCenterTiles(padding, xLength, yLength);
        tileCoords = new List<List<Vector3Int>>();
		tileArray = new List<List<WorldTile>>();
        var wooded = false;
		for(var y = -padding; y < (height - padding); y++){
			var coordsRow = new List<Vector3Int>();
			var tileRow = new List<WorldTile>();
			for(var x = -padding; x < (width - padding); x++){
				point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var worldPoint = new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), 0);
                tileVector = new Vector3Int(worldPoint.x + x, worldPoint.y + y, 0);
				if (tiles.TryGetValue(tileVector, out worldTile)){
                    tileRow.Add(tiles[tileVector]);
                }
				coordsRow.Add(tileVector);
                if(type == "woodcutters hut") {
                    if(checkTile(tiles[tileVector], "tree") == true) {
                        wooded = true;
                    }
                }
			}
			tileCoords.Add(coordsRow);
			tileArray.Add(tileRow);
		}
		dehighlightAllTiles(tiles);
        int suitable = 0;
		for (var y = padding; y < height - padding; y++) {
			for(var x = padding; x < width - padding; x++) {
				if(allBuildingTilesPosition.Contains(tileCoords[y][x]) || checkTile(tileArray[y][x], "grass") == false) {
					highlightTile(tiles, tileCoords[y][x], tileArray[y][x], Color.black);
                    Debug.Log(tileArray[y][x].Type);
				} else {
                    highlightTile(tiles, tileCoords[y][x], tileArray[y][x], new Color(0.2f, 0.2f, 0.2f, 0.5f));
                    Debug.Log(tileArray[y][x].Type);
                    suitable += 1;
                }
			}
        }
    
        Debug.Log(wooded);
        if(suitable == (xLength * yLength)) {
            if(type == "woodcutters hut" && wooded == true){
                return true;
            } else if (type == "woodcutters hut" && wooded == false){
                return false;
            }
            return true;
        }
        return false;
    }

    private List<Vector3> setBuildingTiles(string building) {
        var tiles = GameTiles.instance.tiles;
		var buildingTilePositions = new List<Vector3>();
		for (var y = padding; y < height - padding; y++) {
			for(var x = padding; x < width - padding; x++) {
				setTile(tiles, tileCoords[y][x], building, 1);
                allBuildingTilesPosition.Add(tileCoords[y][x]);
				buildingTilePositions.Add(tileCoords[y][x]);
			}
		}
        tilemapStatus = "inactive";
        return buildingTilePositions;
    }

    private void setTile(Dictionary<Vector3,WorldTile> tiles, Vector3Int coords, string building, int index){
        if (tiles.TryGetValue(coords, out worldTile)){
            Debug.Log(coords);
            // returns class Tile at given coords
            var str = $"{building}{index}";
            var buidlingCloneCoords = BuildingList[str];
            worldTile.Type = str;
            var clonedTile = buildingTilemap.GetTile(buidlingCloneCoords);
            // var clonedBoundary = building
            buildingTilemap.SetTile(coords, clonedTile);
            boundariesTilemap.SetTile(coords, clonedTile);
            worldTile.TilemapMember.SetTileFlags(worldTile.LocalPlace, TileFlags.None);
        }
    }

    private List<int> returnCenterTiles(int padding, int x, int y){
        // GET LIST OF CENTER TILES INDEX
        var centerTiles = new List<int>();
        var width = x + (padding * 2);
        for(var n = padding; n < padding + y; n++){
            for(var t = 0; t < x; t++){
                centerTiles.Add((width * n) + padding + t);
            }
        }
        return centerTiles;

    }

    public void selectBuildingButton(string building){
        Debug.Log("You have clicked the button!");
		tileType = building;
		tilemapStatus = "active";

		// buildingLayer = GameObject.Find("BuildingPlacingMap");
        // buildingLayer.SetActive(true);
    }



    private void getBuildingTileClones() {
        BuildingList = new Dictionary<string, Vector3Int>();
        BuildingList.Add("woodcutters hut1", new Vector3Int(-19, 0, 0));
        BuildingList.Add("woodcutters hut2", new Vector3Int(-19, 0, 0));
        BuildingList.Add("woodcutters hut3", new Vector3Int(-19, 0, 0));
        BuildingList.Add("woodcutters hut4", new Vector3Int(-19, 0, 0));
        BuildingList.Add("farm1", new Vector3Int(-19, 0, 0));
        BuildingList.Add("farm2", new Vector3Int(-18, 0, 0));
        BuildingList.Add("farm3", new Vector3Int(-19, 1, 0));
        BuildingList.Add("farm4", new Vector3Int(-18, 1, 0));
        BuildingList.Add("farm5", new Vector3Int(-19, 1, 0));
        BuildingList.Add("farm6", new Vector3Int(-18, 1, 0));
        BuildingList.Add("Boundary", new Vector3Int(-19, -1, 0));
    }

    private void npcSelectionScreen(Building building){
        gameObject.transform.GetChild(1).gameObject.SetActive(true);
        var allBasicNPCs = FindObjectsOfType<basicNPC>();
        Debug.Log(allBasicNPCs.Length);
        for(var i = 0; i < allBasicNPCs.Length; i++){
            // if(allBasicNPCs[i].status == "idle"){
                GameObject newButton = Instantiate(gameObject.transform.GetChild(2).gameObject, new Vector3(270 + (i * 50), 280, 0),  Quaternion.Euler(0,0,0));
                newButton.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 60);
                Button butt = newButton.GetComponent<Button>();
                butt.image.sprite = allBasicNPCs[i].gameObject.GetComponent<SpriteRenderer>().sprite;
                newButton.transform.SetParent(gameObject.transform.GetChild(1));
                newButton.GetComponent<npcUIButton>().npcObject = allBasicNPCs[i].gameObject;
                butt.onClick.AddListener(() => assignBuildingNPC(building, newButton.GetComponent<npcUIButton>().npcObject));
            // }
        }
    }

    public void assignBuildingNPC(Building building, GameObject npcObject){
        building.assignedNPC = npcObject.GetComponent<basicNPC>().npc;
        npcObject.transform.position = building.tilePositions[0];
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }

}