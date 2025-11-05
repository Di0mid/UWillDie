using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private int damage = 10;


    
    private float _lifeTimer;
    
    private IObjectPool<Bullet> _pool;
    private Rigidbody _rigidbody;



    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _lifeTimer -=  Time.deltaTime;
        if (_lifeTimer <= 0)
        {
            ReleaseToPool();    
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage);
        }
        
        ReleaseToPool();
    }
    
    
    
    public void Init(IObjectPool<Bullet> pool)
    {
        _pool = pool;
        _lifeTimer = lifetime;
    }

    public void Fire(Vector3 direction, float force)
    {
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.AddForce(direction * force, ForceMode.VelocityChange);
    }
    
    private void ReleaseToPool()
    {
        _pool?.Release(this);        
    }
}