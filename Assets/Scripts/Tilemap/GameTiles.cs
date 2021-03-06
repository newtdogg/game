﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameTiles : MonoBehaviour
{
    public static GameTiles instance;
    public GameObject buildingTilemapObject;
    public Tilemap buildingTilemap;
    public WorldTile tile;
    private WorldTile worldTile;
    public Tile groundTile;

    public Dictionary<Vector3, WorldTile> tiles;

    private void Awake()
    {
        buildingTilemap = this.gameObject.GetComponent<Tilemap>();
        if (instance == null)
        {
            instance = this;
        }
        // else if (instance != this)
        // {
        //     Destroy(gameObject);
        // }
        GetWorldTiles();
    }

    // Use this for initialization
    private void GetWorldTiles()
    {
        tiles = new Dictionary<Vector3, WorldTile>();
        foreach (Vector3Int pos in buildingTilemap.cellBounds.allPositionsWithin)
        {
            var localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            if (!buildingTilemap.HasTile(localPlace)) continue;
            var tile = ScriptableObject.CreateInstance<WorldTile>();
            tile.LocalPlace = localPlace;
            tile.WorldLocation = buildingTilemap.CellToWorld(localPlace);
            // Debug.Log(tile.WorldLocation);
            tile.TileBase = buildingTilemap.GetTile(localPlace);
            tile.TilemapMember = buildingTilemap;
            tile.Type = "grass";
            tile.Name = localPlace.x + "," + localPlace.y;
            tile.Cost = 1; // TODO: Change this with the proper cost from ruletile
            buildingTilemap.SetTileFlags(localPlace, TileFlags.None);
            buildingTilemap.SetColor(localPlace, Color.clear);
            tiles.Add(tile.WorldLocation, tile);
        }

        string[,] map = new string[,]{
            {"tree", "tree", "tree", "tree", "tree", "tree", "tree", "tree", "tree", "building", "building", "building", "building", "building", "building", "tree", "tree", "tree", "tree", "tree", "grass", "grass", "grass", "grass", "tree", "tree", "tree", "tree"},
            {"tree", "tree", "tree", "tree", "tree", "tree", "tree", "tree", "tree", "building", "building", "building",  "building", "building", "building", "building", "tree", "tree", "tree", "tree", "tree", "tree", "grass", "grass", "grass", "tree", "tree", "tree"},
            {"tree", "tree", "tree", "tree", "tree", "tree", "tree", "tree", "tree", "building", "building", "building",  "building", "building", "building", "building", "tree", "tree", "tree", "tree", "tree", "tree", "tree", "tree", "tree", "tree", "tree", "tree"},
            {"tree", "tree", "tree", "tree", "tree", "tree", "tree", "tree", "tree", "building", "building", "building",  "building", "building", "building", "building", "tree", "tree", "tree", "tree", "tree", "tree", "tree", "tree", "tree", "tree", "tree", "tree"},
            {"tree", "tree", "tree", "tree", "tree", "tree", "tree", "tree", "tree", "building", "building", "building",  "building", "building", "building", "building", "building", "tree", "tree", "tree", "tree", "tree", "grass", "grass", "grass", "tree", "tree", "tree"},
            {"tree", "tree", "tree", "tree", "tree", "tree", "grass", "grass", "grass", "building", "building", "building",  "building", "building", "building", "building", "building", "grass", "grass", "grass", "grass", "grass", "grass", "grass", "grass", "grass", "tree", "tree"},
            {"tree", "tree", "tree", "tree", "tree", "tree", "grass", "grass", "grass", "grass", "grass", "grass",  "grass", "grass", "grass", "grass", "grass", "grass", "grass", "grass", "grass", "grass", "grass", "grass", "grass", "grass", "tree", "tree"},
            {"tree", "tree", "tree", "tree", "tree", "grass", "grass", "grass", "grass", "grass", "grass", "grass",  "grass", "grass", "grass", "grass", "grass", "grass", "grass", "grass", "grass", "grass", "grass", "grass", "grass", "grass", "tree", "tree"}
        };

        for(var y = 0; y < 7; y++) {
            for(var x = 0; x < 28; x++) {
                var coords = new Vector3Int(14 + x, 31 - y, 0);
                if (tiles.TryGetValue(coords, out worldTile)){
                    worldTile.Type = map[y, x];
                }
            }
        }
    }
}
