using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatDistributionPanel : MonoBehaviour
{
    [SerializeField] private ExperienceSystem exp;
    [SerializeField] private PlayerStats stats;

    [SerializeField] private TMP_Text pointsLabel;
    [SerializeField] private TMP_Text strengthValue;
    [SerializeField] private TMP_Text agilityValue;
    [SerializeField] private TMP_Text vitalityValue;
    [SerializeField] private TMP_Text staminaValue;
    [SerializeField] private Button confirmButton;

    private void Awake() => gameObject.SetActive(false);

    private void OnEnable()
    {
        exp.OnLevelUp += HandleLevelUp;
        stats.OnStatChanged += Refresh;
    }
    private void OnDisable()
    {
        exp.OnLevelUp -= HandleLevelUp;
        stats.OnStatChanged -= Refresh;
    }

    private void HandleLevelUp(int newLevel)
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
        Refresh();
    }

    // 각 + 버튼이 호출 (Inspector에서 OnClick 연결)
    public void OnPlus(int statTypeInt)
    {
        exp.SpendPoint((StatType)statTypeInt);
    }

    public void OnConfirm()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    private void Refresh()
    {
        pointsLabel.text = $"남은 포인트: {exp.PendingPoints}";
        strengthValue.text = stats.Strength.ToString();
        agilityValue.text = stats.Agility.ToString();
        vitalityValue.text = stats.Vitality.ToString();
        staminaValue.text = stats.Stamina.ToString();
        confirmButton.interactable = (exp.PendingPoints == 0);
    }
}