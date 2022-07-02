using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    //ids: 0 = void,  1 = player,  2 = block,  3 = box,  4 = target, 5 = target complate

    //events
    public static event Action OnTargetSet;
    public static event Action<Vector2> OnUndoMove;

    //grids
    public Grid<int> grid;
    public Grid<GameObject> gameObjectsGrid;


    public GridScriptableObject gridScriptableObject;
    public GridSize gridSize;

    public int x, y;
    public float cellsize;
    public Vector3 origin;



    public GameObject[] prefabs;

    private bool gridstarted = false;

    [SerializeField] private List<GridObject> moves = new List<GridObject>();
    [SerializeField] private int currentMove;
    [SerializeField] private GameObject lastBox = null;

    [SerializeField] private List<GameObject> targets = new List<GameObject>();
    void Awake()
    {


        SetGrid();

        grid = new Grid<int>(x, y, cellsize, origin);
            

        grid.InitializeGridBorders(0, 2);
   

        gameObjectsGrid = new Grid<GameObject>(x, y, cellsize, origin);

        //blocks
        foreach (var Block in gridScriptableObject.Block)
        {
            AddBorder(Block.x, Block.y);
        }


        if (!gridstarted) InitializeGrids();


    }
    private void Start()
    {

        //player
        SpawnPlayer(gridScriptableObject.Player.x, gridScriptableObject.Player.y);

        //caja
        foreach (var Box in gridScriptableObject.Boxes)
        {
            AddBox(Box.x, Box.y);
        }


        //target
        foreach (var target in gridScriptableObject.targets)
        {
            AddTarget(target.x, target.y);
        }
    }



    private void SetGrid()
    {
        x = gridSize.x;
        y = gridSize.y;
        cellsize = gridSize.cellsize;
        origin = gridSize.origin;
    }
    private void SpawnPlayer(int x, int y)
    {
        Instantiate(prefabs[1], grid.GetWorldPos(x, y) + (Vector3)Vector2.one * cellsize / 2, Quaternion.identity);
    }
    private void AddBorder(int x,int y)
    {
        grid.SetValue(x, y,2);
    }
    private void AddBox(int x, int y)
    {
        grid.SetValue(x, y, 3);
        GameObject box = Instantiate(prefabs[3], grid.GetWorldPos(x, y) + (Vector3)Vector2.one * cellsize / 2, Quaternion.identity);
        gameObjectsGrid.SetValue(x, y, box);
    }   
    private void AddBox(Vector3 WorldPos)
    {
        grid.SetValue(WorldPos, 3);
        GameObject box = Instantiate(prefabs[3], WorldPos + (Vector3)Vector2.one * cellsize / 2, Quaternion.identity);
        gameObjectsGrid.SetValue(WorldPos, box);
    }
    private void AddTarget(int x, int y)
    {
        grid.SetValue(x, y, 4);
        GameObject target = Instantiate(prefabs[4], grid.GetWorldPos(x, y) + (Vector3)Vector2.one * cellsize / 2, Quaternion.identity);
        gameObjectsGrid.SetValue(x, y, target);
        OnTargetSet?.Invoke();
    }
    private void AddTarget(Vector3 WorldPos)
    {
        grid.SetValue(WorldPos, 4);
        GameObject target = Instantiate(prefabs[4], WorldPos + (Vector3)Vector2.one * cellsize / 2, Quaternion.identity);
        gameObjectsGrid.SetValue(WorldPos, target);
        OnTargetSet?.Invoke();
    }

    private void OnEnable()
    {
        //GridMovement.OnMove += Move;
        GameManager.OnUndo += UndoMove;
    }
    private void OnDisable()
    {
        //GridMovement.OnMove -= Move;
        GameManager.OnUndo -= UndoMove;
    }

    public void Move(Vector2 Direction)
    {
        currentMove++;
        moves.Add(new GridObject(Direction,lastBox));
        //Debug.Log(moves[currentMove-1]?.direction);
    }
    public void UndoMove()
    {
        if (moves.Count == 0) return;
        currentMove--;
        GridObject turn = moves[currentMove];
        Vector2 direction = moves[currentMove].direction * -1;
        GameObject box = moves[currentMove].box;
        OnUndoMove?.Invoke(direction);
        box?.GetComponent<GridMovement>().Move(direction);
        moves.Remove(turn);
    }
    public bool checkDirection(Vector3 WorldPos, Vector2 Direction, int CanMoveId, int id)
    {
        lastBox = null;
        bool canMove = false;
        bool leavetarget = false;
        int neighbourId = grid.GetValue(WorldPos + (Vector3)Direction);
        if (neighbourId == 0 || neighbourId == 4)
        {//si es void mover
            canMove = true;
        }
        else if (CanMoveId != 0 && neighbourId == CanMoveId)
        {
            //empujar caja
            canMove = MoveBox(WorldPos, Direction, CanMoveId);
        }
        else if (CanMoveId != 0 && neighbourId == 5)
        {
            //empujar caja
            canMove = MoveBox(WorldPos, Direction, CanMoveId);
        }
        if (canMove && CanMoveId == 0) //si puedo mover la caja
        {
            if (grid.GetValue(WorldPos) == 5) leavetarget = true;
            

            if (neighbourId == 4) // si llego al target
            {
                GameObject OTarget = gameObjectsGrid.GetValue(WorldPos + (Vector3)Direction);
                Target target = OTarget.GetComponent<Target>() ;
                target.complate();
                targets.Add(OTarget);
                UpdateGrid(WorldPos, Direction, 5, leavetarget);
            }
            else
            {
                UpdateGrid(WorldPos, Direction, id, leavetarget);
            }
        }
        return canMove;
    }

    private void UpdateGrid(Vector3 WorldPos, Vector2 Direction, int id, bool leavetarget)
    {
        grid.SetValue(WorldPos + (Vector3)Direction, id);
        gameObjectsGrid.SetValue(WorldPos + (Vector3)Direction, gameObjectsGrid.GetValue(WorldPos));
        
        if (leavetarget)
        {
            grid.SetValue(WorldPos, 4);
            GameObject lastTarget = targets[targets.Count - 1];

            targets.Remove(lastTarget);
            gameObjectsGrid.SetValue(WorldPos, lastTarget);

            lastTarget.GetComponent<Target>().UndoComplate();
        }
        else
        {
            grid.SetValue(WorldPos, 0);
            gameObjectsGrid.SetValue(WorldPos, null);
        }
        
    }

    private bool MoveBox(Vector3 WorldPos, Vector2 Direction, int CanMoveId)
    {
        bool canMove;
        GameObject box = gameObjectsGrid.GetValue(WorldPos + (Vector3)Direction);

        box.GetComponent<GridMovement>().Move(Direction);//mueve la caja
        if (grid.GetValue(WorldPos + (Vector3)Direction) == CanMoveId)//si la caja sigue ahi entonces no pudo moverse 
        {
            canMove = false;
        }
        else
        {
            canMove = true;
            lastBox = box;
        }

        return canMove;
    }

    void InitializeGrids()
    {
        for (int x = 0; x < this.x; x++)
        {
            for (int y = 0; y < this.y; y++)
            {
                int id = grid.GetValue(x, y);
                Instantiate(prefabs[id], grid.GetWorldPos(x, y) + (Vector3)Vector2.one * cellsize / 2, Quaternion.identity);
            }
        }
        gridstarted = true;
    }
}

