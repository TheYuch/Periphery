using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public Tilemap tilemap;

    [SerializeField]
    private List<TileData> tileDatas;

    private Dictionary<TileBase, TileData> tileDataDict;

    private void Awake() //compatability stuff
    {
        tileDataDict = new Dictionary<TileBase, TileData>();

        foreach(var tileData in tileDatas)
        {
            foreach(var tile in tileData.tiles)
            {
                tileDataDict.Add(tile, tileData);
            }
        }
    }

    public TileData getTileData(Vector2 worldPos)
    {
        Vector3Int gridPos = tilemap.WorldToCell(worldPos);
        TileBase tile = tilemap.GetTile(gridPos);

        if (tile == null) return null;
        
        return tileDataDict[tile];
    }

    public TileData getTileData(int x, int y)
    {
        Vector3Int gridPos = new Vector3Int(x, y, 0);
        TileBase tile = tilemap.GetTile(gridPos);

        if (tile == null) return null;

        return tileDataDict[tile];
    }
}
