using TMPro;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private ShopSystem shop;
    [SerializeField] private WaveManager waveManager;

    [SerializeField] private TMP_Text goldText;
    [SerializeField] private GoldSystem gold;

    [SerializeField] private TMP_Text potionCostText;
    [SerializeField] private TMP_Text healCostText;
    [SerializeField] private TMP_Text statCostText;

    private void Start()
    {
        potionCostText.text = $"Potion +1 ({shop.PotionUpgradeCost}G)";
        healCostText.text = $"HP Regenerate ({shop.HealCost}G)";
        statCostText.text = $"Stat Up ({shop.StatUpgradeCost}G)";
    }

    private void OnEnable()
    {
        gold.OnGoldChanged += UpdateGoldText;
    }

    private void OnDisable()
    {
        gold.OnGoldChanged -= UpdateGoldText;
    }

    public void Open()
    {
        panel.SetActive(true);
        shop.OpenShop();
        UpdateGoldText(gold.CurrentGold);
    }

    public void Close()
    {
        panel.SetActive(false);
        shop.CloseShop();
        waveManager.EndRest();
    }

    private void UpdateGoldText(int amount)
    {
        goldText.text = $"Gold: {amount}";
    }

    public void OnBuyPotion() => shop.BuyPotionUpgrade();
    public void OnBuyHeal() => shop.BuyHeal();
    public void OnBuyStatUpgrade() => shop.BuyStatUpgrade();
}