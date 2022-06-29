using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    public static event Action<Vector3,int> OnStart;
   // public static event Action<Vector3, Vector2,int> OnMove;
    public delegate bool CanMove(Vector3 WorldPos, Vector2 Direction, int canMoveId,int id);
    public static CanMove OnMove;
    public int id;
    public int canMoveId;
    private GridController gridController;
    void Start()
    {
        //OnStart?.Invoke(transform.position,id);
        gridController = FindObjectOfType<GridController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Move(Vector2 direction)
    {
        bool canMove = false;
        if (direction.sqrMagnitude == 0) return ;
        Debug.Log(direction + " " + gameObject.name);
        //bool canMove = OnMove.Invoke(transform.position, direction, canMoveId, id);
        canMove = gridController.checkDirection(transform.position, direction, canMoveId, id);
        if (canMove)
        {
            transform.position += (Vector3)direction;
            gridController.grid.GetXY(transform.position, out int x, out int y);
            Debug.Log("("+x+","+y+") " + gameObject);
        }

    }
}
