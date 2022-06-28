using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Grid<int> grid;
    public int x, y;
    public float cellsize;
    public Vector3 origin;
    public Sprite[] sprites;
    public GameObject tilePrefab;
    void Awake()
    {
        grid = new Grid<int>(x, y, cellsize, origin);
        grid.InitializeGrid(0,2);
        InitializeGrid();
        
    }
    private void OnEnable()
    {
        GridMovement.OnStart += (pos, id) => grid.SetValue(pos,id);
        GridMovement.OnMove += checkDirection;
    }
    private void OnDisable()
    {
        GridMovement.OnMove -= checkDirection;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    bool checkDirection(Vector3 WorldPos, Vector2 Direction, int CanMoveId, int id)
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
        
        if (canMove)
        {
            grid.SetValue(WorldPos, 0);
            grid.SetValue(WorldPos + (Vector3)Direction, id);
        }
        //Debug.Log("neighbourId = " + neighbourId.id);
        Debug.Log("canMove = " + canMove);
        return canMove;
    }
    void InitializeGrid()
    {
        for (int x = 0; x < this.x; x++)
        {
            for (int y = 0; y < this.y; y++)
            {
                GameObject tile =Instantiate(tilePrefab, grid.GetWorldPos(x, y) + (Vector3)Vector2.one * cellsize/2, Quaternion.identity);
                tile.GetComponent<SpriteRenderer>().sprite = sprites[grid.GetValue(x, y)];
            }
        }
    }
}

