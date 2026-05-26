using System;
using UnityEngine;

public class BossHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private BossData data;

    private int currentHp;

    public int CurrentHp => currentHp;
    public int MaxHp => data != null ? data.maxHp : 0;
    public bool IsDead => currentHp <= 0;

    public event Action<int, int> OnHpChanged;
    public event Action OnDeath;
    public event Action OnDamaged;

    private void Awake()
    {
        currentHp = MaxHp;
    }

    public void TakeDamage(int amount)
    {
        if (IsDead) 
            return;

        currentHp = Mathf.Max(currentHp - amount, 0);
        OnHpChanged?.Invoke(currentHp, MaxHp);

        if (currentHp == 0)
            OnDeath?.Invoke();
        else
            OnDamaged?.Invoke();
    }
}