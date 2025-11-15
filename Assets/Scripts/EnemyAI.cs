using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float rotationSpeed = 10f;

    [Space] 
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1.5f;

    [Space]
    [SerializeField] private float aiUpdateRate = 0.1f;



    private float _lastAttackTime = float.MinValue;
    
    private NavMeshAgent _navMeshAgent;
    private Health _health;
    
    private bool _hasTarget;
    private Transform _targetTransform;
    private IDamageable _targetDamageable;

    private IObjectPool<EnemyAI> _myPool;

    private enum State
    {
        Idle,
        Chasing,
        Attacking,
    }
    private State _currentState;
    

    
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _health = GetComponent<Health>();

        _navMeshAgent.speed = moveSpeed;
        _navMeshAgent.angularSpeed = rotationSpeed;
    }

    private void Update()
    {
        if (_currentState == State.Attacking)
        {
            LookAtTarget();
        }
    }

    private void OnEnable()
    {
        _health.OnDeath += ReturnToPool;
    }

    private void OnDisable()
    {
        _health.OnDeath -= ReturnToPool;
    }
    
    

    public void Init(IObjectPool<EnemyAI> pool, Transform playerTransform)
    {
        _myPool = pool;
        _targetTransform = playerTransform;
        _targetDamageable = _targetTransform.GetComponent<IDamageable>();
        
        _health.ResetHealth();
        _hasTarget = true;
        _navMeshAgent.isStopped = false;
        _currentState = State.Chasing;
        
        if (_targetTransform.TryGetComponent(out Health targetHealth))
        {
            targetHealth.OnDeath -= Target_OnDeath;
            targetHealth.OnDeath += Target_OnDeath;
        }
        
        StartCoroutine(AILogic());
    }

    private void Target_OnDeath()
    {
        _navMeshAgent.isStopped = true;
        _hasTarget = false;
        
        _currentState = State.Idle;
    }



    private void ReturnToPool()
    {
        StopAllCoroutines();
        
        if (_targetTransform && _targetTransform.TryGetComponent(out Health targetHealth))
        {
            targetHealth.OnDeath -= Target_OnDeath;
        }   
        
        GameEvents.ReportEnemyDied();
        
        _myPool?.Release(this);
    }
    
    private IEnumerator AILogic()
    {
        var waitToUpdate = new WaitForSeconds(aiUpdateRate);
        
        while (true)
        {
            if (!_hasTarget)
            {
                yield return null;
                continue;
            }
            
            float sqrDistance = (_targetTransform.position - transform.position).sqrMagnitude;

            _currentState = sqrDistance <= attackRange * attackRange ? State.Attacking : State.Chasing;
            
            switch (_currentState)
            {
                default:
                case State.Idle:
                    
                    
                    
                    break;
                case State.Chasing:
                    
                    DoChase();
                    
                    break;
                case State.Attacking:
                    
                    DoAttack();
                    
                    break;
            }
            
            yield return waitToUpdate;
        }
    }

    
    
    private void DoChase()
    {
        _navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(_targetTransform.position);
    }

    private void DoAttack()
    {
        _navMeshAgent.isStopped = true;

        if (!(Time.time > _lastAttackTime + attackCooldown))
        {
            return;
        }
        
        _lastAttackTime = Time.time;
            
        Debug.Log($"{name} attacking!");
            
        _targetDamageable?.TakeDamage(attackDamage);
    }

    
    
    private void LookAtTarget()
    {
        var direction = _targetTransform.position - transform.position;
        direction.y = 0;
        var rotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
}