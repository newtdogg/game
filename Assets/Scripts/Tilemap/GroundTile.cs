using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundTile : Tile
{
    public Sprite tileSprite;
    public Vector3Int WorldLocation;
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData){
        tileData.sprite = tileSprite;
    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap){

    }
}