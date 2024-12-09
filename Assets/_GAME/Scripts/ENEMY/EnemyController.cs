using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [Header("Elements")]
    public EnemySO enemyData;


    [Header("Settings")]
    private int enemyHealth;
    private int enemyDamage;
    private int enemycurrentHealth;
    public TextMeshProUGUI enemyNameText;
    public TextMeshProUGUI enemyHealthText;
    public TextMeshProUGUI enemyDamageText;
    public Image enemyProfile;
    [SerializeField] private Slider enemyHealthSlider;  

    public static Action OnEnemyDied;
    private void Start()
    {
        enemyData = LevelManager.instance.levels[LevelManager.instance.LoadLevel()].enemy;

        enemyNameText.text = enemyData.enemyName;

        enemyDamage = enemyData.damage;
        enemyDamageText.text = enemyDamage.ToString();
        enemyProfile.sprite = enemyData.enemyIcon;
        enemyHealth = enemyData.health;
        enemycurrentHealth = enemyHealth;
        enemyHealthText.text = enemycurrentHealth.ToString();
        enemyHealthSlider.maxValue= enemyHealth;
        enemyHealthSlider.value = enemycurrentHealth;
    }

    public void TakeEnemyDamage(int damage)
    {
        enemycurrentHealth -= damage;
        enemyHealthText.text = enemycurrentHealth.ToString();
        enemyHealthSlider.value = enemycurrentHealth;
        EnemyTakeDamageEffect();

        if (enemycurrentHealth <= 0)
        {
            int newLevel= LevelManager.instance.LoadLevel();
            newLevel++;
            UIManager.instance.GameUIStageChanged(GameState.Finish);

            LevelManager.instance.SaveLevel(newLevel);
            UIManager.instance.WinFinishPanelOpen(newLevel, newLevel * 5, newLevel * 3);

            OnEnemyDied?.Invoke();

        }
    }
    private void EnemyTakeDamageEffect()
    {
        float shakeDuration = 0.5f;
        float shakeStrength = 0.5f;

        Image enemySprite = gameObject.GetComponentInChildren<Image>();
        Color originalColor = enemySprite.color;

        enemySprite.DOColor(Color.red, 0.1f).OnComplete(() =>
        {
            enemySprite.DOColor(originalColor, 0.5f);
        });

        gameObject.transform.DOShakePosition(shakeDuration, shakeStrength);
    }
    public int GetEnemyCurrentHealth()
    {
        return enemycurrentHealth;
    }
    public void PlayEnemyAttackEffect()
    {
        Debug.Log("Düþman saldýrýyor!");

        transform.DOMoveY(transform.position.y - 500f, 0.5f).SetLoops(2, LoopType.Yoyo);
    }

}
