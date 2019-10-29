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


}
