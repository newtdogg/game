

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;


public class BuildingController : MonoBehaviour
{
    public Sprite sprite;
    private List<Vector3Int> tileCoords;
    private Dictionary<string, Vector3Int> BuildingList;
    private Dictionary<string, Vector3Int> Building;
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
    public Tilemap ground;
    private TileBase tileObject;
    private TileData tileData;
    private WorldTile worldTile;
    public string tilemapStatus;
    public string tileType;
    public GameObject player;
    private PlayerController controller;

    void Start()
    {
        player = GameObject.Find("Player");
        controller = player.GetComponent<PlayerController>();
        tilemapStatus = "inactive";
        BuildingList = new Dictionary<string, Vector3Int>();
        Building = new Dictionary<string, Vector3Int>();
        buildingMenu = gameObject.transform.GetChild(0).gameObject;
        buildingMenu.transform.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(() => selectBuildingButton("farm"));
        buildingMenu.SetActive(false);
        buildingClone = GameObject.Find("BuildingDefault");
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
        tilemapObjects = GameObject.Find("Grid");
        // ground = tilemapObjects.transform.GetChild(0).gameObject.GetComponent<Tilemap>();
        buildingTilemap = tilemapObjects.transform.GetChild(2).gameObject.GetComponent<Tilemap>();
        boundariesTilemap = tilemapObjects.transform.GetChild(1).gameObject.GetComponent<Tilemap>();
        buildingPlacingMap = tilemapObjects.transform.GetChild(3).gameObject.GetComponent<Tilemap>();
        getBuildingTileClones();
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
                if(tileType != ""){
                    if(hoverTiles(3, 2) == true){
                        if (Input.GetMouseButtonDown(0)){
                            GameObject newBuilding = Instantiate(buildingClone, new Vector3(0, 0, 0),  Quaternion.Euler(0,0,0));
                            var buildingTiles = setBuildingTiles("Farm", 2, 3, 2);
                            newBuilding.GetComponent<Building>().tilePositions = buildingTiles;
                            npcSelectionScreen(newBuilding.GetComponent<Building>());
                        }
                    } else {
                        Debug.Log("blocked");
                    }
                }
            }
        }
    }

    private void highlightTile(Dictionary<Vector3,WorldTile> tiles, Vector3 coords, WorldTile worldTile){
        if (tiles.TryGetValue(coords, out worldTile))
            {
                // Debug.Log(worldTile.Type);
                if(worldTile.Type == "Empty"){
                    worldTile.TilemapMember.SetTileFlags(worldTile.LocalPlace, TileFlags.None);
                    worldTile.TilemapMember.SetColor(worldTile.LocalPlace, Color.green);
                }
                worldTile.TilemapMember.SetTileFlags(worldTile.LocalPlace, TileFlags.None);
                worldTile.TilemapMember.SetColor(worldTile.LocalPlace, new Color(1f, 0.2f, 0.2f, 0.3f));

            }
    }

    private void dehighlightAllTiles(Dictionary<Vector3,WorldTile> tiles){
        foreach(KeyValuePair<Vector3,WorldTile> tile in tiles){
            tile.Value.TilemapMember.SetColor(tile.Value.LocalPlace, Color.clear);
        }
    }
    private bool emptyTile(WorldTile tile){
        // Debug.Log(tile.Type);
        if(tile.Type != "Empty"){
            return false;
        }
        return true;
    }


    private bool hoverTiles(int xLength, int yLength) {
        var tiles = GameTiles.instance.tiles; // This is our Dictionary of tiles
        var padding = 2;
        var width = xLength + (padding * 2);
        var height = yLength + (padding * 2);

        var centerTiles = returnCenterTiles(padding, xLength, yLength);

        tileCoords = new List<Vector3Int>();
        var tileArray = new List<WorldTile>();
        for(var y = -padding; y < (height - padding); y++){
             for(var x = -padding; x < (width - padding); x++){
                point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var worldPoint = new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), 0);
                tileVector = new Vector3Int(worldPoint.x + x, worldPoint.y + y, 0);
                tileCoords.Add(tileVector);
                if (tiles.TryGetValue(tileVector, out worldTile)){
                    tileArray.Add(tiles[tileVector]);
                }
                // tileArray.Add(new WorldTile());
            }
        }
      
        dehighlightAllTiles(tiles);
        for(var xy = 0; xy < (width * height); xy++){
            if(centerTiles.Contains(xy)){
                highlightTile(tiles, tileCoords[xy], tileArray[xy]);
            }
        }
                        
        foreach(var index in centerTiles){
            if(emptyTile(tileArray[index]) == false){
                return false;
            }
        }
        return true;
    }

    private List<Vector3> setBuildingTiles(string building, int padding, int x, int y) {
        var tiles = GameTiles.instance.tiles;
        var allTilesPosition = new List<Vector3>(); 
        var width = x + (padding * 2);
        var height = y + (padding * 2);
        var centerTiles = returnCenterTiles(padding, x, y);
        var buildingCount = 1;
        for(var xy = 0; xy < (width * height) -1; xy++){
            if(centerTiles.Contains(xy)){
                setTile(tiles, tileCoords[xy], building, buildingCount);
                buildingCount += 1;
                allTilesPosition.Add(tileCoords[xy]);
            }
        }
        tilemapStatus = "inactive";
        return allTilesPosition;


    }

    private void setTile(Dictionary<Vector3,WorldTile> tiles, Vector3Int coords, string building, int index){
        if (tiles.TryGetValue(coords, out worldTile))
            {    
                // returns class Tile at given coords
                var str = $"{building}{index}";
                Debug.Log(str);
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
        BuildingList.Add("Farm1", new Vector3Int(-19, 0, 0));
        BuildingList.Add("Farm2", new Vector3Int(-18, 0, 0));
        BuildingList.Add("Farm3", new Vector3Int(-19, 1, 0));
        BuildingList.Add("Farm4", new Vector3Int(-18, 1, 0));
        BuildingList.Add("Farm5", new Vector3Int(-19, 1, 0));
        BuildingList.Add("Farm6", new Vector3Int(-18, 1, 0));
        BuildingList.Add("Boundary", new Vector3Int(-19, -1, 0));
    }

    private void npcSelectionScreen(Building building){
        gameObject.transform.GetChild(1).gameObject.SetActive(true);
        var allBasicNPCs = FindObjectsOfType<basicNPC>();
        Debug.Log(allBasicNPCs.Length);
        for(var i = 0; i < allBasicNPCs.Length; i++){
            // if(allBasicNPCs[i].status == "idle"){
                Debug.Log(i);
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
        npcObject.transform.position = building.GetComponent<Building>().tilePositions[0];
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }

}

