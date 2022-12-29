using System;
using System.Collections.Generic;
using AutoMaze.Generic;

namespace AutoMaze
{

    public static class Maze
    {

        struct MazeLink
        {
            public Int2 point1;
            public Int2 point2;
        }

        public static MazeNode[,] GetMazeMap(int width, int height, int startX, int startY)
        {
            var linkList = GetMazeLink_RPA(width, height, new Int2(startX, startY));
            int mapWidth = 2 * width + 1;
            int mapHeight = 2 * height + 1;
            MazeNode[,] map = new MazeNode[mapWidth, mapHeight];
            ClearMap(map);
            linkList.ForEach((link) =>
            {
                var p1 = link.point1;
                var p2 = link.point2;
                var x1 = p1.x * 2 + 1;
                var y1 = p1.y * 2 + 1;
                map[x1, y1].isWalkable = true;
                var x2 = p2.x * 2 + 1;
                var y2 = p2.y * 2 + 1;
                map[x2, y2].isWalkable = true;

                if (y1 == y2) map[Math.Min(x1, x2) + 1, y1].isWalkable = true;       // Horizontal break through.
                if (x1 == x2) map[x1, Math.Min(y1, y2) + 1].isWalkable = true;       // Vertical break through.
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

        static List<MazeLink> GetMazeLink_RPA(int width, int height, Int2 start)
        {
            bool[,] searchArray = new bool[width, height];
            List<MazeLink> maze = new List<MazeLink>();
            Stack<Int2> searchStack = new Stack<Int2>();
            searchArray[start.x, start.y] = true;
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

        static bool TryGetNeighbour(Int2 node, int width, int height, bool[,] searchArray, out Int2 neighbour)
        {
            neighbour = node;

            List<Int2> allNeighbours = new List<Int2>();
            neighbour = new Int2(node.x + 1, node.y);
            if (!IsOutBorder(neighbour, width, height) && !searchArray[(int)neighbour.x, (int)neighbour.y]) allNeighbours.Add(neighbour);
            neighbour = new Int2(node.x - 1, node.y);
            if (!IsOutBorder(neighbour, width, height) && !searchArray[(int)neighbour.x, (int)neighbour.y]) allNeighbours.Add(neighbour);
            neighbour = new Int2(node.x, node.y + 1);
            if (!IsOutBorder(neighbour, width, height) && !searchArray[(int)neighbour.x, (int)neighbour.y]) allNeighbours.Add(neighbour);
            neighbour = new Int2(node.x, node.y - 1);
            if (!IsOutBorder(neighbour, width, height) && !searchArray[(int)neighbour.x, (int)neighbour.y]) allNeighbours.Add(neighbour);

            if (allNeighbours.Count == 0) return false;

            Random random = new Random();
            neighbour = allNeighbours[random.Next(0, allNeighbours.Count)];
            return true;
        }

        static bool IsOutBorder(Int2 node, int width, int height)
        {
            var x = (int)node.x;
            var y = (int)node.y;
            return x >= width || x < 0 || y >= height || y < 0;
        }

    }

}

