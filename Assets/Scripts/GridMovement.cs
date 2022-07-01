using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{


    public int id;
    public int canMoveId;
    private GridController gridController;
    private bool undo;
    void Start()
    {
        gridController = FindObjectOfType<GridController>();
        
    }
    public void Move(Vector2 direction)
    {
        bool canMove = false;
        if (direction.sqrMagnitude == 0) return ;
        //Debug.Log(direction + " " + gameObject.name);
        
        canMove = gridController.checkDirection(transform.position, direction, canMoveId, id);
        if (canMove)
        {
            transform.position += (Vector3)direction;
            gridController.grid.GetXY(transform.position, out int x, out int y);
            if (id == 0 && !undo) {gridController.Move(direction); undo = false; }
            //Debug.Log("("+x+","+y+") " + gameObject);
        }

    }
    private void OnEnable()
    {
        
        if (id == 0) GridController.OnUndoMove += (direction) => undo = true; 
        if (id == 0) GridController.OnUndoMove += Move;
    }
    private void OnDisable()
    {
        if (id == 0) GridController.OnUndoMove -= Move;
        //if (id == 0) GridController.OnUndoMove -= (direction) => undo = true;
    }
}
