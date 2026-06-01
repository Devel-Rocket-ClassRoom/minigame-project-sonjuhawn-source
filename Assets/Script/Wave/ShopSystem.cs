using System;
using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    [SerializeField] private GoldSystem gold;
    [SerializeField] private HealthSystem playerHealth;
    [SerializeField] private PotionSystem potion;
    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private int potionUpgradeCost = 100;
    [SerializeField] private int healCost = 80;
    [SerializeField] private int statUpgradeCost = 50;

    public int PotionUpgradeCost => potionUpgradeCost;
    public int HealCost => healCost;
    public int StatUpgradeCost => statUpgradeCost;


    public void OpenShop()
    {
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void CloseShop()
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void BuyPotionUpgrade()
    {
        if (gold.SpendGold(potionUpgradeCost))
        {
            potion.AddMaxPotion();
        }
    }

    public void BuyHeal()
    {
        if (gold.SpendGold(healCost))
        {
            playerHealth.Heal(playerHealth.MaxHp);
        }
    }

    public void BuyStatUpgrade()
    {
        if (gold.SpendGold(statUpgradeCost))
        {
            StatType picked = (StatType)UnityEngine.Random.Range(0, 4); // 0~3 → enum 값으로 캐스팅
            playerStats.Grow(picked, 1);
            ToastManager.Instance.Show($"{picked} +1");
        }
    }
}