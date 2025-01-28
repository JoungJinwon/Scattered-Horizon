using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ObjectiveBase
{
    public string objectiveName;
    public bool isCompleted;

    // Common method to check completion
    // Quest Manager에서 퀘스트 완료 여부를 체크할 때 사용
    public abstract bool CheckCompletion();
}
