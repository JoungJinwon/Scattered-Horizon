using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamController : MonoBehaviour
{
    [Header("Cinemachine Virtual Cameras")]
    public CinemachineVirtualCamera Camera_01;  // 플레이씬에서 1인칭 카메라로 사용된다.
    public CinemachineVirtualCamera Camera_02;  // 플레이씬에서 3인칭 카메라로 사용된다.

    public bool isCutScene;

    public static bool isFirstPersonView = true;  // 현재 시점이 1인칭인가?
    private bool isSwitching = false; // 카메라 시점 전환이 진행 중인가?
    private float switchCamTimeout = 0.5f;


    // private void Awake()
    // {
    // }

    private void Start()
    {
        isFirstPersonView = true;
    }

    private void Update()
    {
        if (GameManager.Instance._input.switchView && !isSwitching)
        {
            GameManager.Instance._input.switchView = false;
            isSwitching = true;
            StartCoroutine(SwitchPerspective());
        }
    }

    // 카메라 시점 전환 코루틴
    private IEnumerator SwitchPerspective()
    {
        if (isFirstPersonView)
        {
            isFirstPersonView = false;
            Camera_01.Priority = 5;
            Camera_02.Priority = 10;
        }
        else
        {
            isFirstPersonView = true;
            Camera_01.Priority = 10;
            Camera_02.Priority = 5;
        }

        yield return new WaitForSeconds(switchCamTimeout);
        
        isSwitching = false;
    }

/// <summary>
/// Scene Director 클래스와 연계된다. 각 컷씬에 해당되는 카메라 컨트롤을 사용한다.
/// </summary>
#region Camera Control
    public void CamControl_CS01()
    {
        Camera_01.Priority = 0;
        Camera_02.Priority = 1;
    }

    
#endregion
}
