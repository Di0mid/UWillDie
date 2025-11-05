using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private InputActions _inputActions;



    //public event Action OnFireStarted;
    public bool IsFireHeld { get; private set; }
    
    
    
    private void Awake()
    {
        _inputActions = new InputActions();
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
        
        _inputActions.Player.Fire.performed += Fire_performed;
        _inputActions.Player.Fire.canceled += Fire_canceled;
    }

    private void OnDisable()
    {
        _inputActions.Player.Disable();
        
        _inputActions.Player.Fire.performed -= Fire_performed;
        _inputActions.Player.Fire.canceled -= Fire_canceled;    
    }
    
    
    
    private void Fire_performed(InputAction.CallbackContext context)
    {
        //OnFireStarted?.Invoke();
        IsFireHeld = true;
    }
    
    private void Fire_canceled(InputAction.CallbackContext context)
    {
        IsFireHeld = false;
    }
    
    
    
    public Vector2 GetMoveInput()
    {
        return _inputActions.Player.Move.ReadValue<Vector2>();
    }

    public Vector2 GetLookInput()
    {
        return _inputActions.Player.Look.ReadValue<Vector2>();
    }
}