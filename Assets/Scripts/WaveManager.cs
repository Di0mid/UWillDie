using System.Collections;
using System.Linq;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private EnemyRosterSO enemyRosterSO;

    [Space] 
    [SerializeField] private float defaultWaveBudget = 1f;
    [SerializeField] private float enemySpawnInterval = 1f;
    [SerializeField] private float waveInterval = 120f;
    
    
    
    private int _remainingEnemies;
    private int _currentWaveNumber;
    private float _currentWaveBudget;
    
    private EnemyPool _pool;


    
    private void Awake()
    {
        _currentWaveBudget = defaultWaveBudget;
        _pool = GetComponent<EnemyPool>();
    }

    private void Start()
    {
        StartCoroutine(WaveLoop());
    }

    private void OnEnable()
    {
        GameEvents.OnEnemyDied += GameEvents_OnEnemyDied;   
    }

    private void OnDisable()
    {
        GameEvents.OnEnemyDied -= GameEvents_OnEnemyDied;   
    }



    private IEnumerator WaveLoop()
    {
        while (true)
        {
            Debug.Log($"New wave is coming...");
            yield return new WaitForSeconds(waveInterval);
            
            _currentWaveNumber++;

            Debug.Log($"Wave {_currentWaveNumber} started!");

            var budget = _currentWaveBudget;
            while (budget > 0)
            {
                var availableEnemies = enemyRosterSO.enemySOList.Where(e => e.threatLevel <= budget).ToList();

                if (availableEnemies.Count == 0)
                {
                    break;
                }
                
                var enemyToSpawn = availableEnemies[Random.Range(0, availableEnemies.Count)];
                budget -= enemyToSpawn.threatLevel;

                _pool.Get(Vector3.zero);

                _remainingEnemies++;
                
                yield return new WaitForSeconds(enemySpawnInterval);
            }

            Debug.Log($"All enemies on Wave {_currentWaveNumber} was spawned ({_remainingEnemies})!");
            
            yield return new WaitUntil(() => _remainingEnemies <= 0);
            
            Debug.Log($"Wave {_currentWaveNumber} ended!");

            _currentWaveBudget++;
        }
    }
    
    private void GameEvents_OnEnemyDied()
    {
        _remainingEnemies--;
    }
}