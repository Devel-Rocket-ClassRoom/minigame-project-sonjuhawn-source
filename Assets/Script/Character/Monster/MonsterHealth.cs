using System;
using UnityEngine;

public class MonsterHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private MonsterData data;

    private int currentHp;

    public int CurrentHp => currentHp;
    public int MaxHp => data != null ? data.maxHp : 0;
    public bool IsDead => currentHp <= 0;

    public event Action<int, int> OnHpChanged;
    public event Action OnDeath;
    public event Action OnDamaged;

    private void Awake()
    {
        if (data != null) currentHp = data.maxHp;
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

    public void Initialize(MonsterData d)
    {
        data = d;
        currentHp = d.maxHp;
        OnHpChanged?.Invoke(currentHp, MaxHp);
    }
}