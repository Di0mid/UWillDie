using System;

public static class GameEvents
{
    public static event Action OnEnemyDied;



    public static void ReportEnemyDied()
    {
        OnEnemyDied?.Invoke();
    }
}