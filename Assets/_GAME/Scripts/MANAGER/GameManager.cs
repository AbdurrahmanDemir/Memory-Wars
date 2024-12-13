//using CrazyGames;
using DG.Tweening;
using Playgama;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
//using CrazyGames;



public class GameManager : MonoBehaviour
{
    [Header("Elements")]
    public LevelManager levelManager;
    EnemyController enemyController;
    public Transform cardParent;
    private Card firstSelectedCard;
    private Card secondSelectedCard;
    public GameObject cardPrefab;
    private List<CardSO> currentCards;
    public bool isProcessingCards = false;
    public bool isProcessingTrapCards = false;

    [Header("Player")]
    [SerializeField] private int playerHealth;
    [SerializeField] private TextMeshProUGUI playerHealthText;
    [SerializeField] private Image playerHealthIcon;
    [SerializeField] private Image playerHealthBackg;


    [Header("Move")]
    public TextMeshProUGUI movesText;
    private int movesRemaining = 2;
    [SerializeField] private Image movesIcon;
    [SerializeField] private Image movesBackg;

    public static Action OnMatchCard;

    private int totalPairs; // Leveldeki toplam çift sayýsý
    private int matchedPairs; // Eþleþen çift sayýsý

    public void GamePlay()
    {
        if (DataManager.instance.TryPurchaseEnergy(1))
        {
            UIManager.instance.GameUIStageChanged(GameState.Game);
            StartLevel(LevelManager.instance.LoadLevel());
            MoveUpdate(0);
            PlayerTakeDamage(0);
            Bridge.advertisement.ShowInterstitial();
        }
        else
        {
            Debug.Log("Enerjin yok");
            UIManager.instance.OpenGameMidAdPanel();
        }
    }

    public void StartLevel(int levelIndex)
    {
        if (currentCards != null)
        {
            currentCards.Clear();
        }

        Levels currentLevel = levelManager.levels[levelIndex];

        enemyController = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyController>();

       

        // Yeni seviyeyi oluþtur.
        Levels generatedLevel = levelManager.GenerateLevel(currentLevel.numberOfPairs, currentLevel.enemy.health);

        currentCards = new List<CardSO>(generatedLevel.cards);


        ShuffleCards(currentCards);
        InstantiateCards();

        totalPairs = currentLevel.numberOfPairs;
        matchedPairs = 0;

        movesRemaining = 2;
    }

