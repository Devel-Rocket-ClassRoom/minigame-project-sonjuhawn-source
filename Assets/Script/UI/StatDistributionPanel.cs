using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatDistributionPanel : MonoBehaviour
{
    // === 시스템 참조 ===
    [SerializeField] private GameObject panelRoot;   // 추가
    [SerializeField] private ExperienceSystem exp;
    [SerializeField] private PlayerStats stats;

    // === UI 참조 ===
    [SerializeField] private TMP_Text pointsLabel;
    [SerializeField] private TMP_Text strengthValue;
    [SerializeField] private TMP_Text agilityValue;
    [SerializeField] private TMP_Text vitalityValue;
    [SerializeField] private TMP_Text staminaValue;
    [SerializeField] private Button confirmButton;


    // === 이벤트 구독/해제 ===
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
        Debug.Log($"LevelUp: {Time.realtimeSinceStartup}");
        panelRoot.SetActive(true);
        PauseManager.Instance.Pause();
        Cursor.visible = true;                      
        Cursor.lockState = CursorLockMode.None;      
        Refresh();
    }

    public void OnPlus(int statTypeInt)
    {
        if(exp.SpendPoint((StatType)statTypeInt))
            Refresh();
    }

    public void OnConfirm()
    {
        PauseManager.Instance.Resume();
        Cursor.visible = false;                     
        Cursor.lockState = CursorLockMode.Locked;
        panelRoot.SetActive(false);
    }

    private void Refresh()
    {
        pointsLabel.text = $"Point: {exp.PendingPoints}";
        strengthValue.text = $"{stats.Strength}";
        agilityValue.text = $"{stats.Agility}";
        vitalityValue.text = $"{stats.Vitality}";
        staminaValue.text = $"{stats.Stamina}";

        if (exp.PendingPoints == 0)
            confirmButton.interactable = true;
        else
            confirmButton.interactable = false;

    }
}