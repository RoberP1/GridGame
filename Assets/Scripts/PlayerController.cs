using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public InputAction playerControls;
    public UnityEvent<Vector2> OnMove;
    private bool finished;
    private void Awake()
    {
        GameManager.OnFinished += () => finished = true;
    }
    private void OnEnable()
    {
        
        playerControls.Enable();
        playerControls.performed += _ => Move() ;
        
    }
    private void OnDisable()
    {
        playerControls.Disable();
        playerControls.performed -= _ => Move();
    }
    void Move()
    {
        if(!finished)OnMove?.Invoke(playerControls.ReadValue<Vector2>());
    }
    
}
