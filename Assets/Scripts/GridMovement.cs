using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{

    [SerializeField] private Animator anim;
    private int verticalAnim, horizontalAnim;
    public int id;
    public int canMoveId;
    private GridController gridController;
    [SerializeField]private bool undo;
    private Vector3 destination;
    [SerializeField] private float interpolationSpeed;
    void Start()
    {
        destination = transform.position;
        gridController = FindObjectOfType<GridController>();
        if (anim != null)
        {
            verticalAnim = Animator.StringToHash("Vertical");
            horizontalAnim = Animator.StringToHash("Horizontal");
        }
        
    }
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, destination, interpolationSpeed * Time.deltaTime);
        bool animationfinish = false;

        if (Vector3.SqrMagnitude(transform.position - destination) <= 0.02)
        {
            transform.position = destination;
            if (anim != null) AnimationFinish();
        }
    }


    public void Move(Vector2 direction)
    {
        bool canMove = false;
        if (direction.sqrMagnitude == 0) return ;
        //Debug.Log(direction + " " + gameObject.name);
        
        canMove = gridController.checkDirection(transform.position, direction, canMoveId, id);
        if (canMove)
        {
            if(id == 3)Debug.Log(undo);
            if(anim!=null && !undo)ActiveAnim(direction);
            if (!undo) destination = transform.position + (Vector3)direction;
            else 
            {
                destination = transform.position + (Vector3)direction;
                transform.position = destination; 
            }
            gridController.grid.GetXY(transform.position, out int x, out int y);
            if (id == 0 && !undo) { gridController.Move(direction); }
            undo = false;
            //Debug.Log("("+x+","+y+") " + gameObject);
        }

    }

    private void ActiveAnim(Vector2 direction)
    {
        anim.SetFloat(verticalAnim, direction.y);
        anim.SetFloat(horizontalAnim, direction.x);
    }

    private void OnEnable()
    {
        
        GridController.OnUndoMove += (direction) => undo = true; 
        if (id == 0) GridController.OnUndoMove += Move;
    }
    private void OnDisable()
    {
        if (id == 0) GridController.OnUndoMove -= Move;
        //if (id == 0) GridController.OnUndoMove -= (direction) => undo = true;
    }
    public void AnimationFinish()
    {
        anim?.SetFloat(verticalAnim, 0);
        anim?.SetFloat(horizontalAnim, 0);
    }
}
