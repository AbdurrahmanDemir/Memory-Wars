using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField] private int currentLevel;
    public Levels[] levels;
    public GameObject[] levelButtons;
    public List<CardSO> allCards; // T�m mevcut kartlar.
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        LoadLevel();

        int currentLevel = PlayerPrefs.GetInt("Level", 0);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            // E�er bu buton daha �nce tamamlanm��sa, ye�il yap ve t�klanamaz hale getir
            if (i < currentLevel)
            {
                //levelButtons[i].gameObject.GetComponent<Button>().interactable = false;
                levelButtons[i].gameObject.GetComponent<Image>().color = Color.green; // Buton rengini ye�il yap
            }
            // E�er bu buton tamamlanabilir seviyedeyse, t�klanabilir yap
            else if (i == currentLevel)
            {
                //levelButtons[i].gameObject.GetComponent<Button>().interactable = true;
                levelButtons[i].gameObject.GetComponent<Image>().color = Color.white; // Varsay�lan buton rengi
            }
            // Gelecek seviyeler kilitli, t�klanamaz olmal�
            else
            {
                //levelButtons[i].gameObject.GetComponent<Button>().interactable = false;
                levelButtons[i].gameObject.GetComponent<Image>().color = Color.gray; // Kilitli seviye rengi
            }
        }
    }


    public Levels GenerateLevel(int numberOfPairs, int enemyHealth)
    {
        Levels generatedLevel = new Levels();
        List<CardSO> selectedCards = new List<CardSO>();
        int totalDamage = 0;

        // Kartlar� kar��t�r ve hasarl� ve hasars�z olarak grupland�r.
        List<CardSO> shuffledCards = new List<CardSO>(allCards);
        ShuffleList(shuffledCards);

        List<CardSO> damageCards = shuffledCards.Where(card => card.GetCurrentDamage() > 0).ToList();
        List<CardSO> zeroDamageCards = shuffledCards.Where(card => card.GetCurrentDamage() == 0).ToList();

        // �ncelikle d��man�n can�n� ge�mek i�in gerekli kartlar� se�.
        foreach (CardSO card in damageCards)
        {
            if (selectedCards.Count >= numberOfPairs) break;

            int cardDamage = card.GetCurrentDamage();

            selectedCards.Add(card);
            totalDamage += cardDamage;

            if (totalDamage >= enemyHealth) break;
        }

        // E�er hala yeterli �ift yoksa d���k hasarl� veya 0 hasarl� kartlar ekle.
        while (selectedCards.Count < numberOfPairs)
        {
            //if (damageCards.Count > 0)
            //{
            //    selectedCards.Add(damageCards[0]); // D���k hasarl� bir kart ekle.
            //    damageCards.RemoveAt(0);
            //}
            if (zeroDamageCards.Count > 0)
            {
                selectedCards.Add(zeroDamageCards[0]); // 0 hasarl� bir kart ekle.
                zeroDamageCards.RemoveAt(0);
            }
            else
            {
                Debug.LogWarning("No more cards available to generate the level!");
                break;
            }
        }

        // �iftler olu�turmak i�in listeyi ikiye katla.
        List<CardSO> pairedCards = new List<CardSO>();
        foreach (CardSO card in selectedCards)
        {
            pairedCards.Add(card);
            pairedCards.Add(card);
        }

        ShuffleList(pairedCards); // �iftleri tekrar kar��t�r.

        generatedLevel.cards = pairedCards.ToArray();
        generatedLevel.enemy = levels[currentLevel].enemy; // Mevcut seviyedeki d��man� kullan.

        return generatedLevel;
    }



    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(0, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    public int LoadLevel()
    {
        currentLevel = PlayerPrefs.GetInt("Level", 0);
        return currentLevel;
    }
    public void SaveLevel(int levelIndex)
    {
        currentLevel = levelIndex;
        PlayerPrefs.SetInt("Level", currentLevel);
        Debug.Log("New Game Level" + currentLevel);
    }
}

[Serializable]
public struct Levels
{
    public EnemySO enemy;
    public int numberOfPairs;
    public CardSO[] cards;
}