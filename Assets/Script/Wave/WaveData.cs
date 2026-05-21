using UnityEngine;

[CreateAssetMenu(fileName = "NewWaveData", menuName = "Game/Wave Data")]
public class WaveData : ScriptableObject
{
    [System.Serializable]
    public struct SpawnEntry
    {
        public MonsterData monster;
        [Min(1)] public int count;
    }
    public SpawnEntry[] spawnEntries;


    [Header("Timing")]
    [Min(0)] public float waveDelay = 2f;
    [Min(0)] public float spawnDuration = 1f;

}