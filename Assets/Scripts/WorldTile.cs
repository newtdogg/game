using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldTile : Tile
{
    public Sprite tileSprite { get; set; }
    [SerializeField]
    public Sprite[] farmSprites;

    public Vector3Int LocalPlace { get; set; }

    public Vector3 WorldLocation { get; set; }

    public TileBase TileBase { get; set; }

    public Tilemap TilemapMember { get; set; }
    public string Type { get; set; }

    public string Name { get; set; }

    // Below is needed for Breadth First Searching
    public bool IsExplored { get; set; }

    public WorldTile ExploredFrom { get; set; }

    public int Cost { get; set; }

//     public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData){
//         tileData.sprite = farmSprites[0];
//         Debug.Log("here");
//     }

//     public override void RefreshTile(Vector3Int position, ITilemap tilemap){
//         for(int x = -1; x < 1; x++){
//             for(int y = -1; y < 1; y++){
//                 Vector3Int nPos = new Vector3Int(position.x)
//             }
//         }
//     }

}
