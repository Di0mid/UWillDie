using UnityEngine;

[RequireComponent(typeof(PlayerInputProvider))]
public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private float fireForce = 20f;
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private Transform firePoint;


    
    private float _lastFireTime = float.MinValue;

    private PlayerInputProvider _input;
    private BulletPool _bulletPool;
    

    
    private void Awake()
    {
        _input = GetComponent<PlayerInputProvider>();
        _bulletPool = GetComponent<BulletPool>();
    }

    private void Update()
    {
        if (_input.IsFireHeld)
        {
            Fire();
        }
    }
    
    
    
    private void Fire()
    {
        if (!(Time.time > _lastFireTime + fireRate))
        {
            return;
        }
        
        _lastFireTime = Time.time;

        var bullet = _bulletPool.Get(firePoint.position, firePoint.rotation);
        bullet.Fire(firePoint.forward, fireForce);
    }
}