    private void ShuffleCards(List<CardSO> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            CardSO temp = cards[i];
            int randomIndex = Random.Range(0, cards.Count);
            cards[i] = cards[randomIndex];
            cards[randomIndex] = temp;
        }
    }
    private void InstantiateCards()
    {
        foreach (CardSO cardSO in currentCards)
        {
            GameObject cardInstance = Instantiate(cardPrefab, cardParent); 
            Card cardScript = cardInstance.GetComponent<Card>();
            cardScript.cardData = cardSO;
            cardScript.gameManager = this;
        }
    }
    public void OnCardMatch(Card card)
    {
        if (isProcessingTrapCards) return;
        if (isProcessingCards) return;

        if (firstSelectedCard == null)
        {
            firstSelectedCard = card;

            if (card.cardData.cardEffect == CardEffect.Trap)
            {
                isProcessingTrapCards = true;
                ApplyCardEffect(firstSelectedCard);
                StartCoroutine(CloseTrapCard(firstSelectedCard));
            }
        }
        else if (secondSelectedCard == null)
        {
            secondSelectedCard = card;
            isProcessingCards = true; 
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {

        yield return new WaitForSeconds(0.5f);

        if (firstSelectedCard.cardData.cardName == secondSelectedCard.cardData.cardName)
        {
            PlayAttackEffect(firstSelectedCard.gameObject, secondSelectedCard.gameObject);
            ApplyCardEffect(firstSelectedCard);
            yield return new WaitForSeconds(1.2f);
            enemyController.TakeEnemyDamage(firstSelectedCard.cardData.GetCurrentDamage());
            firstSelectedCard.gameObject.SetActive(false);
            secondSelectedCard.gameObject.SetActive(false);

            matchedPairs++;

            OnMatchCard?.Invoke();

            if (matchedPairs == totalPairs && enemyController.GetEnemyCurrentHealth() > 0)
            {
                yield return new WaitForSeconds(1f); // Kýsa bir bekleme süresi
                UIManager.instance.GameUIStageChanged(GameState.Finish);
                UIManager.instance.IncompleteLevelPanelOpen();
            }

        }
        else
        {
            ShakeCards(firstSelectedCard.gameObject, secondSelectedCard.gameObject);

            yield return new WaitForSeconds(0.5f);

            firstSelectedCard.HideCard();  
            secondSelectedCard.HideCard();
            MoveUpdate(1);
        }

        firstSelectedCard = null;
        secondSelectedCard = null;

        

        if (movesRemaining <= 0)
        {
            EnemyAttack(); 
            yield return new WaitForSeconds(1.2f);
            PlayerTakeDamage(enemyController.GetComponent<EnemyController>().enemyData.damage);

            MoveUpdate(-2);
        }

        isProcessingCards = false;

    }
    private IEnumerator CloseTrapCard(Card trapCard)
    {
        yield return new WaitForSeconds(1f);
        trapCard.HideCard();
        firstSelectedCard = null;
        MoveUpdate(1);

        if (movesRemaining <= 0)
        {
            EnemyAttack();
            yield return new WaitForSeconds(1.5f);
            PlayerTakeDamage(enemyController.GetComponent<EnemyController>().enemyData.damage);

            MoveUpdate(-2);
        }

        isProcessingTrapCards = false;

    }
    private void ApplyCardEffect(Card card)
    {
        switch (card.cardData.cardEffect)
        {
            case CardEffect.None:
                break;
            case CardEffect.Trap:
                PlayerTakeDamage(card.cardData.effectValue);
                break;
            case CardEffect.Health:
                PlayerTakeDamage(-card.cardData.effectValue);
                break;
            case CardEffect.ExtraMoves:
                MoveUpdate(-card.cardData.effectValue);
                break;
            case CardEffect.Damage:
                enemyController.TakeEnemyDamage(card.cardData.effectValue);
                break;
            case CardEffect.Coin:
                DataManager.instance.AddGold(card.cardData.effectValue);
                break;
        }

    }
    private void ShakeCards(GameObject firstCard, GameObject secondCard)
    {
        firstCard.transform.DOShakePosition(0.5f, strength: new Vector3(10, 0, 0), vibrato: 10, randomness: 90, snapping: false, fadeOut: true);
        secondCard.transform.DOShakePosition(0.5f, strength: new Vector3(10, 0, 0), vibrato: 10, randomness: 90, snapping: false, fadeOut: true);
    }

    private void PlayAttackEffect(GameObject firstCard, GameObject secondCard)
    {
        Vector3 targetPosition = new Vector3(0, 300, 0);

        float moveDuration = 0.3f;  
        float attackDuration = 0.3f;

        firstCard.transform.DOMove(firstCard.transform.position + targetPosition, moveDuration).OnComplete(() =>
        {
            firstCard.transform.DOMoveY(firstCard.transform.position.x + 20f, attackDuration).SetLoops(2, LoopType.Yoyo);
        });

        secondCard.transform.DOMove(secondCard.transform.position + targetPosition, moveDuration).OnComplete(() =>
        {
            secondCard.transform.DOMoveY(secondCard.transform.position.x + 20f, attackDuration).SetLoops(2, LoopType.Yoyo);
        });
    }
    private void EnemyAttack()
    {
        enemyController.PlayEnemyAttackEffect(); 
    }
    public void PlayerTakeDamage(int damage)
    {
        playerHealth -= damage;
        if(playerHealth <= 0)
        {
            UIManager.instance.GameUIStageChanged(GameState.Finish);
            UIManager.instance.LoseFinishPanelOpen();

        }
        playerHealthText.text = playerHealth.ToString();
        playerHealthIcon.transform.DOShakePosition(0.5f, strength: new Vector3(10, 0, 0), vibrato: 10, randomness: 90, snapping: false, fadeOut: true);

        Color originalColor = playerHealthBackg.color;
        playerHealthBackg.DOColor(Color.green, 0.3f).OnComplete(() =>
        {
            playerHealthBackg.DOColor(originalColor, 0.5f);
        });

    }

    public void MoveUpdate(int damage)
    {
        movesRemaining -= damage;
        movesText.text = movesRemaining.ToString();
        movesIcon.transform.DOShakePosition(0.5f, strength: new Vector3(10, 0, 0), vibrato: 10, randomness: 90, snapping: false, fadeOut: true);

        Color originalColor = movesBackg.color;
        movesBackg.DOColor(Color.blue, 0.3f).OnComplete(() =>
        {
            movesBackg.DOColor(originalColor, 0.5f);
        });

    }
}
