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
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }
    private void Start()
    {
        //playerControls.WasPressedThisFrame
    }
    void Update()
    {
        if(playerControls.WasPressedThisFrame())
        OnMove?.Invoke(playerControls.ReadValue<Vector2>());
    }

}
