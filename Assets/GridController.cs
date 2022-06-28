using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Grid<GridObject> grid;
    public int x, y;
    public float cellsize;
    public Vector3 origin;
    public Sprite[] sprites;
    public GameObject tilePrefab;
    void Awake()
    {
        grid = new Grid<GridObject>(x, y, cellsize, origin);
        grid.InitializeGrid(new GridObject(0,sprites[0]),new GridObject(2, sprites[2]));
        InitializeGrid();
        
    }
    private void OnEnable()
    {
        GridMovement.OnStart += (pos, id) => grid.SetValue(pos, new GridObject(id, sprites[id]));
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
        GridObject neighbourId = grid.GetValue(WorldPos + (Vector3)Direction);
        if (neighbourId.id == 0)
        {
            canMove = true;
        }
        else if (CanMoveId != 0 && neighbourId.id == CanMoveId)
        {
            //empujar caja
        }
        
        if (canMove)
        {
            grid.SetValue(WorldPos, new GridObject(0, sprites[0]));
            grid.SetValue(WorldPos + (Vector3)Direction, new GridObject(id, sprites[id]));
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
                tile.GetComponent<SpriteRenderer>().sprite = grid.GetValue(x, y).sprite;
            }
        }
    }
}

