using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectiveCollect : ObjectiveBase
{
    [Header("아이템 코드")]
    public int itemId;
    [Header("요구 개수")]
    public int requiredAmount;
    private int currentAmount; // 현재 아이템 개수

    public void UpdateProgress(int amount)
    {
        currentAmount += amount;
        if (currentAmount >= requiredAmount)
        {
            isCompleted = true;
        }
    }

    public override bool CheckCompletion()
    {
        return isCompleted;
    }
}
