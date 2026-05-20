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

    private void Awake()
    {
        if (data != null) currentHp = data.maxHp;
    }

    private void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.M))
        {
            TakeDamage(10);
            Debug.Log($"Monster HP: {currentHp}/{MaxHp}, Dead: {IsDead}");
        }
    }


    public void TakeDamage(int amount)
    {
        if (IsDead) return;

        currentHp = Mathf.Max(currentHp - amount, 0);
        OnHpChanged?.Invoke(currentHp, MaxHp);

        if (currentHp == 0)
            OnDeath?.Invoke();
    }
}