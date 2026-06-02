using System;
using UnityEngine;

public class PotionSystem : MonoBehaviour
{
    [SerializeField] private int maxPotions = 2;
    [SerializeField] private float healPercent = 0.3f;
    [SerializeField] private float cooldown = 3f;
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private ParticleSystem healParticle;

    private int currentPotions;
    private float nextUseTime;
    private HealthSystem health;
    private PlayerInputHandler input;

    public int CurrentPotions => currentPotions;
    public int MaxPotions => maxPotions;
    public float CooldownProgress =>
    nextUseTime <= Time.time ? 1f : 1f - (nextUseTime - Time.time) / cooldown;

    public event Action<int, int> OnPotionChanged; // (current, max)

    private void Awake()
    {
        health = GetComponent<HealthSystem>();
        input = GetComponent<PlayerInputHandler>();
        currentPotions = maxPotions;
        OnPotionChanged?.Invoke(currentPotions, maxPotions);
    }

    private void OnEnable()
    {
        input.OnPotion += UsePotion;
        waveManager.OnWaveCleared += HandleWaveCleared;
    }

    private void OnDisable()
    {
        input.OnPotion -= UsePotion;
        waveManager.OnWaveCleared -= HandleWaveCleared;
    }

    public void UsePotion()
    {
        if (currentPotions == 0 || Time.time < nextUseTime || health.CurrentHp == health.MaxHp)
            return;
        currentPotions--;
        health.Heal((int)(health.MaxHp * healPercent));
        if (healParticle != null) healParticle.Play();
        nextUseTime = Time.time + cooldown;
        OnPotionChanged?.Invoke(currentPotions, maxPotions);
    }

    public void RestoreAll()
    {
        currentPotions = maxPotions;
        OnPotionChanged?.Invoke(currentPotions, maxPotions);
    }

    public void AddMaxPotion()
    {
        maxPotions++;
        currentPotions++;
        OnPotionChanged?.Invoke(currentPotions, maxPotions);
    }
    private void HandleWaveCleared(int waveIndex)
    {
        RestoreAll();
    }
}