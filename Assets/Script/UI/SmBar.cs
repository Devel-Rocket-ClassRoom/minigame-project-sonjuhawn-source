using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] private StaminaSystem stamina;
    [SerializeField] private Slider slider;

    private void OnEnable()
    {
        stamina.OnStaminaChanged += UpdateBar;
        UpdateBar(stamina.CurrentStamina, stamina.MaxStamina);
    }

    private void OnDisable()
    {
        stamina.OnStaminaChanged -= UpdateBar;
    }

    private void UpdateBar(int current, int max)
    {
        if (max <= 0) return;
        slider.value = (float)current / max;
    }
}