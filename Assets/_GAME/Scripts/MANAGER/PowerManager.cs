using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PowerManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    public float revealDuration = 2f; 

    public static Action OnPowerUse;

    public void UseRevealPowerUp()
    {
        if (DataManager.instance.TryPurchaseGold(20))
        {
            Card unrevealedCard = GetRandomUnrevealedCard();

            if (unrevealedCard != null)
            {
                StartCoroutine(RevealCardTemporarily(unrevealedCard));
                OnPowerUse?.Invoke();
            }
            else
            {
                Debug.Log("No unrevealed cards left to reveal.");
            }
        }
        else
        {
            UIManager.instance.OpenBattleMidAdPanel();
        }

    }

    private Card GetRandomUnrevealedCard()
    {
        List<Card> unrevealedCards = new List<Card>();

        foreach (Transform cardTransform in gameManager.cardParent)
        {
            Card card = cardTransform.GetComponent<Card>();
            if (card != null && !card.isFlipped) 
            {
                unrevealedCards.Add(card);
            }
        }

        if (unrevealedCards.Count > 0)
        {
            int randomIndex = Random.Range(0, unrevealedCards.Count);
            return unrevealedCards[randomIndex];
        }
        else
        {
            return null;
        }
    }

    private IEnumerator RevealCardTemporarily(Card card)
    {
        card.FlipCard(); 
        yield return new WaitForSeconds(revealDuration);

        card.HideCard(); 
    }
    public void HealthPower()
    {
        if (DataManager.instance.TryPurchaseGold(10))
        {
            gameManager.PlayerTakeDamage(-2);
            OnPowerUse?.Invoke();
        }
        else
        {
            UIManager.instance.OpenBattleMidAdPanel();
        }
    }
    public void MovePower()
    {
        if (DataManager.instance.TryPurchaseGold(15))
        {
            gameManager.MoveUpdate(-3);
            OnPowerUse?.Invoke();
        }
        else
        {
            UIManager.instance.OpenBattleMidAdPanel();
        }
    }

}
