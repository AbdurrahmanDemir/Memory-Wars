using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    [Header(" Elements ")]
    [SerializeField] private TextMeshProUGUI menuGoldText;
    [SerializeField] private TextMeshProUGUI battleGoldText;
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private Slider sliderXP;

    [Header(" Data ")]
    [SerializeField] private int gold;
    [SerializeField] private int xp;
    [SerializeField] private int energy;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        LoadData();
    }

    public bool TryPurchaseGold(int price)
    {
        if (price <= gold)
        {
            gold -= price;
            SaveData();
            UpdateGoldText();
            return true;
        }
        else
        {
            UIManager.instance.OpenPopUp("Not enough Gold");
        }
        return false;
    }
    public void AddGold(int value)
    {
        gold += value;

        UpdateGoldText();

        SaveData();
    }
    public void AddXP(int value)
    {
        xp += value;

        UpdateXPText();

        SaveData();
    }
    private void UpdateGoldText()
    {
        menuGoldText.text = gold.ToString();
        battleGoldText.text = gold.ToString();
    }
    private void UpdateXPText()
    {
        xpText.text = xp.ToString();
    }
    private void SaveData()
    {
        PlayerPrefs.SetInt("Gold", gold);
        PlayerPrefs.SetInt("XP", xp);
        PlayerPrefs.SetInt("Energy", energy);
    }
    private void LoadData()
    {
        if (PlayerPrefs.HasKey("Gold"))
        {
            gold= PlayerPrefs.GetInt("Gold");
        }
        else
        {
            AddGold(100);
            AddEnergy(5);
        }
        xp = PlayerPrefs.GetInt("XP",0);
        sliderXP.maxValue = 500;
        sliderXP.value = xp;

        energy = PlayerPrefs.GetInt("Energy", energy);

        SaveData();
        UpdateGoldText();
        UpdateXPText();
        UpdateEnergyText();
    }

    //energy

    public bool TryPurchaseEnergy(int price)
    {
        if (price <= energy)
        {
            energy -= price;
            SaveData();
            UpdateEnergyText();
            return true;
        }
        else
        {
            UIManager.instance.OpenPopUp("Not enough Energy");
        }
        return false;
    }
    private void UpdateEnergyText()
    {
        energyText.text = energy.ToString();
    }
    public void AddEnergy(int value)
    {
        energy += value;

        UpdateEnergyText();

        SaveData();
    }

}

