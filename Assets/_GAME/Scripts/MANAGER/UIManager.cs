using DG.Tweening;
using LayerLab;
using Playgama;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    Menu,
    Game,
    Finish,
    GameLose
}
public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    GameState state;

    [Header("Panels")]
    [SerializeField] private GameObject MenuPanel;
    [SerializeField] private GameObject GamePanel;
    [SerializeField] private GameObject LevelsPanel;
    [SerializeField] private GameObject GameWinPanel;
    [SerializeField] private GameObject GameLosePanel;
    [Header("Elements")]
    [SerializeField] private GameObject WinPanel;
    [SerializeField] private GameObject LosePanel;
    [SerializeField] private GameObject IncompleteLevelPanel;
    [SerializeField] private GameObject QuestPanel;
    [SerializeField] private GameObject ShopPanel;
    [SerializeField] private GameObject HeroesPanel;
    [SerializeField] private GameObject LevelPanel;
    [SerializeField] private GameObject GameMidAdPanel;
    [SerializeField] private GameObject BattleMidAdPanel;
    [SerializeField] private GameObject PopUpPanel;
    [SerializeField] private TextMeshProUGUI finishPanelLevelText;
    [SerializeField] private TextMeshProUGUI rewardGoldText;
    [SerializeField] private TextMeshProUGUI XPText;
    [SerializeField] private TextMeshProUGUI popUpText;
    [Header("Hero Panel")]
    [SerializeField] HeroSO[] heroes;
    [SerializeField] GameObject heroCardPrefab;
    [SerializeField] Transform heroTransform;

    [Header("Cards")]
    public Transform cardDisplayParent; 
    public GameObject cardPrefab;       
    public GameObject cardPanel;       
    public List<CardSO> allCardSOs;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        state = GameState.Menu;
        DisplayAllCards();
        HeroPanelUpdate();

    }
    public void GameUIStageChanged(GameState stage)
    {
        switch (stage)
        {
            case GameState.Menu:
                MenuPanel.SetActive(true);
                GamePanel.SetActive(false);
                LevelsPanel.SetActive(false);
                GameWinPanel.SetActive(false);
                GameLosePanel.SetActive(false);
                break;
            case GameState.Game:
                MenuPanel.SetActive(false);
                GamePanel.SetActive(true);
                LevelsPanel.SetActive(false);
                GameWinPanel.SetActive(false);
                GameLosePanel.SetActive(false);


                break;
            case GameState.Finish:
                MenuPanel.SetActive(false);
                GamePanel.SetActive(false);
                LevelsPanel.SetActive(false);
                GameWinPanel.SetActive(true);
                GameLosePanel.SetActive(false);
                break;

            default:
                break;
        }

    }
    public void WinFinishPanelOpen(int levelIndex, int rewardGold, int XP)
    {
        WinPanel.SetActive(true);

        // Panelin kendisine hafif bir büyüme efekti ekleyelim
        WinPanel.transform.localScale = Vector3.zero;  // Ýlk baþta küçük baþlat
        WinPanel.transform.DOScale(Vector3.one, 1.2f).SetEase(Ease.OutBack);  // Animasyonla büyüt

        // Level Text için hýzlý animasyon
        finishPanelLevelText.text = "0";  // Baþlangýç deðeri 0 olsun
        DOTween.To(() => int.Parse(finishPanelLevelText.text), x => finishPanelLevelText.text = x.ToString(), levelIndex, 2f);

        // Gold Text için animasyon
        rewardGoldText.text = "0";  // Baþlangýç deðeri 0 olsun
        DOTween.To(() => int.Parse(rewardGoldText.text), x => rewardGoldText.text = x.ToString(), rewardGold, 2f)
               .SetEase(Ease.OutQuad);  // Rahat bir animasyon ekleyelim

        // XP Text için animasyon
        XPText.text = "0";  // Baþlangýç deðeri 0 olsun
        DOTween.To(() => int.Parse(XPText.text), x => XPText.text = x.ToString(), XP, 2f)
               .SetEase(Ease.OutQuad);  // Rahat bir animasyon ekleyelim


        DataManager.instance.AddXP(XP);
        DataManager.instance.AddGold(rewardGold);
        DataManager.instance.AddEnergy(2);
    }
    public void LoseFinishPanelOpen()
    {
        LosePanel.SetActive(true);

        LosePanel.transform.localScale = Vector3.zero;  // Ýlk baþta küçük baþlat
        LosePanel.transform.DOScale(Vector3.one, 1.2f).SetEase(Ease.OutBack);  // Animasyonla büyüt
    }

    public void IncompleteLevelPanelOpen()
    {
        IncompleteLevelPanel.SetActive(true);

        IncompleteLevelPanel.transform.localScale = Vector3.zero;  // Ýlk baþta küçük baþlat
        IncompleteLevelPanel.transform.DOScale(Vector3.one, 1.2f).SetEase(Ease.OutBack);  // Animasyonla büyüt
    }
    public void FinishButton()
    {
        Bridge.advertisement.ShowInterstitial();
        SceneManager.LoadScene(0);
    }
    public void OpenCardsPanel()
    {
        

        if (cardPanel.activeSelf)
        {
            cardPanel.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).OnComplete(() => cardPanel.SetActive(false));

        }
        else
        {
            cardPanel.SetActive(true);
            cardPanel.transform.localScale = Vector3.zero;
            cardPanel.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);

        }
    }
    public void OpenQuestPanel()
    {
        if (QuestPanel.activeSelf)
        {
            QuestPanel.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).OnComplete(() => QuestPanel.SetActive(false));
        }
        else
        {
            QuestPanel.SetActive(true);
            QuestPanel.transform.localScale = Vector3.zero;
            QuestPanel.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);

        }
    }

    public void OpenShopPanel()
    {
        if (ShopPanel.activeSelf)
        {
            ShopPanel.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).OnComplete(() => ShopPanel.SetActive(false));
        }
        else
        {
            ShopPanel.SetActive(true);
            ShopPanel.transform.localScale = Vector3.zero;
            ShopPanel.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);

        }
    }

    public void OpenLevelPanel()
    {
        if (LevelPanel.activeSelf)
        {
            LevelPanel.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).OnComplete(() => LevelPanel.SetActive(false));
        }
        else
        {
            LevelPanel.SetActive(true);
            LevelPanel.transform.localScale = Vector3.zero;
            LevelPanel.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);

        }
    }

    public void OpenGameMidAdPanel()
    {
        if (GameMidAdPanel.activeSelf)
        {
            GameMidAdPanel.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).OnComplete(() => GameMidAdPanel.SetActive(false));
        }
        else
        {
            GameMidAdPanel.SetActive(true);
            GameMidAdPanel.transform.localScale = Vector3.zero;
            GameMidAdPanel.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);

        }
    }
    public void OpenBattleMidAdPanel()
    {
        if (BattleMidAdPanel.activeSelf)
        {
            BattleMidAdPanel.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).OnComplete(() => BattleMidAdPanel.SetActive(false));
        }
        else
        {
            BattleMidAdPanel.SetActive(true);
            BattleMidAdPanel.transform.localScale = Vector3.zero;
            BattleMidAdPanel.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);

        }
    }
    public void OpenHeroesPanel()
    {
        if (HeroesPanel.activeSelf)
        {
            HeroesPanel.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).OnComplete(() => HeroesPanel.SetActive(false));
        }
        else
        {
            HeroesPanel.SetActive(true);
            HeroesPanel.transform.localScale = Vector3.zero;
            HeroesPanel.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);

        }
    }

    public void OpenPopUp(string text)
    {
        if (PopUpPanel.activeSelf)
        {
            PopUpPanel.transform.DOScale(Vector3.zero, 0.2f)
                .SetEase(Ease.InBack)
                .OnComplete(() => PopUpPanel.SetActive(false));
        }
        else
        {
            PopUpPanel.SetActive(true);
            PopUpPanel.transform.localScale = Vector3.zero;
            popUpText.text = text;
            PopUpPanel.transform.DOScale(Vector3.one, 0.2f)
                .SetEase(Ease.OutBack)
                .OnComplete(() => StartCoroutine(ClosePanelAfterDelay(1.8f)));
        }
    }

    private IEnumerator ClosePanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (PopUpPanel.activeSelf)
        {
            PopUpPanel.transform.DOScale(Vector3.zero, 0.2f)
                .SetEase(Ease.InBack)
                .OnComplete(() => PopUpPanel.SetActive(false));
        }
    }


    private void DisplayAllCards()
    {
        foreach (CardSO cardSO in allCardSOs)
        {
            GameObject cardInstance = Instantiate(cardPrefab, cardDisplayParent);

            MenuCardUI cardScript=  cardInstance.gameObject.GetComponent<MenuCardUI>();

            cardScript.Config(cardSO);
            Button damageButton = cardScript.damageUpgradeButton;
            damageButton.onClick.AddListener(() => UpgradeCardDamage(cardSO,cardScript));

            //Button effectButton = cardScript.effectUpgradeButton;
            //effectButton.onClick.AddListener(() => UpgradeCardEffect(cardSO));

        }
    }
    public void HeroPanelUpdate()
    {
        for (int i = 0; i < heroes.Length; i++)
        {
           GameObject cardPrefabs= Instantiate(heroCardPrefab, heroTransform);

            HeroCard heroScript= cardPrefabs.gameObject.GetComponent<HeroCard>();

           cardPrefabs.GetComponent<HeroCard>().Config(
               heroes[i].name,
               heroes[i].health,
               heroes[i].numberOfMovesPerTurn,
               heroes[i].energySpentPerMatch,
               heroes[i].heroIcon,
               heroes[i]);

            Button healthButton= heroScript.healthUpgradeButton;
            healthButton.onClick.AddListener(() => UpgradeHeroHealth(heroes[i-1], heroScript));

        }
    }
    public void UpgradeCardDamage(CardSO cardData, MenuCardUI cardUI)
    {
        int currentDamage = cardData.GetCurrentDamage();
        int upgradeCost = cardData.GetUpgradeCost();

        Debug.Log(upgradeCost);

        if (currentDamage < 1)
        {
            Debug.Log($"{cardData.cardName} has insufficient damage to upgrade.");
            OpenPopUp("This card cannot be upgraded.");

            return;
        }

        if (DataManager.instance.TryPurchaseGold(upgradeCost))
        {
            cardData.UpgradeDamage();
            cardUI.UpdateCardUI();
            //SaveCardUpgrade(cardData);
        }
        else
        {
            OpenShopPanel();
        }
    }
    public void UpgradeHeroHealth(HeroSO heroData, HeroCard cardUI)
    {
        int currentHeath = heroData.GetCurrentHealth();
        int upgradeCost = heroData.GetUpgradeCost();

        Debug.Log(upgradeCost);

        if (DataManager.instance.TryPurchaseGold(upgradeCost))
        {
            heroData.UpgradeDamage();
            cardUI.UpdateUI();
        }
        else
        {
            OpenShopPanel();
        }
    }



    public void DiscordLink()
    {
        Application.OpenURL("https://discord.gg/npmtDMbfC3");
    }

}
