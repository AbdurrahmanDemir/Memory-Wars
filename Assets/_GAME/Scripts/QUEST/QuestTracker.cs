using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuestTracker : MonoBehaviour
{
    private QuestManager questManager;
    private void Awake()
    {
        questManager = GetComponent<QuestManager>();

        EnemyController.OnEnemyDied += EnemyDiedCallback;
        PowerManager.OnPowerUse += UsePowerCallback;
        GameManager.OnMatchCard += MatchCardCallback;

    }
    private void OnDestroy()
    {
        EnemyController.OnEnemyDied -= EnemyDiedCallback;
        PowerManager.OnPowerUse -= UsePowerCallback;
        GameManager.OnMatchCard -= MatchCardCallback;
    }

    private void EnemyDiedCallback()
    {
        Dictionary<int, Quest> quests = new Dictionary<int, Quest>(questManager.GetCurrentQuest());

        foreach (KeyValuePair<int, Quest> questData in quests)
        {
            Quest quest = questData.Value;

            if (quest.Type == QuestType.Kill)
            {
                int currentAdsWatch = (int)(quest.progress * quest.target);
                currentAdsWatch++;

                float newProgress = (float)currentAdsWatch / quest.target;

                questManager.UpdateQuestProgress(questData.Key, newProgress);
            }
        }
    }
    private void MatchCardCallback()
    {
        Dictionary<int, Quest> quests = new Dictionary<int, Quest>(questManager.GetCurrentQuest());

        foreach (KeyValuePair<int, Quest> questData in quests)
        {
            Quest quest = questData.Value;

            if (quest.Type == QuestType.MatchCard)
            {
                int currentAdsWatch = (int)(quest.progress * quest.target);
                currentAdsWatch++;

                float newProgress = (float)currentAdsWatch / quest.target;

                questManager.UpdateQuestProgress(questData.Key, newProgress);
            }
        }
    }
    private void UsePowerCallback()
    {
        Dictionary<int, Quest> quests = new Dictionary<int, Quest>(questManager.GetCurrentQuest());

        foreach (KeyValuePair<int, Quest> questData in quests)
        {
            Quest quest = questData.Value;

            if (quest.Type == QuestType.UsePower)
            {
                int currentAdsWatch = (int)(quest.progress * quest.target);
                currentAdsWatch++;

                float newProgress = (float)currentAdsWatch / quest.target;

                questManager.UpdateQuestProgress(questData.Key, newProgress);
            }
        }
    }
   
}
