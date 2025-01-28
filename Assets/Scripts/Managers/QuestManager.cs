using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    // 진행중인 모든 퀘스트 데이터 리스트
    public List<QuestSO> currentQuests;

    public void AddQuest(QuestSO newQuest)
    {
        if (!currentQuests.Contains(newQuest))
        {
            currentQuests.Add(newQuest);
        }
    }

    public void CheckQuestProgress()
    {
        for (int i = 0; i < currentQuests.Count; i++)
        {
            if (currentQuests[i].IsQuestCompleted())
            {
                Debug.Log($"Quest Manager: 퀘스트 완료[ {currentQuests[i].questName} ]");
                // Handle quest completion logic here
            }
            else
            {
                Debug.Log("Quest Manager: 퀘스트가 아직 완료되지 않았습니다");
            }
        }
    }

    public void UpdateBuildObjective(int buildingId)
    {
        for (int i = 0; i < currentQuests.Count; i++)
        {
            QuestSO quest = currentQuests[i];
            foreach (var objective in quest.objectives)
            {
                if (objective is ObjectiveBuild buildObjective && buildObjective.buildingId == buildingId)
                {
                    buildObjective.MarkAsBuilt();
                    Debug.Log($"Objective Completed: {buildObjective.objectiveName}");
                }
            }
        }
    }
}
