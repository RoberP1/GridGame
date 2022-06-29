using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Grid<int> grid;
    public Grid<GameObject> gameObjectsGrid;
    public int x, y;
    public float cellsize;
    public Vector3 origin;
    public GameObject[] prefabs;
    //public GameObject tilePrefab;
    //public GameObject borderPrefab;
    //public GameObject boxPrefab;
    private bool gridstarted = false;
    void Awake()
    {
        grid = new Grid<int>(x, y, cellsize, origin);
        gameObjectsGrid = new Grid<GameObject>(x, y, cellsize, origin);
        
        grid.InitializeGridBorders(0,2);

        grid.SetValue(4, 3, 3);

        

    }
    private void OnEnable()
    {
        GridMovement.OnStart += (pos, id) => { grid.SetValue(pos, id); if(!gridstarted) InitializeGrids(); };
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
    public bool checkDirection(Vector3 WorldPos, Vector2 Direction, int CanMoveId, int id)
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
            GameObject box = gameObjectsGrid.GetValue(WorldPos + (Vector3)Direction);
            box.GetComponent<GridMovement>().Move(Direction);
            if (grid.GetValue(WorldPos + (Vector3)Direction) == CanMoveId)
            {
                canMove = false;
            }else canMove = true;
        }
        if (canMove)
        {
            grid.SetValue(WorldPos, 0);
            grid.SetValue(WorldPos + (Vector3)Direction, id);


            Destroy(gameObjectsGrid.GetValue(WorldPos + (Vector3)Direction));
            gameObjectsGrid.SetValue(WorldPos + (Vector3)Direction, gameObjectsGrid.GetValue(WorldPos));

            GameObject tile = Instantiate(prefabs[0],WorldPos, Quaternion.identity);
            gameObjectsGrid.SetValue(WorldPos, tile);
        }
        //Debug.Log("neighbourId = " + neighbourId.id);
        //Debug.Log("canMove = " + canMove);
        return canMove;
    }
    void InitializeGrids()
    {
        for (int x = 0; x < this.x; x++)
        {
            for (int y = 0; y < this.y; y++)
            {
                GameObject tile;
                int id = grid.GetValue(x, y);
                switch (id)
                {
                    case 1:
                        tile = GameObject.FindGameObjectWithTag("Player");
                        break;
                    default:
                        tile = Instantiate(prefabs[id], grid.GetWorldPos(x, y) + (Vector3)Vector2.one * cellsize / 2, Quaternion.identity);
                        break;
                }
                gameObjectsGrid.SetValue(x, y, tile);
            }
        }
        gridstarted = true;
    }
}

