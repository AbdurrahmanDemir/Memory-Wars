using DG.Tweening;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuCardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI cardDamageText;
    [SerializeField] private TextMeshProUGUI cardUpgradeCostText;
    [SerializeField] private Image cardIconImage;
    [SerializeField] private GameObject cardBackImage;
    [SerializeField] private GameObject cardFrontImage;
    private bool isFlipped = false;
    [SerializeField] private TextMeshProUGUI cardEffectText;
    private CardSO cardData;

    public Button damageUpgradeButton;
    public Button effectUpgradeButton;

    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;


        cardFrontImage.gameObject.SetActive(true);
        cardBackImage.gameObject.SetActive(false);

    }
    public void Config(CardSO cardSO)
    {
        cardData = cardSO;

        cardNameText.text = cardSO.cardName;
        cardDamageText.text = cardSO.GetCurrentDamage().ToString();
        cardIconImage.sprite = cardSO.cardImage;
        cardUpgradeCostText.text = cardSO.GetUpgradeCost().ToString();
    }
    public void UpdateCardUI()
    {
        cardDamageText.text =  cardData.GetCurrentDamage().ToString();
        cardUpgradeCostText.text = cardData.GetUpgradeCost().ToString();

        //cardEffectText.text = cardData.effectValue.ToString();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(originalScale * 1.1f, 0.2f);
    }

    // Fare karttan ayrýldýðýnda
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(originalScale, 0.2f);
    }
    public void OnCardClick()
    {
        if (!isFlipped)
        {
            FlipCard();
            transform.DOScale(originalScale * 1.1f, 0.2f);
        }else
            HideCard();
    }
    private void FlipCard()
    {
        isFlipped = true;

        transform.DORotate(new Vector3(0, 90, 0), 0.25f).OnComplete(() =>
        {
            cardBackImage.gameObject.SetActive(true);
            cardFrontImage.gameObject.SetActive(false);

            Config(cardData);

            if (!string.IsNullOrEmpty(cardData.cardEffectDescription))
            {
                cardEffectText.text = cardData.cardEffectDescription;
            }
            else
            {
                cardEffectText.text = "No extra features.";
            }


            transform.DORotate(new Vector3(0, 0, 0), 0.25f);
        });
    }

    public void HideCard()
    {
        isFlipped = false;
        transform.DORotate(new Vector3(0, 90, 0), 0.25f).OnComplete(() =>
        {
            cardBackImage.gameObject.SetActive(false);
            cardFrontImage.gameObject.SetActive(true);

            transform.DORotate(new Vector3(0, 0, 0), 0.25f);
        });
    }

}
