using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TMP_Text bossNameText;
    [SerializeField] private BossHealth bossHealth;

    public void Setup(BossHealth health)
    {
        bossHealth = health;
        bossHealth.OnFirstHit += ShowBar;
        bossHealth.OnHpChanged += UpdateBar;
        bossHealth.OnDeath += HideBar;

        panel.SetActive(false);
        hpSlider.maxValue = bossHealth.MaxHp;
        hpSlider.value = bossHealth.MaxHp;
        bossNameText.text = bossHealth.DisplayName;
    }

    private void ShowBar()
    {
        panel.SetActive(true);
    }

    private void UpdateBar(int current, int max)
    {
        hpSlider.value = current;
    }

    private void HideBar()
    {
        panel.SetActive(false);
    }

    private void OnDestroy()
    {
        if (bossHealth != null)
        {
            bossHealth.OnFirstHit -= ShowBar;
            bossHealth.OnHpChanged -= UpdateBar;
            bossHealth.OnDeath -= HideBar;
        }
    }
}