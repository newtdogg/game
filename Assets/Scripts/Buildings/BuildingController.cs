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
    private basicNPC[] allBasicNPCs;

    private Vector3Int tileVector;
    private GameObject boundaries;
    private Vector3 point;
    private GameObject tilemapObjects;
    private Tilemap buildingTilemap;
    public GameObject buildingMenu;
    private string buildingMenuActiveBuilding;
    private bool buildingCanvasActive;
    public Tilemap buildingPlacingMap;
    private GameObject buildingClone;
	private List<Vector3> allBuildingTilesPosition;
    private BuildingList buildingTypeList;
    public Tilemap ground;
    public BList navbarBuildingType;
    public string navbarBuildingTypeString;
    private TileBase tileObject;
    private TileData tileData;
    private GameObject buildingModal;
    private WorldTile worldTile;
    public string tilemapStatus;
    public string buildingType;
    public string buildingName;
    private EventController eventController;
    private ResourceList resourceList;
    public GameObject player;
    private PlayerController controller;

    void Start()
    {
        player = GameObject.Find("Player");
        resourceList = new ResourceList();
		allBuildingTilesPosition = new List<Vector3>();
        controller = player.GetComponent<PlayerController>();
        tilemapStatus = "inactive";
        buildingTypeList = new BuildingList();
        navbarBuildingTypeString = "Primary Resource";
        navbarBuildingType = buildingTypeList.b[navbarBuildingTypeString];
        BuildingList = new Dictionary<string, Vector3Int>();
        buildingMenu = gameObject.transform.GetChild(0).gameObject;
        buildingModal = gameObject.transform.GetChild(3).gameObject;
        setBuildingMenuButtons();
        buildingMenu.SetActive(false);
        buildingClone = GameObject.Find("BuildingDefault");
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
        tilemapObjects = GameObject.Find("Grid");
        // ground = tilemapObjects.transform.GetChild(0).gameObject.GetComponent<Tilemap>();
        buildingTilemap = tilemapObjects.transform.GetChild(2).gameObject.GetComponent<Tilemap>();
        buildingPlacingMap = tilemapObjects.transform.GetChild(3).gameObject.GetComponent<Tilemap>();
        getBuildingTileClones();
        eventController = GameObject.Find("EventControllerCanvas").GetComponent<EventController>();
    }

    // Update is called once per frame
    private void Update(){
        if (Input.GetKeyDown("b")){
            Debug.Log(navbarBuildingType);
            if(buildingCanvasActive == true){
                gameObject.transform.GetChild(0).gameObject.SetActive(false);
                buildingCanvasActive = false;
            } else {
                Debug.Log("jeerre");
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
                setBuildingTypeNavbar();
                buildingCanvasActive = true;
            }
        }

        // This is triggered from confirming you want to build a building
        if(tilemapStatus == "active") {
            var buildingInfo = returnBuilding(buildingName);
            if(hoverTiles(buildingInfo.width, buildingInfo.height, buildingInfo.name) == true) {
                if (Input.GetMouseButtonDown(0)){
                    var buildingTiles = setBuildingTiles(buildingInfo.name);
                    var buildingObjectPosition = new Vector3(buildingTiles[0].x + ((float)buildingInfo.width/2), buildingTiles[0].y + ((float)buildingInfo.height/2), 0);
                    Debug.Log(buildingTiles[0].x);
                    Debug.Log(buildingInfo.width/2);
                    Debug.Log(buildingTiles[0].y);
                    Debug.Log(buildingInfo.height/2);
                    GameObject newBuilding = Instantiate(buildingClone, buildingObjectPosition,  Quaternion.Euler(0,0,0));
                    newBuilding.GetComponent<BoxCollider2D>().size = new Vector2(buildingInfo.width, buildingInfo.height);
                    var buildingScript = newBuilding.GetComponent<BuildingObject>();
                    buildingScript.tilePositions = buildingTiles;
                    buildingScript.building = buildingInfo;
                    buildingScript.upgradeList = buildingTypeList.b[buildingInfo.upgradeType];
                    buildingScript.statBonusOrdering = buildingInfo.statBonusOrdering;
                    buildingScript.bonusStat = buildingInfo.bonusStat;
                    npcSelectionScreen(buildingScript);
                    buildingInfo.build(controller.gameManager, buildingInfo);
                    if(buildingInfo.name == "Croft" && controller.gameManager.questManager.Quests["BuildFarm"]["status"] == "active") {
                        controller.gameManager.questManager.completeQuest(controller.gameManager.questButtonImage, controller.gameManager.questCanvasList, "BuildFarm");
                        eventController.questmarkActive("SpeakToNPC");
                    }
                    if(buildingInfo.name == "Woodcutter's Hut" && controller.gameManager.questManager.Quests["buildWoodcuttersHut"]["status"] == "active") {
                        controller.gameManager.questManager.completeQuest(controller.gameManager.questButtonImage, controller.gameManager.questCanvasList, "buildWoodcuttersHut");
                    }
                }
            } else {
                Debug.Log("blocked");
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
                if(type == "Woodcutter's Hut") {
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
    
        if(suitable == (xLength * yLength)) {
            if(type == "Woodcutter's Hut" && wooded == true){
                return true;
            } else if (type == "Woodcutter's Hut" && wooded == false){
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
            var buildingCloneCoords = BuildingList[str];
            worldTile.Type = str;
            var clonedTile = buildingTilemap.GetTile(buildingCloneCoords);
            // var clonedBoundary = building
            buildingTilemap.SetTile(coords, clonedTile);
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

    public void selectBuildingButton(Building building){
        foreach(var resource in building.cost) {
            if(!(controller.gameManager.stockpile.stats[resource.Key].value >= resource.Value)){
                Debug.Log("piss piss piss piss");
                return;
            }
        }
        gameObject.transform.GetChild(3).gameObject.SetActive(true);
        buildingModal.transform.GetChild(0).gameObject.GetComponent<Text>().text = building.name;
        buildingModal.transform.GetChild(1).gameObject.GetComponent<Text>().text = building.description;
        buildingModal.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(() => confirmBuildingButton(building));
		

		// buildingLayer = GameObject.Find("BuildingPlacingMap");
        // buildingLayer.SetActive(true);
    }

    private void confirmBuildingButton(Building building) {
        buildingName = building.name;
        tilemapStatus = "active";
        buildingMenu.SetActive(false);
        buildingCanvasActive = false;
        buildingModal.SetActive(false);
        controller.gameManager.stockpile.calculateBuildingCost(building.cost);
    }

    private Building returnBuilding(string name) {
        var BuildingTList = buildingTypeList.b[navbarBuildingTypeString];
        foreach(KeyValuePair<string, Building> building in BuildingTList.buildings){
            if(name == building.Value.name) {
                buildingType = name;
                return building.Value;
            }
        }
        return new Building("dud", "dud", "dud", "dud", false, 0, 0, new Dictionary<string, int>{});
    }

    private void setBuildingTypeNavbar() {
        foreach (Transform child in buildingMenu.transform.GetChild(3)) {
            GameObject.Destroy(child.gameObject);
        }
        var count = 0;
        handleNavbarBuildingType(navbarBuildingType, navbarBuildingTypeString);
        foreach(var bType in buildingTypeList.b){
            var buildingButton = buildingMenu.transform.GetChild(1).gameObject;
            var bTypeButton = Instantiate(buildingButton, new Vector3(0, 0, 0),  Quaternion.Euler(0,0,0));
            bTypeButton.transform.SetParent(buildingMenu.transform.GetChild(3));
            bTypeButton.GetComponent<Button>().onClick.AddListener(() => handleNavbarBuildingType(bType.Value, bType.Key));
            bTypeButton.transform.GetChild(0).GetComponent<Text>().text = bType.Key;
            bTypeButton.transform.localPosition = new Vector3(-500 + (190 * count), 356, 0);
            count += 1;
        }
    }

    private void handleNavbarBuildingType(BList type, string name) {
        navbarBuildingTypeString = name;
        navbarBuildingType = type;
        setBuildingMenuButtons();
    }

    private void setBuildingMenuButtons() {
        foreach (Transform child in buildingMenu.transform.GetChild(2)) {
            GameObject.Destroy(child.gameObject);
        }

        allBasicNPCs = FindObjectsOfType<basicNPC>();		
        var idleNpcCount = 0;		
        for(var i = 0; i < allBasicNPCs.Length; i++){		
            if(allBasicNPCs[i].status == "idle"){		
                idleNpcCount += 1;		
            }		
        }		
        if(idleNpcCount == 0) {		
            return;		
        }

        var count = 0;
        foreach(var bldType in navbarBuildingType.buildings) {
            var buildingButton = buildingMenu.transform.GetChild(0).gameObject;
            var bTypeButton = Instantiate(buildingButton, new Vector3(0, 0, 0),  Quaternion.Euler(0,0,0));
            bTypeButton.transform.SetParent(buildingMenu.transform.GetChild(2));
            bTypeButton.GetComponent<Button>().onClick.AddListener(() => selectBuildingButton(bldType.Value));
            bTypeButton.transform.GetChild(0).GetComponent<Text>().text = bldType.Value.name;
            bTypeButton.transform.localPosition = new Vector3(-458 + (210 * count), 258, 0);
            count += 1;
        }
    }



    private void getBuildingTileClones() {
        BuildingList = new Dictionary<string, Vector3Int>();
        BuildingList.Add("Woodcutter's Hut1", new Vector3Int(-1, -1, 0));
        BuildingList.Add("Woodcutter's Hut2", new Vector3Int(-1, -1, 0));
        BuildingList.Add("Woodcutter's Hut3", new Vector3Int(-1, -1, 0));
        BuildingList.Add("Woodcutter's Hut4", new Vector3Int(-1, -1, 0));
        BuildingList.Add("Mine1", new Vector3Int(-1, -1, 0));
        BuildingList.Add("Mine2", new Vector3Int(-1, -1, 0));
        BuildingList.Add("Mine3", new Vector3Int(-1, -1, 0));
        BuildingList.Add("Mine4", new Vector3Int(-1, -1, 0));
        BuildingList.Add("Croft1", new Vector3Int(-1, -1, 0));
        BuildingList.Add("Croft2", new Vector3Int(-1, -1, 0));
        BuildingList.Add("Croft3", new Vector3Int(-1, -1, 0));
        BuildingList.Add("Croft4", new Vector3Int(-1, -1, 0));
        BuildingList.Add("Croft5", new Vector3Int(-1, -1, 0));
        BuildingList.Add("Croft6", new Vector3Int(-1, -1, 0));
        BuildingList.Add("house1", new Vector3Int(-1, -1, 0));
        BuildingList.Add("house2", new Vector3Int(-1, -1, 0));
        BuildingList.Add("house3", new Vector3Int(-1, -1, 0));
        BuildingList.Add("house4", new Vector3Int(-1, -1, 0));
        BuildingList.Add("house5", new Vector3Int(-1, -1, 0));
        BuildingList.Add("house6", new Vector3Int(-1, -1, 0));
        BuildingList.Add("Crafting Station1", new Vector3Int(-1, -1, 0));
        BuildingList.Add("Crafting Station2", new Vector3Int(-1, -1, 0));
        BuildingList.Add("Crafting Station3", new Vector3Int(-1, -1, 0));
        BuildingList.Add("Crafting Station4", new Vector3Int(-1, -1, 0));
        BuildingList.Add("Crafting Station5", new Vector3Int(-1, -1, 0));
        BuildingList.Add("Crafting Station6", new Vector3Int(-1, -1, 0));
        BuildingList.Add("Boundary", new Vector3Int(-1, -1, 0));
    }

    private void npcSelectionScreen(BuildingObject building){
        gameObject.transform.GetChild(1).gameObject.SetActive(true);
        for(var i = 0; i < allBasicNPCs.Length; i++){
            if(allBasicNPCs[i].status == "idle"){
                GameObject newButton = Instantiate(gameObject.transform.GetChild(2).gameObject, new Vector3(270 + (i * 50), 280, 0),  Quaternion.Euler(0,0,0));
                newButton.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 60);
                Button butt = newButton.GetComponent<Button>();
                butt.image.sprite = allBasicNPCs[i].gameObject.GetComponent<SpriteRenderer>().sprite;
                newButton.transform.SetParent(gameObject.transform.GetChild(1));
                newButton.GetComponent<npcUIButton>().npcObject = allBasicNPCs[i].gameObject;
                butt.onClick.AddListener(() => assignBuildingNPC(building, newButton.GetComponent<npcUIButton>().npcObject));
            }
        }
    }

    public void assignBuildingNPC(BuildingObject building, GameObject npcObject){
        building.assignedNPC = npcObject.GetComponent<basicNPC>().npc;
        npcObject.transform.position = new Vector3(building.tilePositions[0].x + 1, building.tilePositions[0].y + 1, 0);
        npcObject.GetComponent<basicNPC>().status = "working";
        building.setSpawnChance();
        npcObject.GetComponent<basicNPC>().npc.employment = building.building.name;
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }

}