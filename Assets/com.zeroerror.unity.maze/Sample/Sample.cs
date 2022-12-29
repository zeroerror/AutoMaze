using System.Collections.Generic;
using UnityEngine;
using AutoMaze.Generic;

public class Sample : MonoBehaviour
{

    int width;
    int height;
    List<MazeLink> linkList;

    struct wall
    {
        public bool rowWallBroken;
        public bool columnWallBroken;
    }

    void OnGUI()
    {

        GUILayout.Label($"宽度:{width}");
        width = (int)GUILayout.HorizontalSlider(width, 0, 20);
        GUILayout.Label($"高度:{height}");
        height = (int)GUILayout.HorizontalSlider(height, 0, 20);
        if (GUILayout.Button("生成迷宫"))
        {
            linkList = Maze_Easy.GenEastMaze(width, height);
        }
    }

    void OnDrawGizmos()
    {
        if (linkList == null) return;
        if (linkList.Count == 0) return;

        Gizmos.color = Color.gray;
        DrawNodes();

        wall[,] wallInfos = GetWallInfos();
        Gizmos.color = Color.green;
        DrawWall(wallInfos);
    }

    void DrawWall(wall[,] wallInfos)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (!wallInfos[i, j].columnWallBroken) Gizmos.DrawLine(new Vector2(i - 0.5f, j - 0.5f), new Vector2(i - 0.5f, j - 0.5f + 1));
                if (!wallInfos[i, j].rowWallBroken) Gizmos.DrawLine(new Vector2(i - 0.5f, j - 0.5f), new Vector2(i - 0.5f + 1, j - 0.5f));
            }
        }
    }

    wall[,] GetWallInfos()
    {
        wall[,] wallInfos = new wall[width, height];
        for (int i = 0; i < linkList.Count; i++)
        {
            var link = linkList[i];
            var p1 = link.point1;
            var p2 = link.point2;
            var x1 = (int)p1.x;
            var y1 = (int)p1.y;
            var x2 = (int)p2.x;
            var y2 = (int)p2.y;
            if (y1 == y2)
            {
                // 左右之间
                if (x1 < x2) wallInfos[x2, y2].columnWallBroken = true;
                if (x1 > x2) wallInfos[x1, y1].columnWallBroken = true;
            }
            else
            {
                // 上下之间
                if (y1 < y2) wallInfos[x2, y2].rowWallBroken = true;
                if (y1 > y2) wallInfos[x1, y1].rowWallBroken = true;
            }
        }

        return wallInfos;
    }

    void DrawNodes()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Gizmos.DrawCube(new Vector2(i, j), new Vector3(0.98f, 0.98f, 0.98f));
            }
        }
    }

}
