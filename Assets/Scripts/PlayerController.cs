using UnityEngine;

[RequireComponent(typeof(CharacterController),  typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    
    
    
    private CharacterController _characterController;
    private PlayerInput _playerInput;
    private Plane _plane;
    private Camera _camera;



    private void Awake()
    {
        _plane = new  Plane(Vector3.up, Vector3.zero);
    }
    
    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        _camera = Camera.main;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
    }
    
    
    
    private void HandleMovement()
    {
        var input = _playerInput.GetMoveInput();
        if (input == Vector2.zero)
        {
            return;
        }
        
        var direction = new Vector3(input.x, 0, input.y);
        var motion = direction * (moveSpeed * Time.deltaTime);
        
        _characterController.Move(motion);
    }

    private void HandleRotation()
    {
        var input = _playerInput.GetLookInput();
        
        var ray = _camera.ScreenPointToRay(input);

        if (!_plane.Raycast(ray, out float distance)) 
            return;
        
        var point = ray.GetPoint(distance);
            
        var direction = point - transform.position;
        direction.y = 0;
            
        transform.rotation = Quaternion.LookRotation(direction);
    }
}