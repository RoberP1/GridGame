using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<GridObject> {

    private int width, height;
    private float cellsize;
    private Vector3 originPos;
    private GridObject[,] gridArray;

    public Grid(int width, int height, float cellsize, Vector3 originPos)
    {
        this.width = width;
        this.height = height;
        this.cellsize = cellsize;
        this.originPos = originPos;

        gridArray = new GridObject[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                Debug.DrawLine(GetWorldPos(x, y), GetWorldPos(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPos(x, y), GetWorldPos(x+1, y), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPos(0, height), GetWorldPos(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPos(width,0), GetWorldPos(width, height), Color.white, 100f);
    }
    public Vector3 GetWorldPos(int x, int y)
    {
        return new Vector3(x, y) * cellsize + originPos;
    }

    private void GetXY(Vector3 worldPos, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPos - originPos).x / cellsize);
        y = Mathf.FloorToInt((worldPos - originPos).y / cellsize);
    }
    
    public void SetValue(int x, int y, GridObject value)
    {
        if(x>=0&& y>=0&& x < width && y < height)
        {
            gridArray[x, y] = value;
        }
    }
    public void SetValue(Vector3 worldPos, GridObject value)
    {
        int x, y;
        GetXY(worldPos, out x, out y);
        SetValue(x, y, value);
    }
    public GridObject GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }else return default(GridObject);
    }
    public GridObject GetValue(Vector3 worldPos)
    {
        int x, y;
        GetXY(worldPos, out x, out y);
        return GetValue(x, y);
    }

}
