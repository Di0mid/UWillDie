using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private EnemyAI enemyPrefab;
    [SerializeField] private int defaultSize = 20;
    [SerializeField] private int maxSize = 100;

    [Space] 
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform container;
    
    
    
    private IObjectPool<EnemyAI> _pool;

    

    private void Awake()
    {
        _pool = new ObjectPool<EnemyAI>(
            Create,
            OnGet,
            OnRelease,
            OnDestroyEnemy,
            true,
            defaultSize,
            maxSize);

        var prewarm = new List<EnemyAI>(defaultSize);
        for (var i = 0; i < defaultSize; i++)
        {
            prewarm.Add(_pool.Get());
        }
        
        foreach (var enemy in prewarm)
        {
            _pool.Release(enemy);
        }
    }

    
    
    public EnemyAI Get()
    {
        var enemy = _pool.Get();
        
        enemy.Init(_pool, playerTransform);
        
        return enemy;
    }
    
    
    
    private void OnDestroyEnemy(EnemyAI enemy)
    {
        Destroy(enemy.gameObject);
    }

    private void OnRelease(EnemyAI enemy)
    {
        enemy.gameObject.SetActive(false);
        enemy.transform.parent =  container;
    }

    private void OnGet(EnemyAI enemy)
    {
        enemy.transform.parent = null;
        enemy.gameObject.SetActive(true);
    }

    private EnemyAI Create()
    {
        var enemy = Instantiate(enemyPrefab, container); 
        enemy.gameObject.SetActive(false);
        
        return enemy;
    }
}