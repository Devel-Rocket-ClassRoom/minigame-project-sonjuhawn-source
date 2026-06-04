using System;
using UnityEngine;

public class BossHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private BossData data;
    [SerializeField] private GameObject damagePopupPrefab;
    [SerializeField] private Transform damageCanvas;

    private int currentHp;

    public int CurrentHp => currentHp;
    public int MaxHp => data != null ? data.maxHp : 0;
    public bool IsDead => currentHp <= 0;
    public string DisplayName => data.displayName;

    public event Action<int, int> OnHpChanged;
    public event Action OnDeath;
    public event Action OnDamaged;

    public event Action OnFirstHit;
    private bool firstHitFired = false;

    private void Awake()
    {
        currentHp = MaxHp;
        damageCanvas = GameObject.Find("PopUpCanvas").transform;
    }

    public void TakeDamage(int amount)
    {
        if (!firstHitFired)
        {
            firstHitFired = true;
            OnFirstHit?.Invoke();
        }

        if (IsDead) 
            return;

        currentHp = Mathf.Max(currentHp - amount, 0);
        OnHpChanged?.Invoke(currentHp, MaxHp);

        if (currentHp == 0)
            OnDeath?.Invoke();
        else
            OnDamaged?.Invoke();

        var canvas = FindAnyObjectByType<Canvas>();
        var popup = Instantiate(damagePopupPrefab, damageCanvas);
        popup.GetComponent<DamagePopup>().Init(amount, transform.position); ;
    }
}