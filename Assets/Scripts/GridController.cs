using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    //ids: 0 = void,  1 = player,  2 = block,  3 = box,  4 = target, 5 = target complate

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
        grid.SetValue(2, 7, 4);

        if (!gridstarted) InitializeGrids();

    }
    private void OnEnable()
    {
        GridMovement.OnStart += (pos, id) => { grid.SetValue(pos, id); };
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
        if (neighbourId == 0 || neighbourId == 4)
        {//si es void mover
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
        if (canMove && CanMoveId == 0)
        {


            //si neighbourId es 4
            //        setear valor grid a 5 en objetivo
            //        setear valor grid a 0 en posicion
            //        llamar a funcion del target completado
            //        setear targetComplate como gameObjectsGrid del objetivo
            //else
            //        setear valor grid a id en objetivo
            //        setear valor grid a 0 en posicion
            //        destruya el gameObjectsGrid del objetivo
            //        setear caja como gameObjectsGrid del objetivo


            //setear void como gameObjectsGrid de la posicion

            if (neighbourId == 4)
            {
                grid.SetValue(WorldPos + (Vector3)Direction, 2);
                grid.SetValue(WorldPos, 0);

                GameObject target = gameObjectsGrid.GetValue(WorldPos + (Vector3)Direction);
                //target.GetComponent<Target>().complate();

                GameObject targetComplate = Instantiate(prefabs[grid.GetValue(WorldPos + (Vector3)Direction)], WorldPos+ (Vector3)Direction, Quaternion.identity);
            }
            else
            {
                grid.SetValue(WorldPos + (Vector3)Direction, id);
                grid.SetValue(WorldPos, 0);

                Destroy(gameObjectsGrid.GetValue(WorldPos + (Vector3)Direction));
                gameObjectsGrid.SetValue(WorldPos + (Vector3)Direction, gameObjectsGrid.GetValue(WorldPos));
            }

            GameObject tile = Instantiate(prefabs[grid.GetValue(WorldPos)], WorldPos, Quaternion.identity);
            gameObjectsGrid.SetValue(WorldPos, tile);

        }
        /*
        if (canMove)//actualizar grids
        {
            grid.SetValue(WorldPos, 0);
            grid.SetValue(WorldPos + (Vector3)Direction, id);


            Destroy(gameObjectsGrid.GetValue(WorldPos + (Vector3)Direction));
            gameObjectsGrid.SetValue(WorldPos + (Vector3)Direction, gameObjectsGrid.GetValue(WorldPos));

            GameObject tile = Instantiate(prefabs[grid.GetValue(WorldPos)],WorldPos, Quaternion.identity);
            gameObjectsGrid.SetValue(WorldPos, tile);
        }
        //Debug.Log("neighbourId = " + neighbourId.id);
        //Debug.Log("canMove = " + canMove);
        */
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

