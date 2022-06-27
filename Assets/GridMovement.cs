using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    public static event Action<Vector3,int> OnStart;
   // public static event Action<Vector3, Vector2,int> OnMove;
    public delegate bool CanMove(Vector3 WorldPos, Vector2 Direction, int canMoveId);
    public static CanMove OnMove;
    public int id;
    public int canMoveId;
    void Start()
    {
        OnStart?.Invoke(transform.position,id);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Move(Vector2 direction)
    {
        if(OnMove.Invoke(transform.position,direction, canMoveId))
        {
            transform.position += (Vector3)direction;
        }
    }
}
