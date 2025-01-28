using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDirector : MonoBehaviour
{
    private int gameContext = 0;
    /*
    Game Context    
    0: 컷씬 >> Gray Point로 이동하며 대화를 나누는 Avon과 Hailey
    1: 컷씬 >> Gray Point에 착륙하는 Nova Odyssey
    2: 플레이씬 >> 튜토리얼
    3: 플레이씬 >> 테라포밍
    4: 컷씬 >> 동면에 들어가는 Avon
    5: 플레이씬 >> 테라포밍 완료 후 Gray Point를 떠나는 Avon
    6: 컷씬 >> 이륙하는 Nova Odyssey, 지구의 전경이 보인다
    */

    // Singletone instance
    public static SceneDirector Instance { get; private set;}
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }


    /*
        대화가 종료되거나, 퀘스트를 완료하면 gameContext를 증가시킨다
        증가된 gameContext에 따라 수행될 코루틴이 결정된다
        각 코루틴에서는 일정 시간 이후 씬을 로드하거나, 게임 진행 상황에 따라 필요한 데이터를 미리 불러온다
        또한 퀘스트 진행 상황에 따라 
    */
    public void IncreaseGameContext()
    {
        gameContext++;

        switch (gameContext)
        {
            // case 0:
            //     StartCoroutine(context_Cutscene_1());
            //     break;
            case 1:
                StartCoroutine(context_Cutscene1_End());
                break;
            case 2:
                StartCoroutine(context_Cutscene_2());
                break;
            case 3:
                StartCoroutine(context_PlayScene_Tutorial());
                break;
            default:
                Debug.LogWarning("유효하지 않은 Game Context입니다.");
                break;
        }
    }

    public int GetGameContext()
    {
        return gameContext;
    }

// 씬 진행, 전환 시 처리를 담당한다
#region Context
    private IEnumerator context_Cutscene1_End()
    {   // context = 1
        // 컷씬 0 대화 종료 후 Nova Odyssey 착륙씬을 로드한다
        Debug.Log("Scene Director: 시나리오[컷씬 1] End");

        rocketController rc = FindObjectOfType<rocketController>();
        rc.rocketSpeed = 100f;
        CamController cmCtrler = FindObjectOfType<CamController>();
        cmCtrler.CamControl_CS01();

        yield return new WaitForSecondsRealtime(3f);
        GameManager.Instance.LoadScene(2);
    }

    private IEnumerator context_Cutscene_2()
    {   // context = 2
        // Nova Odyssey가 PlanetX에 착륙씬을 보여주고 PlanetX 플레이씬을 로드한다.
        Debug.Log("Scene Director: 시나리오[컷씬 2] End");
        yield return null;
        GameManager.Instance.LoadScene(3);
    }

    private IEnumerator context_PlayScene_Tutorial()
    {   // context = 3
        // 
        Debug.Log("Scene Director: 시나리오[튜토리얼] Play");
        
        yield return null;
    }
#endregion

}
