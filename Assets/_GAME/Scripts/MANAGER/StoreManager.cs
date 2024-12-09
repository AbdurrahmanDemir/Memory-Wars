//using CrazyGames;
using Playgama.Modules.Platform;
using Playgama;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Playgama.Modules.Advertisement;

public class StoreManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int startedPack1Count;
    [SerializeField] private int startedPack2Count;
    [SerializeField] private int goldCount;
    [SerializeField] private int energyCount;

    [Header("Elements")]
    [SerializeField] private GameObject startedPack1;
    [SerializeField] private TextMeshProUGUI startedPack1CountText;
    [SerializeField] private TextMeshProUGUI startedPack2CountText;
    [SerializeField] private TextMeshProUGUI energyCountText;
    [SerializeField] private TextMeshProUGUI goldText;
    private string currentAdContext;
    private void Awake()
    {
        Bridge.platform.SendMessage(PlatformMessage.GameReady);
        Bridge.storage.Set("level", "dungeon_123");


    }
    private void Start()
    {
        Bridge.advertisement.rewardedStateChanged += OnRewardedStateChanged;

        if (!PlayerPrefs.HasKey("startedpack1"))
            startedPack1.SetActive(true);
        else
        {
            startedPack1.SetActive(false);
        }

    }
    public void ShowAdForPack(string packContext)
    {
        currentAdContext = packContext; // Bağlamı belirle

        Bridge.advertisement.ShowRewarded();

    }
    private void OnRewardedStateChanged(RewardedState state)
    {
        if (state == RewardedState.Rewarded)
        {
            Debug.Log("Rewarded Ad Success for context: " + currentAdContext);
            // Reklam bağlamına göre ilgili metodu çalıştır
            switch (currentAdContext)
            {

                case "startedpack1":
                    StartedPack1();
                    break;
                case "startedpack2":
                    StartedPack2();
                    break;
                case "energypack":
                    EnergyPack();
                    break;
                case "bigenergypack":
                    BigEnergyPack();
                    break;
                case "goldpack":
                    GoldPack();
                    break;
                case "biggoldpack":
                    BigGoldPack();
                    break;
                case "gamemidpack":
                    GameMidAdPack();
                    break;
                case "battlemidpack":
                    BattleMidAdPack();
                    break;

                default:
                    Debug.LogWarning("Unknown context: " + currentAdContext);
                    break;
            }
        }
        else if (state == RewardedState.Failed)
        {
            Debug.Log("Rewarded Ad Failed for context: " + currentAdContext);
            // Başarısız olursa hiçbir şey yapma
        }

        // Bağlamı sıfırla
        currentAdContext = null;
    }
    public void NameAds(string name)
    {
        ShowAdForPack(name);
    }
    public void StartedPack1()
    {
        startedPack1Count++;
        startedPack1CountText.text = startedPack1Count + " AD WATCHED";
        if (startedPack1Count == 3)
        {
            DataManager.instance.AddGold(100);
            DataManager.instance.AddEnergy(20);
            PlayerPrefs.SetInt("startedpack1", 1);
        }

    }
    public void StartedPack2()
    {
        //CrazySDK.Ad.RequestAd(CrazyAdType.Rewarded, () =>
        //{
        //    /** ad started */
        //}, (error) =>
        //{
        //    /** ad error */
        //}, () =>
        //{
        //    DataManager.instance.AddGold(10);
        //    DataManager.instance.AddEnergy(5);
        //});
        DataManager.instance.AddGold(10);
        DataManager.instance.AddEnergy(5);
    }
    public void EnergyPack()
    {
        //CrazySDK.Ad.RequestAd(CrazyAdType.Rewarded, () =>
        //{
        //    /** ad started */
        //}, (error) =>
        //{
        //    /** ad error */
        //}, () =>
        //{
        //    energyCount++;
        //    energyCountText.text = energyCount + " AD WATCHED";
        //    if (energyCount == 2)
        //    {
        //        DataManager.instance.AddEnergy(3);
        //    }
        //});
        energyCount++;
        energyCountText.text = energyCount + " AD WATCHED";
        if (energyCount == 2)
        {
            DataManager.instance.AddEnergy(3);
        }

    }
    public void BigEnergyPack()
    {
        //CrazySDK.Ad.RequestAd(CrazyAdType.Rewarded, () =>
        //{
        //    /** ad started */
        //}, (error) =>
        //{
        //    /** ad error */
        //}, () =>
        //{
        //    energyCount++;
        //    energyCountText.text = energyCount + " AD WATCHED";
        //    if (energyCount == 2)
        //    {
        //        DataManager.instance.AddEnergy(10);

        //    }
        //});
        energyCount++;
        energyCountText.text = energyCount + " AD WATCHED";
        if (energyCount == 2)
        {
            DataManager.instance.AddEnergy(10);

        }
    }



    public void GoldPack()
    {
        //CrazySDK.Ad.RequestAd(CrazyAdType.Rewarded, () =>
        //{
        //    /** ad started */
        //}, (error) =>
        //{
        //    /** ad error */
        //}, () =>
        //{
        //    DataManager.instance.AddGold(15);

        //});

        DataManager.instance.AddGold(15);

    }
    public void BigGoldPack()
    {
        //CrazySDK.Ad.RequestAd(CrazyAdType.Rewarded, () =>
        //{
        //    /** ad started */
        //}, (error) =>
        //{
        //    /** ad error */
        //}, () =>
        //{
        //    goldCount++;
        //    goldText.text = goldCount + " AD WATCHED";
        //    if (goldCount == 2)
        //    {
        //        DataManager.instance.AddGold(40);
        //    }
        //});

        DataManager.instance.AddGold(40);
    }

    public void GameMidAdPack()
    {
        //CrazySDK.Ad.RequestAd(CrazyAdType.Rewarded, () =>
        //{
        //    /** ad started */
        //}, (error) =>
        //{
        //    /** ad error */
        //}, () =>
        //{            
        //        DataManager.instance.AddGold(5);
        //        DataManager.instance.AddEnergy(2);

        //});

        DataManager.instance.AddGold(5);
        DataManager.instance.AddEnergy(2);
    }
    public void BattleMidAdPack()
    {
        //CrazySDK.Ad.RequestAd(CrazyAdType.Rewarded, () =>
        //{
        //    /** ad started */
        //}, (error) =>
        //{
        //    /** ad error */
        //}, () =>
        //{
        //    DataManager.instance.AddGold(10);

        //});

        DataManager.instance.AddGold(10);
    }

}
