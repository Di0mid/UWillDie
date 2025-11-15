using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptable Objects/EnemySO")]
public class EnemySO : ScriptableObject
{
    public EnemyAI enemyPrefab;
    [Range(0.25f, 10f)] public float threatLevel = 0.25f;
}