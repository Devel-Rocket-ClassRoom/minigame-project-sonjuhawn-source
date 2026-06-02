using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour, IDamageable
{
    [SerializeField] private int baseHp = 100;
    [SerializeField] private int hpPerVitality = 10;  // 체력 1당 HP 변환 비율

    private IStatProvider stats;
    private CharacterStateMachine state;
    private int currentHp;
    private int maxHp;

    public int CurrentHp => currentHp;
    public int MaxHp => maxHp;

    public bool IsDead => currentHp <= 0;

    public event Action<int, int> OnHpChanged;  // (current, max) — UI 갱신용
    public event Action OnDeath;
    public event Action OnDamaged;

    private void Awake()
    {
        stats = GetComponent<IStatProvider>();
        state = GetComponent<CharacterStateMachine>();
    }


    private void OnEnable()
    {
        stats.OnStatChanged += RecalculateMaxHp;
        RecalculateMaxHp();
        currentHp = maxHp;
        OnHpChanged?.Invoke(currentHp, maxHp);
    }

    private void OnDisable()
    {
        stats.OnStatChanged -= RecalculateMaxHp;
    }

    private void RecalculateMaxHp()
    {
        int bonus = Mathf.Max(0, stats.Vitality - 10) * hpPerVitality;
        maxHp = baseHp + bonus;
        currentHp = Mathf.Min(currentHp, maxHp);
        OnHpChanged?.Invoke(currentHp, maxHp);
    }

    public void TakeDamage(int amount)
    {
        if (state != null && state.IsInvincible)
            return;

        currentHp = Mathf.Max(currentHp - amount, 0);
        OnHpChanged?.Invoke(currentHp, maxHp);
        OnDamaged?.Invoke();

        if (currentHp == 0)
            OnDeath?.Invoke();
    }

    public void Heal(int amount)
    {
        if(IsDead)
            return;
        currentHp = Mathf.Min(currentHp + amount, maxHp);
        OnHpChanged?.Invoke(currentHp, maxHp);
    }
}