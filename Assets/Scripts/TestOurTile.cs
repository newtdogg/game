

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TestOurTile : MonoBehaviour
{
    public Sprite sprite;
    private List<Vector3Int> tileCoords;
    private List<WorldTile> tileArray;
    private Dictionary<string, Vector3Int> BuildingList;
    private Vector3Int tileVector;
    private GameObject boundaries;
    private Tilemap boundariesTilemap;
    private Vector3 point;
    private GameObject buildingTilemapObject;
    private Tilemap buildingTilemap;
    public GameObject buildingMenu;
    private GameObject buildingCanvas;
    private TileBase tileObject;
    private TileData tileData;
    private WorldTile worldTile;
    public string status;
    public string tileType;

    void Start()
    {
        status = "inactive";
        buildingCanvas = GameObject.Find("BuildingCanvas");
        buildingMenu = GameObject.Find("BuildingMenu");
        buildingMenu.SetActive(false);
        buildingTilemapObject = GameObject.Find("Ground");
        buildingTilemap = buildingTilemapObject.GetComponent<Tilemap>();
        boundaries = GameObject.Find("Boundaries");
        boundariesTilemap = boundaries.GetComponent<Tilemap>();
        this.getBuildingTileClones();
    }
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown("b"))
        {
            print("b key was pressed");
            buildingCanvas.transform.GetChild(0).gameObject.SetActive(true);

        }
        if(status == "active") {
            if (buildingMenu.active == true) {
                buildingMenu.SetActive(false);
            }
            
            this.getTilesAndCoords();
                       
            if(tileType != ""){
                this.hoverFourTile();
                if (Input.GetMouseButtonDown(0)){
                    if(checkEmptyFourTiles() == true){
                        this.setFourTile("Farm");
                        status = "inactive";
                        tileType = "";
                    }
                    

                    // point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    // var worldPoint = new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), 0);
                    // var tiles = GameTiles.instance.tiles;
                    // this.getTileLocation(tiles, worldPoint);
                }
            }

           
        }
        

    }

    private void getTilesAndCoords() {
        tileCoords = new List<Vector3Int>();
        tileArray = new List<WorldTile>();
        for(var x = -1; x < 4; x++){
             for(var y = -1; y < 4; y++){
                point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var worldPoint = new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), 0);
                tileVector = new Vector3Int(worldPoint.x + x, worldPoint.y + y, 0);
                tileCoords.Add(tileVector);
                var tiles = GameTiles.instance.tiles;
                if (tiles.TryGetValue(tileVector, out worldTile))
                {
                    tileArray.Add(tiles[tileVector]);
                }
                // tileArray.Add(new WorldTile());
            }
        }
        // Debug.Log(tileArray[6]);

    }
    private void dehighlightTile(Dictionary<Vector3,WorldTile> tiles, Vector3 coords, WorldTile worldTile){
        if (tiles.TryGetValue(coords, out worldTile))
            {
                worldTile.TilemapMember.SetTileFlags(worldTile.LocalPlace, TileFlags.None);
                worldTile.TilemapMember.SetColor(worldTile.LocalPlace, Color.clear);
            }
    }

    private void highlightTile(Dictionary<Vector3,WorldTile> tiles, Vector3 coords, WorldTile worldTile){
        if (tiles.TryGetValue(coords, out worldTile))
            {
                Debug.Log(worldTile.Type);
                if(worldTile.Type == "Empty"){
                    worldTile.TilemapMember.SetTileFlags(worldTile.LocalPlace, TileFlags.None);
                    worldTile.TilemapMember.SetColor(worldTile.LocalPlace, Color.green);
                }
                worldTile.TilemapMember.SetTileFlags(worldTile.LocalPlace, TileFlags.None);
                worldTile.TilemapMember.SetColor(worldTile.LocalPlace, new Color(1f, 0.2f, 0.2f, 0.3f));
                
            }
    }
    private void getTileLocation(Dictionary<Vector3,WorldTile> tiles, Vector3 coords) {
         if (tiles.TryGetValue(coords, out worldTile))
            {
                Debug.Log(worldTile.LocalPlace);
                
            }
    }

    private bool emptyTile(WorldTile tile){
        Debug.Log(tile.Type);
        if(tile.Type != "Empty"){
            return false;
        }
        return true;
    }

    private bool checkEmptyFourTiles() {
        if(emptyTile(tileArray[6]) == true && emptyTile(tileArray[7]) == true && emptyTile(tileArray[11]) == true && emptyTile(tileArray[12])){
            return true;
        }
        return false;
    }


    private void hoverFourTile() {
        var tiles = GameTiles.instance.tiles; // This is our Dictionary of tiles
            
        highlightTile(tiles, tileCoords[6], tileArray[6]);
        highlightTile(tiles, tileCoords[7], tileArray[7]);
        highlightTile(tiles, tileCoords[11], tileArray[11]);
        highlightTile(tiles, tileCoords[12], tileArray[12]);

        dehighlightTile(tiles, tileCoords[0], tileArray[0]);
        dehighlightTile(tiles, tileCoords[1], tileArray[1]);
        dehighlightTile(tiles, tileCoords[2], tileArray[2]);
        dehighlightTile(tiles, tileCoords[3], tileArray[3]);

        dehighlightTile(tiles, tileCoords[5], tileArray[5]);
        dehighlightTile(tiles, tileCoords[8], tileArray[8]);
        dehighlightTile(tiles, tileCoords[10], tileArray[10]);
        dehighlightTile(tiles, tileCoords[13], tileArray[13]);

        dehighlightTile(tiles, tileCoords[15], tileArray[15]);
        dehighlightTile(tiles, tileCoords[16], tileArray[16]);
        dehighlightTile(tiles, tileCoords[17], tileArray[17]);
        dehighlightTile(tiles, tileCoords[18], tileArray[18]);
            
    }

    private void setTile(Dictionary<Vector3,WorldTile> tiles, Vector3Int coords, string building, int index){
        if (tiles.TryGetValue(coords, out worldTile))
            {
                // returns class Tile at given coords
                var str = $"{building}{index}";
                var buidlingCloneCoords = BuildingList[str];
                worldTile.Type = str;
                var clonedTile = buildingTilemap.GetTile(buidlingCloneCoords);
                // var clonedBoundary = building
                Debug.Log(clonedTile);
                buildingTilemap.SetTile(coords, clonedTile);
                boundariesTilemap.SetTile(coords, clonedTile);
                worldTile.TilemapMember.SetTileFlags(worldTile.LocalPlace, TileFlags.None);
                worldTile.TilemapMember.SetColor(worldTile.LocalPlace, Color.clear);

            // // returns tilebase at given coords
            // tileObject = tilemap.GetTile(coords);
           
            

            //    Sprite spr = tileObject

                // tileObject.sprite = $"{building}{1}";
                // worldTile.TilemapMember.sprite = $"{building}{1}";
                // worldTile.TilemapMember.SetColor(worldTile.LocalPlace, new Color(1f, 0.2f, 0.2f, 0.3f));
            }
    }

    private void setFourTile(string building) {
        var tiles = GameTiles.instance.tiles;
        setTile(tiles, tileCoords[6], building, 1);
        setTile(tiles, tileCoords[7], building, 3);
        setTile(tiles, tileCoords[11], building, 2);
        setTile(tiles, tileCoords[12], building, 4);
    }

    private void getBuildingTileClones() {
        BuildingList = new Dictionary<string, Vector3Int>();
        BuildingList.Add("Farm1", new Vector3Int(-19, 0, 0));
        BuildingList.Add("Farm2", new Vector3Int(-18, 0, 0));
        BuildingList.Add("Farm3", new Vector3Int(-19, 1, 0));
        BuildingList.Add("Farm4", new Vector3Int(-18, 1, 0));
        BuildingList.Add("Boundary", new Vector3Int(-19, -1, 0));
    }

}
