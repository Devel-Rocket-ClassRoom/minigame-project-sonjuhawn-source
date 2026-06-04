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

    [SerializeField] private AudioClip UsingGoldCilp;

    public int PotionUpgradeCost => potionUpgradeCost;
    public int HealCost => healCost;
    public int StatUpgradeCost => statUpgradeCost;


    public void OpenShop()
    {
        PauseManager.Instance.Pause();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void CloseShop()
    {
        PauseManager.Instance.Resume();
        if (PauseManager.Instance.PauseCount == 0)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void BuyPotionUpgrade()
    {
        if (gold.SpendGold(potionUpgradeCost))
        {
            AudioManager.Instance.PlaySFX(UsingGoldCilp);
            potion.AddMaxPotion();
        }
    }

    public void BuyHeal()
    {
        if (gold.SpendGold(healCost))
        {
            AudioManager.Instance.PlaySFX(UsingGoldCilp);
            playerHealth.Heal(playerHealth.MaxHp);
        }
    }

    public void BuyStatUpgrade()
    {
        if (gold.SpendGold(statUpgradeCost))
        {
            AudioManager.Instance.PlaySFX(UsingGoldCilp);
            StatType picked = (StatType)UnityEngine.Random.Range(0, 4);
            playerStats.Grow(picked, 1);
            ToastManager.Instance.Show($"{picked} +1");
        }
    }
}