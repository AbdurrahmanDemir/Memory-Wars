using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroCard : MonoBehaviour
{
    [Header("Elements")]
    public Image heroIcon;
    public TextMeshProUGUI heroNameText;
    public TextMeshProUGUI heroHealthText;
    public TextMeshProUGUI heroMoveText;
    public TextMeshProUGUI heroEnergyText;
    [SerializeField] private TextMeshProUGUI cardUpgradeCostText;
    public Slider heroHealthSlider;
    public Slider heroMoveSlider;
    public Slider heroEnergySlider;

    public Button healthUpgradeButton;

    private HeroSO heroData;

    public void Config(string name, int health, int move, int energy, Sprite icon, HeroSO hero)
    {
        heroData = hero;

        heroIcon.sprite = icon;
        heroNameText.text = name;
        heroHealthText.text = heroData.GetCurrentHealth().ToString();
        heroMoveText.text = move.ToString();
        heroEnergyText.text = energy.ToString();
        cardUpgradeCostText.text= heroData.GetUpgradeCost().ToString();

        heroMoveSlider.value = move;
        heroHealthSlider.value = health;
        heroEnergySlider.value = energy;
    }
    public void UpdateUI()
    {
        heroHealthSlider.value = heroData.GetCurrentHealth();
        heroHealthText.text = heroData.GetCurrentHealth().ToString();
        cardUpgradeCostText.text = heroData.GetUpgradeCost().ToString();
    }

}
