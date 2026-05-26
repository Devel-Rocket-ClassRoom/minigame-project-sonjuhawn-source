using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
    [SerializeField] private ExperienceSystem exp;
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text levelLabel;

    private void OnEnable()
    {
        exp.OnExpChanged += UpdateBar;
        exp.OnLevelUp += UpdateLevel;
        UpdateBar(exp.CurrentExp, exp.ExpToNext);
        UpdateLevel(exp.CurrentLevel);
    }

    private void OnDisable()
    {
        exp.OnExpChanged -= UpdateBar;
        exp.OnLevelUp -= UpdateLevel;
    }

    private void UpdateBar(int current, int max)
    {
        if (max <= 0) return;
        slider.value = (float)current / max;
    }

    private void UpdateLevel(int level)
    {
        levelLabel.text = $"Lv. {level}";
    }
}