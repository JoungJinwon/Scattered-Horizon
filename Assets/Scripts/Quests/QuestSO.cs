using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/QuestScriptableObject", order = 1)]
public class QuestSO : ScriptableObject
{
    [Header("퀘스트 고유 ID")]
    public int questId;
    [Header("퀘스트명")]
    public string questName;
    [Header("퀘스트 설명")]
    public string questDescrption;
    
    [Header("목표 리스트")]
    public List<ObjectiveBase> objectives;
    [Header("수집 목표")] [SerializeField]
    public ObjectiveCollect[] objCollects;
    [Header("건설 목표")] [SerializeField]
    public ObjectiveBuild[] objBuilds;

    public void QuestListInit()
    {
        for (int i = 0; i < objCollects.Length; i++)
            objectives.Add(objCollects[i]);
        for (int i = 0; i < objCollects.Length; i++)
            objectives.Add(objBuilds[i]);
    }

    public bool IsQuestCompleted()
    {
        for (int i = 0; i < objectives.Count; i++)
        {
            if (!objectives[i].CheckCompletion())
                return false;
        }

        return true;
    }
}