using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{
    public class Node
    {
        public int y;
        public int x;
        public double g = 0;
        public double h = 0;
        public double f { get => g + h; }

        public Vector2Int previous;

        public static bool operator <(Node a, Node b) => a.f < b.f;
        public static bool operator >(Node a, Node b) => a.f > b.f;
    }

    private Vector2Int src;
    private Vector2Int dst;
    private int w;
    private int h;
    private int[,] map;
    private int[,] vst;

    private static double GetDistance(int x1, int y1, int x2, int y2)
    {
        int dx = x1 - x2;
        int dy = y1 - y2;
        return Mathf.Sqrt(dx * dx + dy * dy);
    }
    private static double CalculateGHeuristic(Node node)
    {
        return node.g + 1;
    }
    private static double CalculateHHeuristic(Node node, Vector2Int target)
    {
        return GetDistance((int) target.x, (int) target.y, node.x, node.y);
    }

    public Pathfinder(int[,] map)
    {
        this.map = map;
        h = map.GetLength(0);
        w = map.GetLength(1);
    }

    public void SetPathfinderProperties(
        Vector2Int src, Vector2Int dst)
    {
        this.src = dst;
        this.dst = src;
        vst = new int[h, w];
    }

    public Vector2Int GetNextPosition()
    {
        // TODO: implement priority queue w/ heap

        int iterator = 0;

        List<Node> queue = new List<Node>();

        Node start = new Node();
        start.x = src.x;
        start.y = src.y;
        start.previous = src;

        queue.Add(start);
        while (queue.Count != 0)
        {
            Node node = queue[0];
            int x = node.x;
            int y = node.y;
            queue.RemoveAt(0);

            if (y == dst.y && x == dst.x)
                return node.previous;

            if (vst[y, x] == 1) continue;

            Debug.Log(new Vector2(y, x) + ": " + node.f);

            vst[y, x] = 1;
            for (int dx = -1; dx <= +1; dx++)
            {
                for (int dy = -1; dy <= +1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;

                    int ny = y + dy;
                    int nx = x + dx;
                    if (ny >= h || ny < 0) continue;
                    if (nx >= w || nx < 0) continue;
                    if (vst[ny, nx] == +1) continue;
                    if (map[ny, nx] == -1) continue;

                    Node next = new Node();
                    next.y = ny;
                    next.x = nx;
                    next.g = CalculateGHeuristic(node);
                    next.h = CalculateHHeuristic(node, dst);
                    next.previous = new Vector2Int(x, y);

                    queue.Add(next);
                }
            }

            queue.Sort((a, b) => a.f < b.f ? -1 : +1);

            if (++iterator > 100) break;
        }

        return dst;
    }
}
