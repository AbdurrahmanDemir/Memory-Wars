using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Card")]
public class CardSO : ScriptableObject
{
    public string cardName;
    public Sprite cardImage;
    public int damage;

    public CardEffect cardEffect;
    public int effectValue;

    [TextArea] public string cardEffectDescription;

   
    public int baseUpgradeCost=10; // Varsayýlan yükseltme maliyeti


    public int GetCurrentDamage()
    {
        return PlayerPrefs.GetInt($"{cardName}_Damage", damage);
    }

    public int GetUpgradeCost()
    {
        return PlayerPrefs.GetInt($"{cardName}_UpgradeCost", baseUpgradeCost);
    }


    public void UpgradeDamage()
    {
        int currentDamage = GetCurrentDamage();
        int upgradeCost = GetUpgradeCost();

        // Yeni deðerleri hesapla
        int newDamage = currentDamage + 1;
        int newUpgradeCost = upgradeCost * 2;

        // PlayerPrefs'e kaydet
        PlayerPrefs.SetInt($"{cardName}_Damage", newDamage);
        PlayerPrefs.SetInt($"{cardName}_UpgradeCost", newUpgradeCost);
        PlayerPrefs.Save();
    }

    //public void UpgradeEffect()
    //{
    //    upgradeLevel++;
    //    effectValue += 1; // Efekt artýrýmý
    //}
}
public enum CardEffect
{
    None=0, 
    Trap=5,
    Damage=1,          
    ExtraMoves=2,
    Health=3,
    Coin=4
}
