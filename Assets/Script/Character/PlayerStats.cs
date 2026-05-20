using System;
using UnityEngine;

public enum StatType
{
    Strength,
    Agility,
    Vitality,
    Stamina
}

public class PlayerStats : MonoBehaviour, IStatProvider
{
    [SerializeField] private int strength = 10;
    [SerializeField] private int agility = 10;
    [SerializeField] private int vitality = 10;
    [SerializeField] private int stamina = 10;

    public int Strength => strength;
    public int Agility => agility;
    public int Vitality => vitality;
    public int Stamina => stamina;

    public event Action OnStatChanged;

    // === 개별 스탯 강화 (2주차 정식 UI에서 사용) ===
    public void Grow(StatType type, int amount = 1)
    {
        switch (type)
        {
            case StatType.Strength: strength += amount; break;
            case StatType.Agility: agility += amount; break;
            case StatType.Vitality: vitality += amount; break;
            case StatType.Stamina: stamina += amount; break;
        }
        OnStatChanged?.Invoke();
    }

    // === 모든 스탯 동시 강화 (1주차 MVP 자동 분배) ===
    public void GrowAll(int amount = 1)
    {
        strength += amount;
        agility += amount;
        vitality += amount;
        stamina += amount;
        OnStatChanged?.Invoke();
    }
}