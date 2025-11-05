using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private int defaultSize = 20;
    [SerializeField] private int maxSize = 100;
    [SerializeField] private Transform container;
    
    
    
    private IObjectPool<Bullet> _pool;



    private void Awake()
    {
        _pool = new ObjectPool<Bullet>(
            Create,
            OnGet,
            OnRelease,
            OnBulletDestroy,
            true,
            defaultSize,
            maxSize);
        
        var prewarm = new List<Bullet>(defaultSize);
        for (var i = 0; i < defaultSize; i++)
        {
            prewarm.Add(_pool.Get());
        }
        
        foreach (var bullet in prewarm)
        {
            _pool.Release(bullet);
        }
    }


    
    public Bullet Get()
    {
        var bullet = _pool.Get();
        bullet.Init(_pool);
        
        return bullet;
    }
    
    
    
    private void OnBulletDestroy(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    private void OnRelease(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        bullet.transform.parent = container;
    }

    private void OnGet(Bullet bullet)
    {
        bullet.transform.parent = null;
        bullet.gameObject.SetActive(true);
    }

    private Bullet Create()
    {
        return Instantiate(bulletPrefab, container);
    }
}