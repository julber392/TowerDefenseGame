using System;

public static class GameEvents
{
    // событие: враг умер и дал XP
    public static Action<int> OnEnemyKilled;
    
}