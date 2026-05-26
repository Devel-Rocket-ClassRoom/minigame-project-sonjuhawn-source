using TMPro;
using UnityEngine;

public class GoldHud : MonoBehaviour
{
    [SerializeField] private GoldSystem gold;
    [SerializeField] private TMP_Text goldLabel;

    private void OnEnable()
    {
        gold.OnGoldChanged += Refresh;
        Refresh(gold.CurrentGold);
    }

    private void OnDisable()
    {
        gold.OnGoldChanged -= Refresh;
    }

    private void Refresh(int amount)
    {
        goldLabel.text = $"Gold: {amount}";
    }
}