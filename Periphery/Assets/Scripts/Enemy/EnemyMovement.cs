using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyMovement : MonoBehaviour
{
    public GameObject player;

    // enemy properties
    int range = 500;

    // tilemap properties
    private int[,] map;
    public Tilemap tilemap;
    public TileBase[] tiles;
    public int[] obstacleTilesID;
    public int[] passableTilesID;

    // create pathfinder
    Pathfinder pathfinder;

    private void Start()
    {
        BoundsInt bounds = tilemap.cellBounds;
        int w = bounds.size.x;
        int h = bounds.size.y;

        // TODO: get custome tiles with ID property
        tiles = tilemap.GetTilesBlock(bounds);

        Debug.Log(bounds);

        map = new int[h, w];
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                TileBase tile = tiles[x + y * w];
                if (tile == null) continue;
                //else if (obstacleTilesID.Contains(tile.id)) map[y, x] = -1;
                //else if (passableTilesID.Contains(tile.id)) map[y, x] = +1;

                map[y, x] = 1; // TODO: remove, temporary
            }
        }

        pathfinder = new Pathfinder(map);
    }

    private void FixedUpdate()
    {
        if (!CheckPlayerVisible())
            return;

        Vector2Int src = (Vector2Int) tilemap.WorldToCell(transform.position);
        Vector2Int dst = (Vector2Int) tilemap.WorldToCell(player.transform.position);

        Debug.Log("SRC: " + src);
        Debug.Log("DST: " + dst);

        pathfinder.SetPathfinderProperties(src, dst);
        Vector2Int nextPosition = pathfinder.GetNextPosition();

        Vector3 cur = transform.position;
        Vector3 nxt = tilemap.GetCellCenterWorld((Vector3Int) nextPosition);
        transform.position = Vector3.MoveTowards(cur, nxt, Time.deltaTime);
    }

    private bool CheckPlayerVisible()
    {
        // TODO: raycast from enemy to player to check if blocked

        float deltaX = player.transform.position.x - transform.position.x;
        float deltaY = player.transform.position.y - transform.position.y;
        float distance = Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY);
        if (distance > range) return false;

        return true;
    }
}
