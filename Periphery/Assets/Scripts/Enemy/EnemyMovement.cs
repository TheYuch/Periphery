using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyMovement : MonoBehaviour
{
    public GameObject player;

    // enemy properties
    public int range = 500;
    public float defaultSpeed = 4f;
    private const float playerSpeedMultiplier = 0.8f; //scales down enemy speed to be always slower than player

    // tilemap properties
    private int[,] map;
    public Tilemap tilemap;
    public TileBase[] tiles;
    [SerializeField] private MapManager mapManager;

    // create pathfinder
    Pathfinder pathfinder;

    private void Start()
    {
        BoundsInt bounds = tilemap.cellBounds;
        int w = bounds.size.x;
        int h = bounds.size.y;

        tiles = tilemap.GetTilesBlock(bounds);

        map = new int[h, w];
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                TileBase tile = tiles[x + y * w];
                if (tile == null) continue;
                else if (mapManager.getTileData(x, y).isObstacle) map[y, x] = -1;
                else if (!mapManager.getTileData(x, y).isObstacle) map[y, x] = +1;
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

        //Debug.Log("SRC: " + src);
        //Debug.Log("DST: " + dst);

        pathfinder.SetPathfinderProperties(src, dst);
        Vector2Int nextPosition = pathfinder.GetNextPosition();

        Vector3 cur = transform.position;
        Vector3 nxt = tilemap.GetCellCenterWorld((Vector3Int) nextPosition);

        float speed = defaultSpeed;
        TileData tmp = mapManager.getTileData(transform.position);
        if (tmp != null) speed = tmp.walkingSpeed * playerSpeedMultiplier;
        transform.position = Vector3.MoveTowards(cur, nxt, speed * Time.fixedDeltaTime);
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
