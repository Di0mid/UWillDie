using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputProvider : MonoBehaviour
{
    private InputActions _inputActions;



    public bool IsFireHeld { get; private set; }
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    
    
    
    private void Awake()
    {
        _inputActions = new InputActions();
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
        
        _inputActions.Player.Fire.performed += Fire_performed;
        _inputActions.Player.Fire.canceled += Fire_canceled;
        _inputActions.Player.Move.performed += Move_performed;
        _inputActions.Player.Move.canceled += Move_performed;
        _inputActions.Player.Look.performed += Look_performed;
        _inputActions.Player.Look.canceled += Look_performed;
    }

    private void OnDisable()
    {
        _inputActions.Player.Disable();
        
        _inputActions.Player.Fire.performed -= Fire_performed;
        _inputActions.Player.Fire.canceled -= Fire_canceled;    
        _inputActions.Player.Move.performed -= Move_performed;
        _inputActions.Player.Move.canceled -= Move_performed;
        _inputActions.Player.Look.performed -= Look_performed;
        _inputActions.Player.Move.canceled -= Look_performed;
    }

    
    
    private void Move_performed(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    private void Look_performed(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
    }

    private void Fire_performed(InputAction.CallbackContext context)
    {
        IsFireHeld = true;
    }
    
    private void Fire_canceled(InputAction.CallbackContext context)
    {
        IsFireHeld = false;
    }
}