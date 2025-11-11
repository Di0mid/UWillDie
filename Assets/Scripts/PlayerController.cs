using UnityEngine;

[RequireComponent(typeof(CharacterController),  typeof(PlayerInputProvider))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    
    
    
    private CharacterController _characterController;
    private PlayerInputProvider _playerInputProvider;
    private Plane _plane;
    private Camera _camera;



    private void Awake()
    {
        _plane = new  Plane(Vector3.up, Vector3.zero);
        
        _characterController = GetComponent<CharacterController>();
        _playerInputProvider = GetComponent<PlayerInputProvider>();
        
        _camera = Camera.main;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
    }
    
    
    
    private void HandleMovement()
    {
        var input = _playerInputProvider.MoveInput;
        if (input == Vector2.zero)
        {
            return;
        }
        
        var direction = new Vector3(input.x, 0, input.y);
        
        _characterController.Move(direction * (moveSpeed * Time.deltaTime));
    }

    private void HandleRotation()
    {
        var mousePosition = _playerInputProvider.LookInput;
        
        var ray = _camera.ScreenPointToRay(mousePosition);

        if (!_plane.Raycast(ray, out float distance)) 
            return;
        
        var worldMousePosition = ray.GetPoint(distance);
            
        var direction = worldMousePosition - transform.position;
        direction.y = 0;
            
        transform.rotation = Quaternion.LookRotation(direction);
    }
}