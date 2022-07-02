using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public InputAction playerControls;
    public UnityEvent<Vector2> OnMove;
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
        OnMove?.Invoke(playerControls.ReadValue<Vector2>());
    }
}
