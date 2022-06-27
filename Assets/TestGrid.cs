using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGrid : MonoBehaviour
{
    public Grid<int> grid;
    public int x, y;
    public float cellsize;
    public Vector3 origin;
        
    void Awake()
    {
        grid = new Grid<int>(x, y, cellsize, origin);
        GridMovement.OnStart += (pos, id) => { grid.SetValue(pos, id); Debug.Log(grid.GetValue(pos)); };
        GridMovement.OnMove += checkDirection;
    }

    void Update()
    {
        
    }

    bool checkDirection(Vector3 WorldPos,Vector2 Direction, int CanMoveId)
    {
        bool canMove = false;
        int neighbourId = grid.GetValue(WorldPos + (Vector3)Direction);
        if (neighbourId == 0)
        {
            canMove = true;
        }
        else if (CanMoveId != 0 && neighbourId == CanMoveId)
        {
            //empujar caja
        }
        else
        {
            canMove = false;
        }
        Debug.Log("neighbourId = " + neighbourId);
        Debug.Log("canMove = " + canMove);

        return canMove;
    }

}
