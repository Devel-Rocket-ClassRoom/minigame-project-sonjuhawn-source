using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int hpPerVitality = 10;  // 체력 1당 HP 변환 비율

    private IStatProvider stats;
    private CharacterStateMachine state;
    private int currentHp;
    private int maxHp;

    public int CurrentHp => currentHp;
    public int MaxHp => maxHp;

    public event Action<int, int> OnHpChanged;  // (current, max) — UI 갱신용
    public event Action OnDeath;

    private void Awake()
    {
        stats = GetComponent<IStatProvider>();
        state = GetComponent<CharacterStateMachine>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            TakeDamage(10);
            Debug.Log($"HP: {CurrentHp}/{MaxHp}");
        }
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
        maxHp = stats.Vitality * hpPerVitality;
        currentHp = Mathf.Min(currentHp, maxHp);
        OnHpChanged?.Invoke(currentHp, maxHp);
    }

    public void TakeDamage(int amount)
    {
        if (state != null && state.IsInvincible)
            return;

        currentHp = Mathf.Max(currentHp - amount, 0);
        OnHpChanged?.Invoke(currentHp, maxHp);

        if (currentHp == 0)
            OnDeath?.Invoke();
    }
}