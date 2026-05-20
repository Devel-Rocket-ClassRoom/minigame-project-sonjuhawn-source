using System;
using UnityEngine;

public class StaminaSystem : MonoBehaviour
{
    [SerializeField] private int staminaPerStaminaStat = 10;

    private IStatProvider stats;
    private int currentStamina;
    private int maxStamina;

    public int CurrentStamina => currentStamina;
    public int MaxStamina => maxStamina;

    public event Action<int, int> OnStaminaChanged;

    private void Awake()
    {
        stats = GetComponent<IStatProvider>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            bool ok = TryConsume(20);
            Debug.Log($"TryConsume(20): {ok}, Stamina: {currentStamina}/{maxStamina}");
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            Recover(10);
            Debug.Log($"Recover(10), Stamina: {currentStamina}/{maxStamina}");
        }
    }
    private void OnEnable()
    {
        stats.OnStatChanged += RecalculateMaxStamina;
        RecalculateMaxStamina();
        currentStamina = maxStamina;
        OnStaminaChanged?.Invoke(currentStamina, maxStamina);
    }

    private void OnDisable()
    {
        stats.OnStatChanged -= RecalculateMaxStamina;
    }

    private void RecalculateMaxStamina()
    {
        maxStamina = stats.Stamina * staminaPerStaminaStat;
        currentStamina = Mathf.Min(currentStamina, maxStamina);
        OnStaminaChanged?.Invoke(currentStamina, maxStamina);
    }

    // 스테미나 소모 시도 — 충분하면 소모 후 true, 부족하면 false
    public bool TryConsume(int amount)
    {
        if (currentStamina < amount)
            return false;

        currentStamina -= amount;
        OnStaminaChanged?.Invoke(currentStamina, maxStamina);
        return true;
    }

    // 스테미나 회복 (기본공격 시 외부에서 호출)
    public void Recover(int amount)
    {
        currentStamina = Mathf.Min(currentStamina + amount, maxStamina);
        OnStaminaChanged?.Invoke(currentStamina, maxStamina);
    }
}