using System.Collections.Generic;
using UnityEngine;
using AutoMaze.Generic;

struct MazeLink
{
    public Vector2 point1;
    public Vector2 point2;
}

public static class Maze
{

    public static MazeNode[,] GetMazeMap(int width, int height, Vector2 start)
    {
        var linkList = GetMazeLink_RPA(width, height, start);
        int mapWidth = 2 * width + 1;
        int mapHeight = 2 * height + 1;
        MazeNode[,] map = new MazeNode[mapWidth, mapHeight];
        ClearMap(map);
        linkList.ForEach((link) =>
        {
            var p1 = link.point1;
            var p2 = link.point2;
            var x1 = (int)p1.x * 2 + 1;
            var y1 = (int)p1.y * 2 + 1;
            map[x1, y1].isWalkable = true;
            var x2 = (int)p2.x * 2 + 1;
            var y2 = (int)p2.y * 2 + 1;
            map[x2, y2].isWalkable = true;

            if (y1 == y2) map[Mathf.Min(x1, x2) + 1, y1].isWalkable = true;       // Horizontal break through.
            if (x1 == x2) map[x1, Mathf.Min(y1, y2) + 1].isWalkable = true;       // Vertical break through.
        });
        return map;
    }

    static void ClearMap(MazeNode[,] map)
    {
        var mapWidth = map.GetLength(0);
        var mapHeight = map.GetLength(0);
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                MazeNode node;
                node.x = i;
                node.y = j;
                node.isWalkable = false;
                map[i, j] = node;
            }
        }
    }

    static List<MazeLink> GetMazeLink_RPA(int width, int height, Vector2 start)
    {
        bool[,] searchArray = new bool[width, height];
        List<MazeLink> maze = new List<MazeLink>();
        Stack<Vector2> searchStack = new Stack<Vector2>();
        int x = (int)start.x;
        int y = (int)start.y;
        searchArray[x, y] = true;
        searchStack.Push(start);
        while (searchStack.Count > 0)
        {
            var curPos = searchStack.Peek();
            if (!TryGetNeighbour(curPos, width, height, searchArray, out var neighbour))
            {
                searchStack.Pop();
                continue;
            }

            searchStack.Push(neighbour);
            searchArray[(int)neighbour.x, (int)neighbour.y] = true;
            maze.Add(new MazeLink { point1 = curPos, point2 = neighbour });
        }

        return maze;
    }

    static bool TryGetNeighbour(Vector2 node, int width, int height, bool[,] searchArray, out Vector2 neighbour)
    {
        neighbour = node;

        List<Vector2> allNeighbours = new List<Vector2>();
        neighbour = node + new Vector2(1, 0);
        if (!IsOutBorder(neighbour, width, height) && !searchArray[(int)neighbour.x, (int)neighbour.y]) allNeighbours.Add(neighbour);
        neighbour = node + new Vector2(-1, 0);
        if (!IsOutBorder(neighbour, width, height) && !searchArray[(int)neighbour.x, (int)neighbour.y]) allNeighbours.Add(neighbour);
        neighbour = node + new Vector2(0, 1);
        if (!IsOutBorder(neighbour, width, height) && !searchArray[(int)neighbour.x, (int)neighbour.y]) allNeighbours.Add(neighbour);
        neighbour = node + new Vector2(0, -1);
        if (!IsOutBorder(neighbour, width, height) && !searchArray[(int)neighbour.x, (int)neighbour.y]) allNeighbours.Add(neighbour);

        if (allNeighbours.Count == 0) return false;

        neighbour = allNeighbours[Random.Range(0, allNeighbours.Count)];
        return true;
    }

    static bool IsOutBorder(Vector2 node, int width, int height)
    {
        var x = (int)node.x;
        var y = (int)node.y;
        return x >= width || x < 0 || y >= height || y < 0;
    }

}
