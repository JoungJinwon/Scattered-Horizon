using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectiveBuild : ObjectiveBase
{
    [Header("건설물 코드")]
    public int buildingId;
    [Header("요구 건설 개수")]
    public int buildingNum;
    public bool isBuilt; // 건설 여부

    public void MarkAsBuilt()
    {
        isBuilt = true;
        isCompleted = true;
    }

    public override bool CheckCompletion()
    {
        return isCompleted;
    }
}
