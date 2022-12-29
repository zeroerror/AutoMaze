using System.Collections.Generic;
using UnityEngine;
using AutoMaze.Generic;

public static class Maze_Easy
{

    public static List<MazeLink> GenEastMaze(int width, int height)
    {
        bool[,] searchArray = new bool[width, height];
        List<MazeLink> maze = new List<MazeLink>();
        Stack<Vector2> searchStack = new Stack<Vector2>();
        Vector2 start = Vector2.zero;
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

            // - 打通道路
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
