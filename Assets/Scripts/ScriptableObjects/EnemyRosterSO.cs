using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyRoosterSO", menuName = "Scriptable Objects/EnemyRoosterSO")]
public class EnemyRosterSO : ScriptableObject
{
    public List<EnemySO> enemySOList;
}