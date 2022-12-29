using UnityEngine;
using AutoMaze.Generic;

namespace AutoMaze.Sample
{

    public class Sample : MonoBehaviour
    {

        public int width;
        public int height;
        public int startX;
        public int startY;
        MazeNode[,] map;

        void OnGUI()
        {
            if (GUILayout.Button("生成迷宫"))
            {
                map = Maze.GetMazeMap(width, height, startX, startY);
            }
        }

        void OnDrawGizmos()
        {
            if (map == null) return;
            if (map.GetLength(0) == 0) return;
            if (map.GetLength(1) == 0) return;

            Gizmos.color = Color.black;
            DrawMap();
        }

        void DrawMap()
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (!map[i, j].isWalkable) Gizmos.DrawCube(new Vector2(i, j), new Vector3(1, 1, 1));
                }
            }
        }

    }

}
