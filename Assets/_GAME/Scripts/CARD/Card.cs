using DG.Tweening.Core.Easing;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CardSO cardData;
     public GameManager gameManager;

    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI cardDamageText;
    [SerializeField] private Image cardIconImage;
    [SerializeField] private GameObject cardBackImage;
    [SerializeField] private GameObject cardFrontImage;

    public bool isFlipped = false;
    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;

        cardFrontImage.gameObject.SetActive(false);
        cardBackImage.gameObject.SetActive(true);

        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
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
        if (!isFlipped && !gameManager.isProcessingCards && !gameManager.isProcessingTrapCards)
        {
            FlipCard();
            gameManager.OnCardMatch(this);
            transform.DOScale(originalScale * 1.1f, 0.2f);
        }
    }

    public void FlipCard()
    {
        isFlipped = true;

        transform.DORotate(new Vector3(0, 90, 0), 0.25f).OnComplete(() =>
        {
            cardBackImage.gameObject.SetActive(false);
            cardFrontImage.gameObject.SetActive(true);

            cardNameText.text = cardData.cardName;
            cardDamageText.text = cardData.damage.ToString();
            cardIconImage.sprite = cardData.cardImage;

            transform.DORotate(new Vector3(0, 0, 0), 0.25f);
        });
    }

    public void HideCard()
    {
        isFlipped = false;
        transform.DORotate(new Vector3(0, 90, 0), 0.25f).OnComplete(() =>
        {
            cardFrontImage.gameObject.SetActive(false);
            cardBackImage.gameObject.SetActive(true);

            transform.DORotate(new Vector3(0, 0, 0), 0.25f);
        });
    }

}
