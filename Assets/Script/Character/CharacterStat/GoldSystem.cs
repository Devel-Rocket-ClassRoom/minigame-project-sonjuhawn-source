using System;
using UnityEngine;

public class GoldSystem : MonoBehaviour
{
    [SerializeField] private int currentGold;

    public int CurrentGold => currentGold;
    public int TotalSpent { get; private set; }


    public event Action<int> OnGoldChanged;

    public void AddGold(int amount)
    {
        if (amount <= 0) return;
        currentGold += amount;
        OnGoldChanged?.Invoke(currentGold);
    }

    public bool SpendGold(int amount)
    {
        if (currentGold < amount) return false;
        currentGold -= amount;
        OnGoldChanged?.Invoke(currentGold);
        TotalSpent += amount;
        return true;
    }
}