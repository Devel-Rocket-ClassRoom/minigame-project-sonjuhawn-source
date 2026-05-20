using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] private HealthSystem health;
    [SerializeField] private Slider slider;

    private void OnEnable()
    {
        health.OnHpChanged += UpdateBar;
        UpdateBar(health.CurrentHp, health.MaxHp);
    }

    private void OnDisable()
    {
        health.OnHpChanged -= UpdateBar;
    }

    private void UpdateBar(int current, int max)
    {
        if (max <= 0) return;
        slider.value = (float)current / max;
    }
}