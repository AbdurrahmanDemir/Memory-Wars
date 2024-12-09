using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewHero", menuName = "Hero")]

public class HeroSO : ScriptableObject
{
    public string heroName;
    public Sprite heroIcon;
    public int health;
    public int numberOfMovesPerTurn;
    public int energySpentPerMatch;
    public int baseUpgradeCost = 10;
    public int GetCurrentHealth()
    {
        return PlayerPrefs.GetInt($"{heroName}_Health", health);
    }

    public int GetUpgradeCost()
    {
        return PlayerPrefs.GetInt($"{heroName}_UpgradeCostHero", baseUpgradeCost);
    }


    public void UpgradeDamage()
    {
        int currentDamage = GetCurrentHealth();
        int upgradeCost = GetUpgradeCost();

        // Yeni deðerleri hesapla
        int newDamage = currentDamage + 1;
        int newUpgradeCost = upgradeCost * 2;

        // PlayerPrefs'e kaydet
        PlayerPrefs.SetInt($"{heroName}_Health", newDamage);
        PlayerPrefs.SetInt($"{heroName}_UpgradeCostHero", newUpgradeCost);
        PlayerPrefs.Save();
    }
}
