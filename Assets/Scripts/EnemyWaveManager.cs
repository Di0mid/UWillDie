using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    private EnemyPool _pool;


    
    private void Awake()
    {
        _pool = GetComponent<EnemyPool>();
    }
    
    
    
    
}